using System.Net;
using System.Windows;
using Microsoft.Phone.Controls;
using SocialPhone.Models.Socialcast;
using SocialPhone.ViewModels.Socialcast;
using Message = SocialPhone.ViewModels.Socialcast.Message;

namespace SocialPhone.Helpers
{
    public class LikeHelper
    {
        public static void AttachClickEvent(MenuItem item)
        {
            var message = (Message)item.DataContext;

            if (!message.Likeable)
                item.IsEnabled = false;
            else if (message.LikedByMe == null)
            {
                item.Click += LikeClick;
                item.Header = "like";
            }
            else if (message.LikedByMe != null)
            {
                item.Click += UnlikeClick;
                item.Header = "unlike";
            }
        }

        private static async void LikeClick(object sender, RoutedEventArgs e)
        {
            var currentPage = (Pages.PageBase)((App)Application.Current).RootFrame.Content;
            currentPage.Progress.IsIndeterminate = true;
            var item = (MenuItem)sender;
            var message = (Message)item.DataContext;

            try
            {
                SocialCastResult result;

                if (message.Type == MessageType.Message)
                    result = await Services.Service.Current.Socialcast.LikeMessageAsync(message.Id);
                else
                    result = await Services.Service.Current.Socialcast.LikeCommentAsync(message.ParentId, message.Id);

                message.LikedByMe = result.like;
                message.Likes++;
            }
            catch(WebException ex) {}

            item.Click -= LikeClick;
            currentPage.Progress.IsIndeterminate = false;
        }

        private static async void UnlikeClick(object sender, RoutedEventArgs e)
        {
            var currentPage = (Pages.PageBase)((App)Application.Current).RootFrame.Content;
            currentPage.Progress.IsIndeterminate = true;
            var item = (MenuItem)sender;
            var message = (Message)item.DataContext;
            
            try
            {
                if (message.Type == MessageType.Message)
                    await Services.Service.Current.Socialcast.UnLikeMessageAsync(message.Id, message.LikedByMe.id);
                else
                    await Services.Service.Current.Socialcast.UnLikeCommentAsync(message.ParentId, message.Id, message.LikedByMe.id);

                message.LikedByMe = null;
                message.Likes--;
            }
            catch (WebException ex) { }

            item.Click -= UnlikeClick;
            currentPage.Progress.IsIndeterminate = false;
        }
    }
}
