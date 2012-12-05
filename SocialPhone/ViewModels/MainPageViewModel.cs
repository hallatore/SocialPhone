using System.Collections.ObjectModel;
using System.Windows;
using SocialPhone.ViewModels.Socialcast;
using Stream = SocialPhone.Models.Socialcast.Stream;

namespace SocialPhone.ViewModels
{
    public class MainPageViewModel : BindingBase
    {
        public int CurrentPage { get; set; }
        public ObservableCollection<Message> Messages { get; set; }
        public ObservableCollection<Stream> Streams { get; set; }

        public MainPageViewModel()
        {
            Messages = new ObservableCollection<Message>();
            Streams = new ObservableCollection<Stream>();
            StreamSelectorVisibility = Visibility.Collapsed;
            CurrentPage = 1;
        }

        private Visibility streamSelectorVisibility;

        public Visibility StreamSelectorVisibility
        {
            get { return streamSelectorVisibility; }
            set
            {
                if (value != streamSelectorVisibility)
                {
                    streamSelectorVisibility = value;
                    NotifyPropertyChanged("StreamSelectorVisibility");
                }
            }
        }

        private string currentStreamName;

        public string CurrentStreamName
        {
            get { return currentStreamName; }
            set
            {
                if (value != currentStreamName)
                {
                    currentStreamName = value;
                    NotifyPropertyChanged("CurrentStreamName");
                }
            }
        }
    }
}
