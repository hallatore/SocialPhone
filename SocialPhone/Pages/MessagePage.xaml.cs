using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using SocialPhone.ViewModels;
using System.Threading.Tasks;
using SocialPhone.ViewModels.Socialcast;

namespace SocialPhone.Pages
{
    public partial class MessagePage
    {
        private MessagePageViewModel model;

        public MessagePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (model == null)
            {
                model = new MessagePageViewModel();
                model.MessageId = Convert.ToInt32(NavigationContext.QueryString["id"]);
                DataContext = model;
                LoadMessage();
            }
        }

        private async Task LoadMessage()
        {
            Progress.IsIndeterminate = true;

            var result = await Service.Socialcast.GetMessageAsync(model.MessageId);

            model.Messages.Add(new Message
            {
                Id = result.Value.message.id,
                Body = result.Value.message.body,
                Header = result.Value.message.user.name + (result.Value.message.groups.Count > 0 ? " > " + string.Join(", ", result.Value.message.groups.Select(g => g.name)) : string.Empty),
                Title = result.Value.message.title,
                Likes = result.Value.message.likes_count,
                Status = result.Value.message.created_at.ToRelativeDate(),
                UserAvatarUrl = result.Value.message.user.avatars.square140,
                Likeable = result.Value.message.likable,
                LikedByMe = result.Value.message.likes.SingleOrDefault(l => l.user.id == Service.Settings.AuthUser.Id)
            });

            foreach (var comment in result.Value.message.comments)
            {
                model.Messages.Add(new Message
                {
                    Id = comment.id,
                    ParentId = result.Value.message.id,
                    Body = comment.text,
                    Title = comment.user.name,
                    Likes = comment.likes_count,
                    Status = comment.created_at.ToRelativeDate(),
                    UserAvatarUrl = comment.user.avatars.square140,
                    Type = MessageType.Comment,
                    Likeable = comment.likable,
                    LikedByMe = comment.likes.SingleOrDefault(l => l.user.id == Service.Settings.AuthUser.Id)
                });
            }

            Progress.IsIndeterminate = false;
        }

        private void LikeMenuItemLoaded(object sender, RoutedEventArgs e)
        {
            Helpers.LikeHelper.AttachClickEvent((MenuItem)sender);
        }
    }
}