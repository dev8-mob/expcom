using Xamarin.Forms;

namespace Xperimen.Stylekit
{
    public class EditorExpandableBorderless : Editor
    {
        public EditorExpandableBorderless()
        {
            TextChanged += (sender, e) => { InvalidateMeasure(); };
            FontSize = 14;
            FontFamily = (string)Application.Current.Resources["NormalFont"];
            TextColor = (Color)Application.Current.Resources["LabelTextColor"];
            BackgroundColor = Color.White;

            Focused += EntryBorderless_Focused;
            Unfocused += EntryBorderless_Unfocused;
        }

        private void EntryBorderless_Focused(object sender, FocusEventArgs e)
        {
            var view = (Editor)sender;
            var parent = (StackLayout)view.Parent;
            var boxview = (BoxView)parent.Children[1];
            boxview.BackgroundColor = (Color)Application.Current.Resources["Primary"];
        }

        private void EntryBorderless_Unfocused(object sender, FocusEventArgs e)
        {
            var view = (Editor)sender;
            var parent = (StackLayout)view.Parent;
            var boxview = (BoxView)parent.Children[1];
            boxview.BackgroundColor = Color.DarkGray;
        }
    }
}
