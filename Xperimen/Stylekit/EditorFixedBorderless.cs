using Xamarin.Forms;

namespace Xperimen.Stylekit
{
    public class EditorFixedBorderless : Editor
    {
        public EditorFixedBorderless()
        {
            FontSize = 14;
            TextColor = (Color)Application.Current.Resources["LabelTextColor"];
            BackgroundColor = Color.White;
        }
    }
}
