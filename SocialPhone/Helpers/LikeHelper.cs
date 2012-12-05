using System.Windows;
using Microsoft.Phone.Controls;
using SocialPhone.ViewModels.Socialcast;

namespace SocialPhone.Helpers
{
    public class LikeHelper
    {
        public static void AttachClickEvent(MenuItem item)
        {
            var message = (Message)item.DataContext;

            if (!message.Likeable)
                item.IsEnabled = false;
            else if (message.Type == MessageType.Message && message.LikedByMe == null)
            {
                item.Click += LikeMessageClick;
                item.Header = "like";
            }
            else if (message.Type == MessageType.Message && message.LikedByMe != null)
            {
                item.Click += UnLikeMessageClick;
                item.Header = "remove like";
            }
            else if (message.Type == MessageType.Comment && message.LikedByMe == null)
            {
                item.Click += LikeCommentClick;
                item.Header = "like";
            }
            else if (message.Type == MessageType.Comment && message.LikedByMe != null)
            {
                item.Click += UnLikeCommentClick;
                item.Header = "remove like";
            }
        }

        private static async void LikeMessageClick(object sender, RoutedEventArgs e)
        {
            var message = (Message)((FrameworkElement)sender).DataContext;
            await Services.Service.Current.Socialcast.LikeMessageAsync(message.Id);
            message.Likes++;
        }

        private static async void UnLikeMessageClick(object sender, RoutedEventArgs e)
        {
            var message = (Message)((FrameworkElement)sender).DataContext;
            await Services.Service.Current.Socialcast.UnLikeMessageAsync(message.Id, message.LikedByMe.id);
            message.Likes--;
        }

        private static async void LikeCommentClick(object sender, RoutedEventArgs e)
        {
            var message = (Message)((FrameworkElement)sender).DataContext;
            await Services.Service.Current.Socialcast.LikeCommentAsync(message.Id);
            message.Likes++;
        }

        private static async void UnLikeCommentClick(object sender, RoutedEventArgs e)
        {
            var message = (Message)((FrameworkElement)sender).DataContext;
            await Services.Service.Current.Socialcast.UnLikeCommentAsync(message.Id, message.LikedByMe.id);
            message.Likes--;
        }
    }
}
