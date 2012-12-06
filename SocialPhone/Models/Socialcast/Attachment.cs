using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialPhone.Models.Socialcast
{
    public class Attachment
    {
        public string id { get; set; }
        public string filename { get; set; }
        public string url { get; set; }
        public string public_filename { get; set; }
        public string content_type { get; set; }
        public string file_extension { get; set; }
    }
}
