using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace SocialPhone.Rest
{
    public class Service
    {
        public async Task<Result<T>> GetAsync<T>(string url, NetworkCredential credentials = null, bool cacheResult = false)
        {
            try
            {
                var client = new WebClient();
                client.Credentials = credentials;
                client.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";

                var query = cacheResult ? url : AppendAntiCacheToken(url);
                var result = await client.GetStreamAsync(new Uri(query));

                if (client.ResponseHeaders[HttpRequestHeader.ContentEncoding] == "gzip")
                {
                    var gzipStream = new ICSharpCode.SharpZipLib.GZip.GZipInputStream(result);
                    return new Result<T>(ReadObject<T>(gzipStream));
                }

                return new Result<T>(ReadObject<T>(result));
            }
            catch (Exception ex)
            {
                if (ex is WebException && ((WebException)ex).Response != null && ((HttpWebResponse)((WebException)ex).Response).StatusCode == HttpStatusCode.Unauthorized)
                    return new Result<T>(new UnauthorizedAccessException());

                return new Result<T>(ex);
            }
        }

        private static string AppendAntiCacheToken(string url)
        {
            if (url.Contains("?"))
                return url + "&_=" + DateTime.Now.ToString("ddMMyyyymm") + DateTime.Now.Second / 30;
            
            return url + "?_=" + DateTime.Now.ToString("ddMMyyyymm") + DateTime.Now.Second / 30;
        }

        public virtual T ReadObject<T>(Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            return (T)serializer.ReadObject(stream);
        }
    }
}
