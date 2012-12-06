using SocialPhone.Models.Socialcast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SocialPhone.ViewModels
{
    public class AugmentedAttachment
    {

        public Attachment Attachment { get; set; }

        public MediaFile MediaFile { get; set; }

        public AugmentedAttachment(Attachment attachment, MediaFile mediaFile)
        {
            Attachment = attachment;
            MediaFile = mediaFile;
        }

        public Visibility ThumbnailVisibility
        {
            get { return (MediaFile != null && MediaFile.thumbnails != null) ? Visibility.Visible : Visibility.Collapsed; }
        }

        public string ThumbnailUrl
        {
            get { return (MediaFile != null && MediaFile.thumbnails != null) ? MediaFile.thumbnails.square45 : ""; }
        }



    }
}
