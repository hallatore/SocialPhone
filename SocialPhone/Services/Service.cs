namespace SocialPhone.Services
{
    public class Service
    {
        public Settings Settings { get; set; }
        public Socialcast Socialcast { get; set; }

        public Service()
        {
            Settings = Storage.Get<Settings>("CurrentSettings") ?? new Settings();
            Socialcast = new Socialcast();
        }

        public void SaveSettings()
        {
            Storage.Save("CurrentSettings", Settings);
        }

        private static Service _current;
        public static Service Current
        {
            get { return _current ?? (_current = new Service()); }
        }
    }
}
