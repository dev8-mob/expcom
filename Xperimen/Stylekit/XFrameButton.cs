using System;
using Xamarin.Forms;

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
            FrameId = Guid.NewGuid().ToString();

            MessagingCenter.Subscribe<XFrameButton, string>(this, "FrameSelected", (sender, arg) =>
            {
                BorderColor = Color.DarkGray;
                BackgroundColor = Color.Transparent;
                if (arg.Equals(FrameId))
                {
                    BorderColor = Color.FromHex(App.Primary);
                    if (Application.Current.Properties.ContainsKey("app_theme"))
                    {
                        var theme = Application.Current.Properties["app_theme"] as string;
                        if (theme.Equals("dark"))
                        { BackgroundColor = Color.FromHex(App.CharcoalBlack); }
                        if (theme.Equals("dim"))
                        { BackgroundColor = Color.FromHex(App.CharcoalGray); }
                        if (theme.Equals("light"))
                        { BackgroundColor = Color.FromHex(App.DimGray2); }
                    }
                    else BackgroundColor = Color.FromHex(App.DimGray2);
                }
            });
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
