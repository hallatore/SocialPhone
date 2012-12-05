using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SocialPhone
{
    public static class Extensions
    {
        public static Task<Stream> GetStreamAsync(this WebClient client, Uri uri)
        {
            var tcs = new TaskCompletionSource<Stream>();
            client.OpenReadCompleted += (sender, args) =>
            {
                if (args.Error != null)
                    tcs.SetException(args.Error);
                else if (args.Cancelled)
                    tcs.SetCanceled();
                else
                    tcs.SetResult(args.Result);
            };
            client.OpenReadAsync(uri);
            return tcs.Task;
        }

        public static string CutEnd(this string input, int limit)
        {
            if (input.Length > limit)
                input = input.Substring(0, limit) + " ...";

            return input;
        }
    }
}
