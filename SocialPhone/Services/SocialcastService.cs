using System.Net;
using System.Threading.Tasks;
using SocialPhone.Models.Socialcast;
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
}
