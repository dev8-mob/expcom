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
            if (Device.RuntimePlatform == Device.Android) FontFamily = "Ubuntu-Regular.ttf#Ubuntu Regular";
            else if (Device.RuntimePlatform == Device.iOS) FontFamily = "Ubuntu-Regular";
            //TextChanged += (sender, e) => { InvalidateMeasure(); };
        }
    }
}
