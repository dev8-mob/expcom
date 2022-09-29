
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xperimen.View.Commitment
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListCell : StackLayout
    {
        public ListCell()
        {
            InitializeComponent();

            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) BackgroundColor = Color.FromHex(App.CharcoalBlack);
                if (theme.Equals("dim")) BackgroundColor = Color.FromHex(App.CharcoalGray);
                if (theme.Equals("light")) BackgroundColor = Color.FromHex(App.DimGray2);
            }
        }
    }
}