
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.ViewModel;
using Xperimen.ViewModel.Setting;

namespace Xperimen.Stylekit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomCheckbox : StackLayout
    {
        #region bindables
        #region custom properties
        public string CheckOn1
        {
            get { return (string)GetValue(CheckOn1Property); }
            set { SetValue(CheckOn1Property, value); }
        }
        public string CheckOff1
        {
            get { return (string)GetValue(CheckOff1Property); }
            set { SetValue(CheckOff1Property, value); }
        }
        public string CheckOn2
        {
            get { return (string)GetValue(CheckOn2Property); }
            set { SetValue(CheckOn2Property, value); }
        }
        public string CheckOff2
        {
            get { return (string)GetValue(CheckOff2Property); }
            set { SetValue(CheckOff2Property, value); }
        }
        public string BindingCheck
        {
            get { return (string)GetValue(BindingCheckProperty); }
            set { SetValue(BindingCheckProperty, value); }
        }
        #endregion
        #region custom properties binding
        public static BindableProperty CheckOn1Property =
            BindableProperty.Create(nameof(CheckOn1), typeof(string), typeof(CustomCheckbox), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomCheckbox)bindable).UpdateCheckOn1(); });
        public static BindableProperty CheckOff1Property =
            BindableProperty.Create(nameof(CheckOff1), typeof(string), typeof(CustomCheckbox), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomCheckbox)bindable).UpdateCheckOff1(); });
        public static BindableProperty CheckOn2Property =
            BindableProperty.Create(nameof(CheckOn2), typeof(string), typeof(CustomCheckbox), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomCheckbox)bindable).UpdateCheckOn2(); });
        public static BindableProperty CheckOff2Property =
            BindableProperty.Create(nameof(CheckOff2), typeof(string), typeof(CustomCheckbox), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomCheckbox)bindable).UpdateCheckOff2(); });
        public static BindableProperty BindingCheckProperty =
            BindableProperty.Create(nameof(BindingCheck), typeof(string), typeof(CustomCheckbox), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomCheckbox)bindable).UpdateBindingCheck((string)newVal); });
        #endregion
        #region binding implementation
        public void UpdateCheckOn1() { SetupView(); }
        public void UpdateCheckOff1() { SetupView(); }
        public void UpdateCheckOn2() { SetupView(); }
        public void UpdateCheckOff2() { SetupView(); }
        public void UpdateBindingCheck(string data)
        { checkbox.SetBinding(CheckBox.IsCheckedProperty, new Binding() { Path = data }); }
        #endregion
        #endregion

        public CustomCheckbox()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<CreateaccViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
            MessagingCenter.Subscribe<LoginViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
            MessagingCenter.Subscribe<SettingViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
        }

        private async void CheckboxTapped(object sender, System.EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.8, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            var file = view.Source.ToString();

            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark"))
                {
                    if (file.Equals("File: " + CheckOff2))
                    { img_check.Source = CheckOn2; checkbox.IsChecked = true; }
                    else if (file.Equals("File: " + CheckOn2))
                    { img_check.Source = CheckOff2; checkbox.IsChecked = false; }
                }
                else
                {
                    if (file.Equals("File: " + CheckOff1))
                    { img_check.Source = CheckOn1; checkbox.IsChecked = true; }
                    else if (file.Equals("File: " + CheckOn1))
                    { img_check.Source = CheckOff1; checkbox.IsChecked = false; }
                }
            }
            else
            {
                if (file.Equals("File: " + CheckOff1))
                { img_check.Source = CheckOn1; checkbox.IsChecked = true; }
                else if (file.Equals("File: " + CheckOn1))
                { img_check.Source = CheckOff1; checkbox.IsChecked = false; }
            }
            view.IsEnabled = true;
        }

        public void SetupView()
        {
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) img_check.Source = CheckOff2;
                else img_check.Source = CheckOff1;
            }
            else img_check.Source = CheckOff1;
        }

        public void InitCheckbox(bool ischecked)
        {
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark"))
                {
                    if (ischecked) img_check.Source = CheckOn2;
                    else img_check.Source = CheckOff2;
                }
                else
                {
                    if (ischecked) img_check.Source = CheckOn1;
                    else img_check.Source = CheckOff1;
                }
            }
            else
            {
                if (ischecked) img_check.Source = CheckOn1;
                else img_check.Source = CheckOff1;
            }
        }
    }
}