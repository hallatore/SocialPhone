using SocialPhone.Models.Socialcast;

namespace SocialPhone.ViewModels.Socialcast
{
    public enum MessageType
    {
        Message,
        Comment
    }

    public class Message : BindingBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Body { get; set; }
        public int Comments { get; set; }
        public string UserAvatarUrl { get; set; }
        public MessageType Type { get; set; }

        public bool Likeable { get; set; }
        public Like LikedByMe { get; set; }

        public int ParentId { get; set; }

        private int likes;

        public int Likes
        {
            get { return likes; }
            set
            {
                if (value != likes)
                {
                    likes = value;
                    NotifyPropertyChanged("Likes");
                }
            }
        }
    }
}