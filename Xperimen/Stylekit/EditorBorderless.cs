using Xamarin.Forms;

namespace Xperimen.Stylekit
{
    public class EditorBorderless : Editor
    {
        public EditorBorderless()
        {
            FontSize = 14;
            TextColor = Color.Black;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            HeightRequest = 85;
            //TextChanged += (sender, e) => { InvalidateMeasure(); };
        }
    }
}
