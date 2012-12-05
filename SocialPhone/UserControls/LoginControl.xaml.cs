using System;
using System.Windows;
using System.Windows.Input;
using SocialPhone.Services;

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
                service.Settings.AuthUser = new AuthUser
                {
                    ServiceUrl = url,
                    Email = username,
                    Password = password
                };

                var userInfo = await service.Socialcast.GetUserInfo();

                if (!userInfo.HasError())
                {
                    service.Settings.AuthUser.Id = userInfo.Value.user.id;

                    if (OnLoginSuccess != null)
                        OnLoginSuccess.Invoke(this, new EventArgs());

                    return;
                }
            }

            service.Settings.AuthUser = null;
            MessageBox.Show("Authentication failed");
        }
    }
}
