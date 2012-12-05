using System;
using System.Linq;
using System.Windows.Navigation;
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
                Title = result.Value.message.user.name + (result.Value.message.groups.Count > 0 ? " > " + string.Join(", ", result.Value.message.groups.Select(g => g.name)) : string.Empty),
                Likes = result.Value.message.likes_count,
                Status = result.Value.message.created_at.ToRelativeDate(),
                UserAvatarUrl = result.Value.message.user.avatars.square140
            });

            foreach (var comment in result.Value.message.comments)
            {
                model.Messages.Add(new Message
                {
                    Id = comment.id,
                    Body = comment.text,
                    Title = comment.user.name,
                    Likes = comment.likes_count,
                    Status = comment.created_at.ToRelativeDate(),
                    UserAvatarUrl = comment.user.avatars.square140,
                    Type = MessageType.Comment
                });
            }

            Progress.IsIndeterminate = false;
        }
    }
}