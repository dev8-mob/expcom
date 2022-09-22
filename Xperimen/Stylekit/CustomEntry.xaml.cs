
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.View;

namespace Xperimen.Stylekit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomEntry : Frame
    {
        #region bindables
        #region custom properties
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }
        public string ImgLeft
        {
            get { return (string)GetValue(ImgLeftProperty); }
            set { SetValue(ImgLeftProperty, value); }
        }
        public string ImgLeft_
        {
            get { return (string)GetValue(ImgLeft_Property); }
            set { SetValue(ImgLeft_Property, value); }
        }
        public string ImgRight
        {
            get { return (string)GetValue(ImgRightProperty); }
            set { SetValue(ImgRightProperty, value); }
        }
        public string ImgRight_
        {
            get { return (string)GetValue(ImgRight_Property); }
            set { SetValue(ImgRight_Property, value); }
        }
        public string ImgRight2
        {
            get { return (string)GetValue(ImgRight2Property); }
            set { SetValue(ImgRight2Property, value); }
        }
        public string ImgRight2_
        {
            get { return (string)GetValue(ImgRight2_Property); }
            set { SetValue(ImgRight2_Property, value); }
        }
        public bool Ispassword
        {
            get { return (bool)GetValue(IsPasswordProperty); }
            set { SetValue(IsPasswordProperty, value); }
        }
        public bool Isfocus
        {
            get { return (bool)GetValue(IsFocusProperty); }
            set { SetValue(IsFocusProperty, value); }
        }
        #endregion
        #region custom bindable properties
        public static BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomEntry), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateText((string)newVal); });
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
        #endregion
        #region binding implementation
        public void UpdateText(string data) { entry.SetBinding(Entry.TextProperty, new Binding() { Path = data }); }
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
        #endregion
        #endregion

        public CustomEntry()
        {
            InitializeComponent();
            entry.Focused += Entry_Focused;
            entry.Unfocused += Entry_Unfocused;
            entry.IsPassword = Ispassword;
            SetupView();

            MessagingCenter.Subscribe<CreateAccount>(this, "AppThemeUpdated", (sender) =>
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
        }

        public void SetupView()
        {
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark"))
                {
                    entry.TextColor = Color.White;
                    BackgroundColor = Color.Black;
                    entry.PlaceholderColor = Color.White;
                }
                if (theme.Equals("dim"))
                {
                    entry.TextColor = Color.Default;
                    BackgroundColor = Color.SlateGray;
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
                    else BackgroundColor = Color.White;
                }
                else BackgroundColor = Color.White;
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
                    if (theme.Equals("dark")) BackgroundColor = Color.Black;
                    if (theme.Equals("dim")) BackgroundColor = Color.SlateGray;
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
            entry.Focus();
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