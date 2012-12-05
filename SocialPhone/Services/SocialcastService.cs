using System.Net;
using System.Threading.Tasks;
using SocialPhone.Models.Socialcast;
using SocialPhone.Rest;

namespace SocialPhone.Services
{
    public class Socialcast
    {
        private static NetworkCredential Credentials
        {
            get
            {
                return new NetworkCredential(Service.Current.Settings.AuthUser.Email, Service.Current.Settings.AuthUser.Password);
            }
        }

        private static string ServiceUrl
        {
            get { return Service.Current.Settings.AuthUser.ServiceUrl; }
        }

        public Task<Result<SocialCastResult>> GetMessagesAsync(int streamId, int perPage = 10, int page = 1)
        {
            return new Rest.Service().GetAsync<SocialCastResult>(string.Format("{0}/api/streams/{1}/messages.json?per_page={2}&page={3}&comments_limit=0", ServiceUrl, streamId, perPage, page), Credentials);
        }

        public Task<Result<SocialCastResult>> GetMessageAsync(int messageId)
        {
            return new Rest.Service().GetAsync<SocialCastResult>(string.Format("{0}/api/messages/{1}.json", ServiceUrl, messageId), Credentials);
        }

        public Task<Result<SocialCastResult>> GetStreamsAsync()
        {
            return new Rest.Service().GetAsync<SocialCastResult>(string.Format("{0}/api/streams.json", ServiceUrl), Credentials, true);
        }

        public Task<Result<SocialCastResult>> GetUserInfo()
        {
            return new Rest.Service().GetAsync<SocialCastResult>(string.Format("{0}/api/userinfo.json", ServiceUrl), Credentials);
        }

        public async Task<bool> VerifyCredentialsAsync(string url, string username, string password)
        {
            var tmpCredentials = new NetworkCredential(username, password);
            var result = await new Rest.Service().GetAsync<SocialCastResult>(string.Format("{0}/api/authentication.json", url), tmpCredentials);

            return !result.HasError();
        }

        public Task<SocialCastResult> LikeMessageAsync(int messageId)
        {
            return new Rest.Service().PostAsync<SocialCastResult>(string.Format("{0}/api/messages/{1}/likes.json", ServiceUrl, messageId), Credentials);
        }

        public Task UnLikeMessageAsync(int messageId, int likeId)
        {
            return new Rest.Service().PostAsync(string.Format("{0}/api/messages/{1}/likes/{2}.json", ServiceUrl, messageId, likeId), Credentials, method: "DELETE");
        }

        public Task<SocialCastResult> LikeCommentAsync(int messageId, int commentId)
        {
            return new Rest.Service().PostAsync<SocialCastResult>(string.Format("{0}/api/messages/{1}/comments/{2}/likes.json", ServiceUrl, messageId, commentId), Credentials);
        }

        public Task UnLikeCommentAsync(int messageId, int commentId, int likeId)
        {
            return new Rest.Service().PostAsync(string.Format("{0}/api/messages/{1}/comments/{2}/likes/{3}.json", ServiceUrl, messageId, commentId, likeId), Credentials, method: "DELETE");
        }
    }
}
