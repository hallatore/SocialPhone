namespace SocialPhone.Services
{
    public class Settings : BindingBase
    {
        public AuthUser AuthUser { get; set; }

        public int CurrentStreamId { get; set; }
        public string CurrentStreamName { get; set; }

    }
}