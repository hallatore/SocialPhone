using System.Runtime.Serialization;

namespace SocialPhone.Models.Socialcast
{
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
}