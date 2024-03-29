﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Threading.Tasks;
using SocialPhone.Models.Socialcast;
using SocialPhone.UserControls;
using SocialPhone.ViewModels;
using Message = SocialPhone.ViewModels.Socialcast.Message;
using SocialPhone.Services;

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

                if (Service.Settings.AuthUser == null)
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

            var result = await (
                Service.Settings.StreamMode == StreamMode.Stream
                ? Service.Socialcast.GetMessagesAsync(Service.Settings.CurrentStreamId, 5, model.CurrentPage++)
                : Service.Socialcast.GetMessagesByTopicAsync(Service.Settings.CurrentTopic, 5, model.CurrentPage++)
                );

            if (result.HasError())
            {
                MessageBox.Show("Failed to load stream");
                model.CurrentPage--;
            }
            else
            {
                foreach (var message in result.Value.messages)
                {
                    model.Messages.Add(new Message
                    {
                        Id = message.id,
                        Body = message.body.CutEnd(400),
                        Header = message.user.name + (message.groups.Count > 0 ? " > " + string.Join(", ", message.groups.Select(g => g.name)) : string.Empty),
                        Title = message.title,
                        Likes = message.likes_count,
                        Comments = message.comments_count,
                        Status = message.created_at.ToRelativeDate(),
                        UserAvatarUrl = message.user.avatars.square140,
                        Likeable = message.likable,
                        LikedByMe = message.likes.SingleOrDefault(l => l.user.id == Service.Settings.AuthUser.Id)
                    });
                }
            }

            loading = false;
            Progress.IsIndeterminate = false;
        }


        private void LoadMoreMessages_Link(object sender, LinkUnlinkEventArgs e)
        {
            var message = (Message)e.ContentPresenter.Content;
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
            var stream = (Stream)((LongListSelector)sender).SelectedItem;
            LoadStream(stream);
        }

        private async Task LoadStream(Stream stream)
        {
            Service.Settings.CurrentStreamId = stream.Id;
            Service.Settings.CurrentStreamName = stream.Name;
            Service.Settings.StreamMode = StreamMode.Stream;
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
            else if (messageControl != null)
            {
                RemoveMessageControl();
                e.Cancel = true;
            }
        }

        private void MessageClick(object sender, RoutedEventArgs e)
        {
            var message = (Message)((FrameworkElement)sender).DataContext;
            NavigationService.Navigate(new Uri("/Pages/MessagePage.xaml?id=" + message.Id, UriKind.Relative));
        }


        private MessageControl messageControl;

        private void NewPostClick(object sender, EventArgs e)
        {
            messageControl = new MessageControl();
            messageControl.OnButtonClick += messageControl_OnButtonClick;
            messageControl.SetValue(Grid.RowSpanProperty, 2);
            LayoutRoot.Children.Add(messageControl);
            ApplicationBar.IsVisible = false;
        }

        private void messageControl_OnButtonClick(object sender, TextEventArgs e)
        {
            RemoveMessageControl();
        }

        private void RemoveMessageControl()
        {
            LayoutRoot.Children.Remove(messageControl);
            messageControl.OnButtonClick += messageControl_OnButtonClick;
            messageControl = null;
            ApplicationBar.IsVisible = true;
        }

        private void SetupMenuItems(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = (ContextMenu)sender;
            foreach(var item in menu.Items.OfType<MenuItem>().Where(i => i.Header == "Like")) {
                 Helpers.LikeHelper.AttachClickEvent(item);
            }
            foreach (var item in menu.Items.OfType<MenuItem>())
            {
                if (item.Header.ToString().StartsWith("#"))
                {
                    item.Click += (s, re) =>
                    {
                        Service.Settings.CurrentTopic = item.Header.ToString().Replace("#", "");
                        Service.Settings.StreamMode = StreamMode.Topic;
                        Service.Settings.CurrentStreamName = item.Header.ToString();
                        model.CurrentStreamName = Service.Settings.CurrentStreamName;
                        LoadMessages(true);
                    };
                }
            }
        }
    }
}