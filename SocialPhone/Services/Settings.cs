namespace SocialPhone.Services
{
    public class Settings : BindingBase
    {
        public AuthUser AuthUser { get; set; }

        public int CurrentStreamId { get; set; }
        public string CurrentStreamName { get; set; }


        public string CurrentTopic { get; set; }
        
        public StreamMode StreamMode { get; set; }

        public Settings()
        {
            StreamMode = StreamMode.Stream;
        }

    }

    public enum StreamMode
    {
        Stream = 0,
        Topic = 1
    }
}