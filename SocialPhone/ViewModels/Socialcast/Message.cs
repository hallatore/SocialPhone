using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using SocialPhone.Models.Socialcast;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialPhone.ViewModels.Socialcast
{
    public enum MessageType
    {
        Message,
        Comment
    }

    public class Message : BindingBase
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }
        public int Comments { get; set; }
        public string UserAvatarUrl { get; set; }
        public MessageType Type { get; set; }
        public string ExternalUrl { get; set; }
        public List<Attachment> Attachments { get; set; }
        public MediaFile[] MediaFiles { get; set; }

        public bool Likeable { get; set; }
        public Like LikedByMe { get; set; }

        public int ParentId { get; set; }

        private int likes;

        public int Likes
        {
            get { return likes; }
            set
            {
                if (value != likes)
                {
                    likes = value;
                    NotifyPropertyChanged("Likes");
                }
            }
        }
        public IEnumerable<string> Urls { get { return Body.ExtractUris(ExternalUrl); } }

        public IEnumerable<MenuItem> MenuItems
        {
            get
            {
                var likeItem = new MenuItem();
                likeItem.IsEnabled = Likeable;
                likeItem.FontSize = 20;
                likeItem.Header = "Like";
                return new List<MenuItem>(new[] { likeItem }).Union(BuildUrlItems(Urls, Attachments));
            }
        }

        public string AttachmentList
        {
            get
            {
                return Attachments.Any() ?
                    "\n" + Attachments
                           .Select(a => "[Attachment: " + a.filename + "]").Aggregate((s1, s2) => s1 + "\n" + s2)
                    : "";
            }
        }

        public IEnumerable<AugmentedAttachment> AugmentedAttachments
        {
            get
            {
                
                return Attachments.Select(a => new AugmentedAttachment(a, MediaFiles.FirstOrDefault(m => m.attachment_id == a.id)));
            }
        }

        public static IEnumerable<MenuItem> BuildUrlItems(IEnumerable<string> urls, IEnumerable<Attachment> attachments)
        {
            var urlItems = urls.Select(u => WebTask(u, u));
            var attachmenItems = attachments.Select(a => WebTask(a.public_filename, "Attachment: " + a.filename));
            return urlItems.Union(attachmenItems);
        }
        private static MenuItem WebTask(String uri, String header)
        {
            var item = new MenuItem();
            item.FontSize = 20;
            item.Click += (s, re) =>
            {
                var wbTask = new WebBrowserTask { Uri = new Uri(uri, UriKind.Absolute) };
                wbTask.Show();
            };
            item.Header = header;
            return item;
        }

    }
}