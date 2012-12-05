using System.Collections.ObjectModel;
using SocialPhone.ViewModels.Socialcast;

namespace SocialPhone.ViewModels
{
    public class MessagePageViewModel : BindingBase
    {
        public int MessageId { get; set; }
        public ObservableCollection<Message> Messages { get; set; }

        public MessagePageViewModel()
        {
            Messages = new ObservableCollection<Message>();
        }
    }
}
