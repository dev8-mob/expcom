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
        }
    }
}
