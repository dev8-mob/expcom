using Xamarin.Forms;

namespace Xperimen.Stylekit
{
    public class PickerBorderless : Picker
    {
        public PickerBorderless()
        {
            FontSize = 14;
            TextColor = Color.Black;
            HeightRequest = 40;
            HorizontalOptions = LayoutOptions.FillAndExpand;

            Focused += PickerBorderless_Focused;
            Unfocused += PickerBorderless_Unfocused;
        }

        private void PickerBorderless_Focused(object sender, FocusEventArgs e)
        {
            var view = (Picker)sender;
            var parent = (StackLayout)view.Parent.Parent;
            var boxview = (BoxView)parent.Children[1];
            boxview.BackgroundColor = (Color)Application.Current.Resources["Primary"];
        }

        private void PickerBorderless_Unfocused(object sender, FocusEventArgs e)
        {
            var view = (Picker)sender;
            var parent = (StackLayout)view.Parent.Parent;
            var boxview = (BoxView)parent.Children[1];
            boxview.BackgroundColor = Color.DarkGray;
        }
    }
}
