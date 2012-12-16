using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SocialPhone.ViewModels;
using Microsoft.Phone.Tasks;
using SocialPhone.ViewModels.Socialcast;

namespace SocialPhone.UserControls
{
    public partial class AttachmentsList : UserControl
    {
        public AttachmentsList()
        {
            InitializeComponent();
        }


        public void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AttachmentsListBox.SelectedIndex == -1) return;
            var url = (AttachmentsListBox.SelectedItems[0] as AugmentedAttachment).Attachment.public_filename;
            Dispatcher.BeginInvoke(() =>
            {
                var webbrowser = new WebBrowserTask();
                webbrowser.Uri = new Uri(url);
                webbrowser.Show();
            });
            AttachmentsListBox.SelectedIndex = -1;
        }
    }
}
