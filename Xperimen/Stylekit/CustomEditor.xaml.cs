
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.ViewModel;
using Xperimen.ViewModel.Setting;

namespace Xperimen.Stylekit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomEditor : Frame
    {
        #region bindables
        #region custom properties
        public string BindingText
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }
        public bool Isfocus
        {
            get { return (bool)GetValue(IsFocusProperty); }
            set { SetValue(IsFocusProperty, value); }
        }
        #endregion
        #region custom properties binding
        public static BindableProperty TextProperty =
            BindableProperty.Create(nameof(BindingText), typeof(string), typeof(CustomEditor), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEditor)bindable).UpdateText((string)newVal); });
        public static BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(CustomEditor), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEditor)bindable).UpdatePlaceholder((string)newVal); });
        public static BindableProperty IsFocusProperty =
            BindableProperty.Create(nameof(Isfocus), typeof(bool), typeof(CustomEditor), defaultValue: false,
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEditor)bindable).UpdateIsfocus((bool)newVal); });
        #endregion
        #region binding implementation
        public void UpdateText(string data) { editor.SetBinding(Editor.TextProperty, new Binding() { Path = data }); }
        public void UpdatePlaceholder(string data) { editor.Placeholder = data; }
        public void UpdateIsfocus(bool data) { if (data) editor.Focus(); else editor.Unfocus(); }
        #endregion
        #endregion

        public CustomEditor()
        {
            InitializeComponent();
            editor.Focused += Editor_Focused;
            editor.Unfocused += Editor_Unfocused;
            SetupView();

            MessagingCenter.Subscribe<CreateaccViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
            MessagingCenter.Subscribe<LoginViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
            MessagingCenter.Subscribe<SettingViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
        }

        public void SetupView()
        {
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark"))
                {
                    editor.TextColor = Color.White;
                    BackgroundColor = Color.FromHex(App.CharcoalBlack);
                    editor.PlaceholderColor = Color.White;
                }
                if (theme.Equals("dim"))
                {
                    editor.TextColor = Color.Default;
                    BackgroundColor = Color.FromHex(App.CharcoalGray);
                    editor.PlaceholderColor = Color.Default;
                }
                if (theme.Equals("light"))
                {
                    editor.TextColor = Color.Default;
                    BackgroundColor = Color.FromHex(App.DimGray2);
                    editor.PlaceholderColor = Color.Default;
                }
            }
            else
            {
                editor.TextColor = Color.Default;
                BackgroundColor = Color.FromHex(App.DimGray2);
                editor.PlaceholderColor = Color.Default;
            }
        }

        private void Editor_Focused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused)
            {
                line.BackgroundColor = Color.FromHex(App.Primary);
                if (Application.Current.Properties.ContainsKey("app_theme"))
                {
                    var theme = Application.Current.Properties["app_theme"] as string;
                    if (theme.Equals("dark")) BackgroundColor = Color.FromHex(App.SlateGray);
                    else if (theme.Equals("dim")) BackgroundColor = Color.FromHex(App.DimGray2);
                    else if (theme.Equals("white")) BackgroundColor = Color.White;
                }
                else BackgroundColor = Color.White;
            }
        }

        private void Editor_Unfocused(object sender, FocusEventArgs e)
        {
            if (!e.IsFocused)
            {
                line.BackgroundColor = Color.DarkGray;
                if (Application.Current.Properties.ContainsKey("app_theme"))
                {
                    var theme = Application.Current.Properties["app_theme"] as string;
                    if (theme.Equals("dark")) BackgroundColor = Color.FromHex(App.CharcoalBlack);
                    if (theme.Equals("dim")) BackgroundColor = Color.FromHex(App.CharcoalGray);
                    if (theme.Equals("light")) BackgroundColor = Color.FromHex(App.DimGray2);
                }
                else BackgroundColor = Color.FromHex(App.DimGray2);
            }
        }
    }
}