using System.Net;
using System.Threading.Tasks;
using SocialPhone.Models.Socialcast;
using SocialPhone.Rest;

namespace SocialPhone.Services
{
    public class Socialcast
    {
        private static NetworkCredential Nc
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
            return new Rest.Service().GetAsync<SocialCastResult>(string.Format("{0}/api/streams/{1}/messages.json?per_page={2}&page={3}&comments_limit=0", ServiceUrl, streamId, perPage, page), Nc);
        }

        public Task<Result<SocialCastResult>> GetMessageAsync(int messageId)
        {
            return new Rest.Service().GetAsync<SocialCastResult>(string.Format("{0}/api/messages/{1}.json", ServiceUrl, messageId), Nc);
        }

        public Task<Result<SocialCastResult>> GetStreamsAsync()
        {
            return new Rest.Service().GetAsync<SocialCastResult>(string.Format("{0}/api/streams.json", ServiceUrl), Nc, true);
        }

        public Task<Result<SocialCastResult>> GetUserInfo()
        {
            return new Rest.Service().GetAsync<SocialCastResult>(string.Format("{0}/api/userinfo.json", ServiceUrl), Nc);
        }

        public async Task<bool> VerifyCredentialsAsync(string url, string username, string password)
        {
            var tmpNc = new NetworkCredential(username, password);
            var result = await new Rest.Service().GetAsync<SocialCastResult>(string.Format("{0}/api/authentication.json", url), tmpNc);

            return !result.HasError();
        }

        public Task LikeMessageAsync(int messageId)
        {
            return new Rest.Service().PostAsync(string.Format("{0}/api/messages/{1}/likes", ServiceUrl, messageId), Nc);
        }

        public Task UnLikeMessageAsync(int messageId, int likeId)
        {
            return new Rest.Service().PostAsync(string.Format("{0}/api/messages/{1}/likes/{2}", ServiceUrl, messageId, likeId), Nc, null, "DELETE");
        }

        public Task LikeCommentAsync(int commentId)
        {
            return new Rest.Service().PostAsync(string.Format("{0}/api/comments/{1}/likes", ServiceUrl, commentId), Nc);
        }

        public Task UnLikeCommentAsync(int commentId, int likeId)
        {
            return new Rest.Service().PostAsync(string.Format("{0}/api/comments/{1}/likes/{2}", ServiceUrl, commentId, likeId), Nc, null, "DELETE");
        }
    }
}
