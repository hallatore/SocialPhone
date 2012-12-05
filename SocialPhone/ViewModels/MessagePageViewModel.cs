using System.Collections.ObjectModel;
using SocialPhone.Pages;

namespace SocialPhone.ViewModels
{
    public class MessagePageViewModel : BindingBase
    {
        public int MessageId { get; set; }
        public ObservableCollection<ScMessage> Messages { get; set; }

        public MessagePageViewModel()
        {
            Messages = new ObservableCollection<ScMessage>();
        }
    }
}
