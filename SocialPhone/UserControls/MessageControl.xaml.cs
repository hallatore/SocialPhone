using System;
using System.Windows;

namespace SocialPhone.UserControls
{
    public class TextEventArgs : EventArgs
    {
        public string Text { get; private set; }

        public TextEventArgs(string text)
        {
            Text = text;
        }
    }

    public partial class MessageControl
    {
        public event EventHandler<TextEventArgs> OnButtonClick;

        public MessageControl(string caption = null, string content = null, string buttonCaption = null)
        {
            InitializeComponent();
            Loaded += MessageControl_Loaded;

            HeaderTextBlock.Text = caption ?? "new post";
            MessageTextBox.Text = content ?? string.Empty;
            SaveButton.Content = buttonCaption ?? "send";
        }

        void MessageControl_Loaded(object sender, RoutedEventArgs e)
        {
            MessageTextBox.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OnButtonClick != null)
                OnButtonClick.Invoke(this, new TextEventArgs(MessageTextBox.Text));
        }
    }
}
