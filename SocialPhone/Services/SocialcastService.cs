using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using SocialPhone.Rest;

namespace SocialPhone.Services
{
    public class Socialcast
    {
        public Task<Result<SocialCastResult>> GetMessagesAsync(int streamId, int perPage = 10, int page = 1)
        {
            var settings = Service.Current.Settings;
            var nc = new NetworkCredential(settings.Username, settings.Password);
            return new Rest.Service().GetAsync<SocialCastResult>(string.Format("{0}/api/streams/{1}/messages.json?per_page={2}&page={3}&comments_limit=0", settings.SocialcastUrl, streamId, perPage, page), nc);
        }

        public Task<Result<SocialCastResult>> GetMessageAsync(int messageId)
        {
            var settings = Service.Current.Settings;
            var nc = new NetworkCredential(settings.Username, settings.Password);
            return new Rest.Service().GetAsync<SocialCastResult>(string.Format("{0}/api/messages/{1}.json", settings.SocialcastUrl, messageId), nc);
        }

        public Task<Result<SocialCastResult>> GetStreamsAsync()
        {
            var settings = Service.Current.Settings;
            var nc = new NetworkCredential(settings.Username, settings.Password);
            return new Rest.Service().GetAsync<SocialCastResult>(string.Format("{0}/api/streams.json", settings.SocialcastUrl), nc, true);
        }

        public async Task<bool> VerifyCredentialsAsync(string url, string username, string password)
        {
            var nc = new NetworkCredential(username, password);
            var result = await new Rest.Service().GetAsync<SocialCastResult>(string.Format("{0}/api/authentication.json", url), nc, true);

            return !result.HasError();
        }
    }

    public class SocialCastResult
    {
        public Message message { get; set; }
        public List<Message> messages { get; set; }
        public List<Stream> streams { get; set; }
    }

    [DataContract]
    public class Stream
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "default")]
        public bool Default { get; set; }
    }

    public class Message
    {
        public int id { get; set; }
        public int likes_count { get; set; }
        public int comments_count { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string text { get; set; }
        public List<User> groups { get; set; }
        public List<User> recipients { get; set; }
        public List<Message> comments { get; set; }
        public User user { get; set; }

        public DateTime created_at { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public Avatars avatars { get; set; }
    }

    public class Avatars
    {
        public string square70 { get; set; }
        public string square45 { get; set; }
        public string square30 { get; set; }
        public string square140 { get; set; }
    }
}
