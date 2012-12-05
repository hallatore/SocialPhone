namespace SocialPhone.ViewModels.Socialcast
{
    public enum MessageType
    {
        Message,
        Comment
    }

    public class Message
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Body { get; set; }
        public int Comments { get; set; }
        public int Likes { get; set; }
        public string UserAvatarUrl { get; set; }
        public MessageType Type { get; set; }
    }
}