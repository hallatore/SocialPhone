namespace SocialPhone.Services
{
    public class Settings : BindingBase
    {
        public string SocialcastUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public int CurrentStreamId { get; set; }
        public string CurrentStreamName { get; set; }

    }
}