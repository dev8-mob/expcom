
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.ViewModel;
using Xperimen.ViewModel.Setting;

namespace Xperimen.Stylekit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomEntry : Frame
    {
        #region bindables
        #region custom properties
        public string BindingText
        {
            get => (string)GetValue(BindingTextProperty);
            set { SetValue(BindingTextProperty, value); }
        }
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set { SetValue(PlaceholderProperty, value); }
        }
        public string ImgLeft
        {
            get => (string)GetValue(ImgLeftProperty);
            set { SetValue(ImgLeftProperty, value); }
        }
        public string ImgLeft_
        {
            get => (string)GetValue(ImgLeft_Property);
            set { SetValue(ImgLeft_Property, value); }
        }
        public string ImgRight
        {
            get => (string)GetValue(ImgRightProperty);
            set { SetValue(ImgRightProperty, value); }
        }
        public string ImgRight_
        {
            get => (string)GetValue(ImgRight_Property);
            set { SetValue(ImgRight_Property, value); }
        }
        public string ImgRight2
        {
            get => (string)GetValue(ImgRight2Property);
            set { SetValue(ImgRight2Property, value); }
        }
        public string ImgRight2_
        {
            get => (string)GetValue(ImgRight2_Property);
            set { SetValue(ImgRight2_Property, value); }
        }
        public bool Ispassword
        {
            get => (bool)GetValue(IsPasswordProperty);
            set { SetValue(IsPasswordProperty, value); }
        }
        public bool Isfocus
        {
            get => (bool)GetValue(IsFocusProperty);
            set { SetValue(IsFocusProperty, value); }
        }
        public Keyboard KeyboardType
        {
            get => (Keyboard)GetValue(KeyboardTypeProperty);
            set { SetValue(KeyboardTypeProperty, value); }
        }
        #endregion
        #region custom bindable properties
        public static BindableProperty BindingTextProperty =
            BindableProperty.Create(nameof(BindingText), typeof(string), typeof(CustomEntry), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateBindingText((string)newVal); });
        public static BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(CustomEntry), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdatePlaceholder((string)newVal); });
        public static BindableProperty ImgLeftProperty =
            BindableProperty.Create(nameof(ImgLeft), typeof(string), typeof(CustomEntry), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateImgLeft((string)newVal); });
        public static BindableProperty ImgLeft_Property =
            BindableProperty.Create(nameof(ImgLeft_), typeof(string), typeof(CustomEntry), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateImgLeft_((string)newVal); });
        public static BindableProperty ImgRightProperty =
            BindableProperty.Create(nameof(ImgRight), typeof(string), typeof(CustomEntry), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateImgRight((string)newVal); });
        public static BindableProperty ImgRight_Property =
            BindableProperty.Create(nameof(ImgRight_), typeof(string), typeof(CustomEntry), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateImgRight_((string)newVal); });
        public static BindableProperty ImgRight2Property =
            BindableProperty.Create(nameof(ImgRight2), typeof(string), typeof(CustomEntry), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateImgRight2((string)newVal); });
        public static BindableProperty ImgRight2_Property =
            BindableProperty.Create(nameof(ImgRight2_), typeof(string), typeof(CustomEntry), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateImgRight2_((string)newVal); });
        public static BindableProperty IsPasswordProperty =
            BindableProperty.Create(nameof(Ispassword), typeof(bool), typeof(CustomEntry), defaultValue: false,
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateIspassword((bool)newVal); });
        public static BindableProperty IsFocusProperty =
            BindableProperty.Create(nameof(Isfocus), typeof(bool), typeof(CustomEntry), defaultValue: false,
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateIsfocus((bool)newVal); });
        public static BindableProperty KeyboardTypeProperty =
            BindableProperty.Create(nameof(KeyboardType), typeof(Keyboard), typeof(CustomEntry), defaultValue: Keyboard.Default,
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateKeyboardType((Keyboard)newVal); });
        #endregion
        #region binding implementation
        public void UpdateBindingText(string data) { entry.SetBinding(Entry.TextProperty, new Binding() { Path = data }); }
        public void UpdatePlaceholder(string data) { entry.Placeholder = data; }
        public void UpdateImgLeft(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                if (Application.Current.Properties.ContainsKey("app_theme"))
                {
                    var theme = Application.Current.Properties["app_theme"] as string;
                    if (theme.Equals("dark")) img_left.Source = ImgLeft_;
                    else img_left.Source = data;
                }
                else img_left.Source = data;

                img_left.IsVisible = true;
                var tap = new TapGestureRecognizer();
                tap.Tapped += ImgTapSetFocus;
                img_left.GestureRecognizers.Add(tap);
            }
            else img_left.IsVisible = false;
        }
        public void UpdateImgLeft_(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                if (Application.Current.Properties.ContainsKey("app_theme"))
                {
                    var theme = Application.Current.Properties["app_theme"] as string;
                    if (theme.Equals("dark")) img_left.Source = data;
                    else img_left.Source = ImgLeft;
                }
                else img_left.Source = ImgLeft;
            }
            else img_left.IsVisible = false;
        }
        public void UpdateImgRight(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                if (Application.Current.Properties.ContainsKey("app_theme"))
                {
                    var theme = Application.Current.Properties["app_theme"] as string;
                    if (theme.Equals("dark")) img_right.Source = ImgRight_;
                    else img_right.Source = data;
                }
                else img_right.Source = data;
                img_right.IsVisible = true;
            }
            else img_right.IsVisible = false;
        }
        public void UpdateImgRight_(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                if (Application.Current.Properties.ContainsKey("app_theme"))
                {
                    var theme = Application.Current.Properties["app_theme"] as string;
                    if (theme.Equals("dark")) img_right.Source = data;
                    else img_right.Source = ImgRight;
                }
                else img_right.Source = ImgRight;
                img_right.IsVisible = true;
            }
            else img_right.IsVisible = false;
        }
        public void UpdateImgRight2(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                var tap = new TapGestureRecognizer();
                tap.Tapped += OnShowHideTap;
                img_right.GestureRecognizers.Add(tap);
            }
            else img_right.GestureRecognizers.Clear();
        }
        public void UpdateImgRight2_(string data) { }
        public void UpdateIspassword(bool data) { entry.IsPassword = data; }
        public void UpdateIsfocus(bool data) { if (data) entry.Focus(); else entry.Unfocus(); }
        public void UpdateKeyboardType(Keyboard data) { entry.Keyboard = data; }
        #endregion
        #endregion

        public CustomEntry()
        {
            InitializeComponent();
            entry.Focused += Entry_Focused;
            entry.Unfocused += Entry_Unfocused;
            entry.IsPassword = Ispassword;
            SetupView();

            #region messaging center
            MessagingCenter.Subscribe<CreateaccViewmodel>(this, "AppThemeUpdated", (sender) =>
            {
                try
                {
                    UpdateImgLeft(ImgLeft);
                    UpdateImgLeft_(ImgLeft_);
                    UpdateImgRight(ImgRight);
                    UpdateImgRight_(ImgRight_);
                    SetupView();
                }
                catch (Exception ex) { var error = ex.Message; }
            });
            MessagingCenter.Subscribe<LoginViewmodel>(this, "AppThemeUpdated", (sender) =>
            {
                try
                {
                    UpdateImgLeft(ImgLeft);
                    UpdateImgLeft_(ImgLeft_);
                    UpdateImgRight(ImgRight);
                    UpdateImgRight_(ImgRight_);
                    SetupView();
                }
                catch (Exception ex) { var error = ex.Message; }
            });
            MessagingCenter.Subscribe<SettingViewmodel>(this, "AppThemeUpdated", (sender) =>
            {
                try
                {
                    UpdateImgLeft(ImgLeft);
                    UpdateImgLeft_(ImgLeft_);
                    UpdateImgRight(ImgRight);
                    UpdateImgRight_(ImgRight_);
                    SetupView();
                }
                catch (Exception ex) { var error = ex.Message; }
            });
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
                    BackgroundColor = Color.FromHex(App.CharcoalBlack);
                    entry.PlaceholderColor = Color.White;
                }
                if (theme.Equals("dim"))
                {
                    entry.TextColor = Color.Default;
                    BackgroundColor = Color.FromHex(App.CharcoalGray);
                    entry.PlaceholderColor = Color.Default;
                }
                if (theme.Equals("light"))
                {
                    entry.TextColor = Color.Default;
                    BackgroundColor = Color.FromHex(App.DimGray2);
                    entry.PlaceholderColor = Color.Default;
                }
            }
            else
            {
                entry.TextColor = Color.Default;
                BackgroundColor = Color.FromHex(App.DimGray2);
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
                    if (theme.Equals("dark")) BackgroundColor = Color.FromHex(App.SlateGray);
                    else if (theme.Equals("dim")) BackgroundColor = Color.FromHex(App.DimGray2);
                    else if (theme.Equals("light")) BackgroundColor = Color.White;
                }
                else BackgroundColor = Color.White;

                if (!string.IsNullOrEmpty(entry.Text))
                    if (entry.Text.Equals("0")) entry.Text = "";
            }
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            var view = (Entry)sender;
            if (!e.IsFocused)
            {
                if (string.IsNullOrEmpty(view.Text))
                {
                    if (view.Keyboard == Keyboard.Numeric) view.Text = "0";
                    else view.Text = string.Empty;
                }

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

        private async void ImgTapSetFocus(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            entry.Focus();
            view.IsEnabled = true;
        }

        private async void OnShowHideTap(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            var src = view.Source.ToString();

            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark"))
                {
                    if (src.Equals("File: " + ImgRight_))
                    {
                        img_right.Source = ImgRight2_;
                        entry.IsPassword = false;
                    }
                    else
                    {
                        img_right.Source = ImgRight_;
                        entry.IsPassword = true;
                    }
                }
                else
                {
                    if (src.Equals("File: " + ImgRight))
                    {
                        img_right.Source = ImgRight2;
                        entry.IsPassword = false;
                    }
                    else
                    {
                        img_right.Source = ImgRight;
                        entry.IsPassword = true;
                    }
                }
            }
            else
            {
                if (src.Equals("File: " + ImgRight))
                {
                    img_right.Source = ImgRight2;
                    entry.IsPassword = false;
                }
                else
                {
                    img_right.Source = ImgRight;
                    entry.IsPassword = true;
                }
            }
        }
    }
}