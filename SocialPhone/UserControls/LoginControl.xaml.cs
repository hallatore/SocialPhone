using System;
using System.Windows;
using System.Windows.Input;

namespace SocialPhone.UserControls
{
    public partial class LoginControl
    {
        private readonly Services.Service service;
        public event EventHandler OnLoginSuccess;

        public LoginControl(Services.Service service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void Login_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, null);
            }
        }

        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var url = ServerTextBox.Text;
            var username = UserNameTextBox.Text;
            var password = PasswordTextBox.Password;

            var success = await service.Socialcast.VerifyCredentialsAsync(url, username, password);

            if (success)
            {
                service.Settings.SocialcastUrl = url;
                service.Settings.Username = username;
                service.Settings.Password = password;

                if (OnLoginSuccess != null)
                    OnLoginSuccess.Invoke(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Authentication failed");
            }
        }
    }
}
