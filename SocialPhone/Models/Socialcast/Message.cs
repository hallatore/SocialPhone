using System;
using System.Collections.Generic;

namespace SocialPhone.Models.Socialcast
{
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
        public List<Like> likes { get; set; }
        public User user { get; set; }
        public DateTime created_at { get; set; }
        public bool editable { get; set; }
        public bool deletable { get; set; }
        public bool likable { get; set; }
        public string external_url { get; set; }
        public List<Attachment> attachments { get; set; }
        public MediaFile[] media_files { get; set; }
    }
}