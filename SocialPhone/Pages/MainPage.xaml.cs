using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Threading.Tasks;
using SocialPhone.Services;
using SocialPhone.UserControls;
using SocialPhone.ViewModels;

namespace SocialPhone.Pages
{
    public partial class MainPage
    {
        private MainPageViewModel model;
        private bool loading;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (model == null)
            {
                model = new MainPageViewModel();
                DataContext = model;

                if (string.IsNullOrWhiteSpace(Service.Settings.Username))
                {
                    ShowLogin();
                }
                else if (Service.Settings.CurrentStreamId == 0)
                {
                    LoadDefaultStream();
                }
                else
                {
                    model.CurrentStreamName = Service.Settings.CurrentStreamName;
                    LoadMessages(true);
                }
            }
        }

        private LoginControl loginControl;
        private void ShowLogin()
        {
            loginControl = new LoginControl(Service);
            loginControl.OnLoginSuccess += loginControl_OnLoginSuccess;
            LayoutRoot.Children.Add(loginControl);
        }

        void loginControl_OnLoginSuccess(object sender, EventArgs e)
        {
            LayoutRoot.Children.Remove(loginControl);
            loginControl = null;
            LoadDefaultStream();
        }

        private async Task LoadDefaultStream()
        {
            Progress.IsIndeterminate = true;
            var result = await Service.Socialcast.GetStreamsAsync();

            if (result.HasError())
            {
            }
            else
            {
                var defaultStream = result.Value.streams.Single(s => s.Default);
                await LoadStream(defaultStream);
            }

            Progress.IsIndeterminate = false;
        }

        private async Task ShowStreams()
        {
            Progress.IsIndeterminate = true;
            model.StreamSelectorVisibility = Visibility.Visible;

            var result = await Service.Socialcast.GetStreamsAsync();

            if (result.HasError())
            {
            }
            else
            {
                model.Streams.Clear();

                foreach(var stream in result.Value.streams)
                {
                    model.Streams.Add(stream);
                }
            }

            Progress.IsIndeterminate = false;
        }

        private async Task LoadMessages(bool refresh = false)
        {
            if (loading) return;
            loading = true;
            Progress.IsIndeterminate = true;

            if (refresh)
            {
                model.CurrentPage = 1;
                model.Messages.Clear();
            }

            var result = await Service.Socialcast.GetMessagesAsync(Service.Settings.CurrentStreamId, 5, model.CurrentPage++);

            if (result.HasError())
            {
                model.CurrentPage--;
            }
            else
            {
                foreach (var message in result.Value.messages)
                {
                    model.Messages.Add(new ScMessage
                        {
                            Id = message.id,
                            Body = message.body.CutEnd(400),
                            Title = message.user.name + (message.groups.Count > 0 ? " > " + string.Join(", ", message.groups.Select(g => g.name)) : string.Empty),
                            Likes = message.likes_count,
                            Comments = message.comments_count,
                            Status = message.created_at.ToRelativeDate(),
                            UserAvatarUrl = message.user.avatars.square140
                        });
                }
            }

            loading = false;
            Progress.IsIndeterminate = false;
        }


        private void LoadMoreMessages_Link(object sender, LinkUnlinkEventArgs e)
        {
            var message = e.ContentPresenter.Content as ScMessage;
            var index = model.Messages.IndexOf(message);

            if (index + 1 == model.Messages.Count)
                LoadMessages();
        }

        private void UpdateStreamClick(object sender, EventArgs e)
        {
            LoadMessages(true);
        }

        private void StreamClicked(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var stream = ((LongListSelector)sender).SelectedItem as Services.Stream;
            LoadStream(stream);
        }

        private async Task LoadStream(Stream stream)
        {
            Service.Settings.CurrentStreamId = stream.Id;
            Service.Settings.CurrentStreamName = stream.Name;
            model.CurrentStreamName = stream.Name;
            model.StreamSelectorVisibility = Visibility.Collapsed;
            await LoadMessages(true);
        }

        private void ShowStreamsClick(object sender, EventArgs e)
        {
            ShowStreams();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (model.StreamSelectorVisibility == Visibility.Visible)
            {
                model.StreamSelectorVisibility = Visibility.Collapsed;
                e.Cancel = true;
            }
        }

        private void MessageClick(object sender, RoutedEventArgs e)
        {
            var message = ((FrameworkElement)sender).DataContext as ScMessage;
            NavigationService.Navigate(new Uri("/Pages/MessagePage.xaml?id=" + message.Id, UriKind.Relative));
        }
    }

    public class ScMessage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Body { get; set; }
        public int Comments { get; set; }
        public int Likes { get; set; }
        public string UserAvatarUrl { get; set; }
    }
}