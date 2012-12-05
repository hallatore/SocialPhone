
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SocialPhone.Services;

namespace SocialPhone.Pages
{
    public class PageBase : PhoneApplicationPage
    {
        protected Service Service { get; set; }
        public ProgressIndicator Progress { get; set; }

        public PageBase()
        {
            var navInTransition = new NavigationInTransition
            {
                Backward = new TurnstileTransition { Mode = TurnstileTransitionMode.BackwardIn },
                Forward = new TurnstileTransition { Mode = TurnstileTransitionMode.ForwardIn }
            };

            var navOutTransition = new NavigationOutTransition
            {
                Backward = new TurnstileTransition { Mode = TurnstileTransitionMode.BackwardOut },
                Forward = new TurnstileTransition { Mode = TurnstileTransitionMode.ForwardOut }
            };

            TransitionService.SetNavigationInTransition(this, navInTransition);
            TransitionService.SetNavigationOutTransition(this, navOutTransition);

            SetValue(TiltEffect.IsTiltEnabledProperty, true);

            Language = System.Windows.Markup.XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);

            Progress = new ProgressIndicator
            {
                IsIndeterminate = false,
                IsVisible = true
            };

            SystemTray.SetProgressIndicator(this, Progress);

            if (!DesignerProperties.IsInDesignTool)
            {
                Service = App.Service;
            }
        }
    }
}
