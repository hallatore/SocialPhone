using System.IO.IsolatedStorage;

namespace SocialPhone.Services
{
    public class Storage
    {
        private static readonly IsolatedStorageSettings IsolatedStorage = IsolatedStorageSettings.ApplicationSettings;

        public static void Save(string key, object value)
        {
            IsolatedStorage.Remove(key);
            IsolatedStorage.Add(key, value);
            IsolatedStorage.Save();
        }

        public static T Get<T>(string key)
        {
            if (IsolatedStorage.Contains(key))
            {
                return (T)IsolatedStorage[key];
            }

            return default(T);
        }

        public static void Remove(string key)
        {
            IsolatedStorage.Remove(key);
        }
    }
}