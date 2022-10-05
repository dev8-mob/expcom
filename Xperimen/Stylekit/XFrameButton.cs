using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xperimen.ViewModel;
using Xperimen.ViewModel.Setting;

namespace Xperimen.Stylekit
{
    public class XFrameButton : Frame
    {
        public string FrameId { get; set; }

        public XFrameButton()
        {
            HasShadow = false;
            CornerRadius = 8;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.Fill;
            BorderColor = Color.DarkGray;
            BackgroundColor = Color.Transparent;
            Padding = 0;
            IsClippedToBounds = true;
            FrameId = Guid.NewGuid().ToString();

            var tap = new TapGestureRecognizer();
            tap.Tapped += FrameTapped;
            GestureRecognizers.Add(tap);

            #region messaging center
            MessagingCenter.Subscribe<XFrameButton, string>(this, "FrameSelected", async (sender, arg) =>
            {
                BorderColor = Color.DarkGray;
                BackgroundColor = Color.Transparent;
                if (arg.Equals(FrameId))
                {
                    BorderColor = Color.FromHex(App.Primary);
                    await Task.Delay(500);
                    if (Application.Current.Properties.ContainsKey("app_theme"))
                    {
                        var theme = Application.Current.Properties["app_theme"] as string;
                        if (theme.Equals("dark")) BackgroundColor = Color.FromHex(App.CharcoalBlack);
                        if (theme.Equals("dim")) BackgroundColor = Color.FromHex(App.CharcoalGray);
                        if (theme.Equals("light")) BackgroundColor = Color.FromHex(App.DimGray2);
                    }
                    else BackgroundColor = Color.FromHex(App.DimGray2);
                }
            });
            MessagingCenter.Subscribe<CreateaccViewmodel>(this, "AppThemeUpdated", (sender) =>
            {
                BorderColor = Color.DarkGray;
                BackgroundColor = Color.Transparent;
            });
            MessagingCenter.Subscribe<LoginViewmodel>(this, "AppThemeUpdated", (sender) =>
            {
                BorderColor = Color.DarkGray;
                BackgroundColor = Color.Transparent;
            });
            MessagingCenter.Subscribe<SettingViewmodel>(this, "AppThemeUpdated", (sender) =>
            {
                BorderColor = Color.DarkGray;
                BackgroundColor = Color.Transparent;
            });
            #endregion
        }

        public async void FrameTapped(object sender, EventArgs arg)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            MessagingCenter.Send(this, "FrameSelected", FrameId);
            view.IsEnabled = true;
        }
    }
}
