
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.ViewModel;
using Xperimen.ViewModel.Setting;

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
            SetupView();

            #region messaging center
            MessagingCenter.Subscribe<CreateaccViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
            MessagingCenter.Subscribe<LoginViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
            MessagingCenter.Subscribe<SettingViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
            #endregion
        }

        public void SetupView()
        {
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark"))
                {
                    entry.TextColor = Color.White;
                    stack_entry.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                    entry.PlaceholderColor = Color.White;
                }
                if (theme.Equals("dim"))
                {
                    entry.TextColor = Color.Default;
                    stack_entry.BackgroundColor = Color.FromHex(App.CharcoalGray);
                    entry.PlaceholderColor = Color.Default;
                }
                if (theme.Equals("light"))
                {
                    entry.TextColor = Color.Default;
                    stack_entry.BackgroundColor = Color.FromHex(App.DimGray2);
                    entry.PlaceholderColor = Color.Default;
                }
            }
            else
            {
                entry.TextColor = Color.Default;
                stack_entry.BackgroundColor = Color.FromHex(App.DimGray2);
                entry.PlaceholderColor = Color.Default;
            }
        }

        private void Entry_Focused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused)
            {
                line.BackgroundColor = Color.FromHex(App.Primary);
                if (Application.Current.Properties.ContainsKey("app_theme"))
                {
                    var theme = Application.Current.Properties["app_theme"] as string;
                    if (theme.Equals("dark")) stack_entry.BackgroundColor = Color.FromHex(App.SlateGray);
                    else stack_entry.BackgroundColor = Color.White;
                }
                else stack_entry.BackgroundColor = Color.White;
            }
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            if (!e.IsFocused)
            {
                line.BackgroundColor = Color.DarkGray;
                if (Application.Current.Properties.ContainsKey("app_theme"))
                {
                    var theme = Application.Current.Properties["app_theme"] as string;
                    if (theme.Equals("dark")) stack_entry.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                    if (theme.Equals("dim")) stack_entry.BackgroundColor = Color.FromHex(App.CharcoalGray);
                    if (theme.Equals("light")) stack_entry.BackgroundColor = Color.FromHex(App.DimGray2);
                }
                else stack_entry.BackgroundColor = Color.FromHex(App.DimGray2);
            }
        }
    }
}