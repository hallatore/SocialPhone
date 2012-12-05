using System;

namespace SocialPhone.Models.Socialcast
{
    public class Like
    {
        public int id { get; set; }
        public User user { get; set; }
        public DateTime created_at { get; set; }
        public bool unlikeable { get; set; }
    }
}