using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialPhone.Models.Socialcast
{
    public class MediaFile
    {
        public String title { get; set; }
        public Thumbnails thumbnails { get; set; }
        public String page_url { get; set; }
        public String url { get; set; }
        public String content_type { get; set; }
        public String attachment_id { get; set; }

    }
}
