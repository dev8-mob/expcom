using Xamarin.Forms;

namespace Xperimen.Stylekit
{
    public class EntryBorderless : Entry
    {
        public EntryBorderless()
        {
            FontSize = 14;
            TextColor = Color.Black;
            HeightRequest = 40;
            VerticalTextAlignment = TextAlignment.Center;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            if (Device.RuntimePlatform == Device.Android) FontFamily = "Ubuntu-Regular.ttf#Ubuntu Regular";
            else if (Device.RuntimePlatform == Device.iOS) FontFamily = "Ubuntu-Regular.ttf";
        }
    }
}
