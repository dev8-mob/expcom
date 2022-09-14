using Xamarin.Forms;

namespace Xperimen.Stylekit
{
    public class EditorFixedBorderless : Editor
    {
        public EditorFixedBorderless()
        {
            FontSize = 14;
            TextColor = Color.Black;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            HeightRequest = 50;
        }
    }
}
