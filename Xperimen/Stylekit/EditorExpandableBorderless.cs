using Xamarin.Forms;

namespace Xperimen.Stylekit
{
    public class EditorExpandableBorderless : Editor
    {
        public EditorExpandableBorderless()
        {
            TextChanged += (sender, e) => { InvalidateMeasure(); };
            FontSize = 14;
            TextColor = Color.Black;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            HeightRequest = 50;
        }
    }
}
