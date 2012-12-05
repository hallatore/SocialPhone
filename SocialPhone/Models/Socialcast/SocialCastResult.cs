using System.Collections.Generic;

namespace SocialPhone.Models.Socialcast
{
    public class SocialCastResult
    {
        public Message message { get; set; }
        public User user { get; set; }
        public List<Message> messages { get; set; }
        public List<Stream> streams { get; set; }
    }
}