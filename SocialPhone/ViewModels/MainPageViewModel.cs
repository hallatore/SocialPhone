using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using SocialPhone.Pages;
using SocialPhone.Services;

namespace SocialPhone.ViewModels
{
    public class MainPageViewModel : BindingBase
    {
        public int CurrentPage { get; set; }
        public ObservableCollection<ScMessage> Messages { get; set; }
        public ObservableCollection<SocialPhone.Services.Stream> Streams { get; set; }

        public MainPageViewModel()
        {
            Messages = new ObservableCollection<ScMessage>();
            Streams = new ObservableCollection<SocialPhone.Services.Stream>();
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
