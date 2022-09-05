
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xperimen.Stylekit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomSearchBar : StackLayout
    {
        public CustomSearchBar()
        {
            InitializeComponent();
            entry.Focused += Entry_Focused;
            entry.Unfocused += Entry_Unfocused;
        }

        private void Entry_Focused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused)
            {
                line.BackgroundColor = Color.FromHex(App.Primary);
                frame_bg.BackgroundColor = Color.White;
            }
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            if (!e.IsFocused)
            {
                line.BackgroundColor = Color.DarkGray;
                frame_bg.BackgroundColor = Color.FromHex(App.DimGray2);
            }
        }
    }
}