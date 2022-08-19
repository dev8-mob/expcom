
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            set { SetValue(TextProperty, value); OnPropertyChanged(); }
        }
        public string GetText() { return entry.Text; }
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
        public string ImgRight
        {
            get { return (string)GetValue(ImgRightProperty); }
            set { SetValue(ImgRightProperty, value); }
        }
        public string ImgRight2
        {
            get { return (string)GetValue(ImgRight2Property); }
            set { SetValue(ImgRight2Property, value); }
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
        #region custom properties binding
        public static BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomEntry), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateText((string)newVal); });
        public static BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(CustomEntry), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdatePlaceholder((string)newVal); });
        public static BindableProperty ImgLeftProperty =
            BindableProperty.Create(nameof(ImgLeft), typeof(string), typeof(CustomEntry), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateImgLeft((string)newVal); });
        public static BindableProperty ImgRightProperty =
            BindableProperty.Create(nameof(ImgRight), typeof(string), typeof(CustomEntry), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateImgRight((string)newVal); });
        public static BindableProperty ImgRight2Property =
            BindableProperty.Create(nameof(ImgRight2), typeof(string), typeof(CustomEntry), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateImgRight2((string)newVal); });
        public static BindableProperty IsPasswordProperty =
            BindableProperty.Create(nameof(Ispassword), typeof(bool), typeof(CustomEntry), defaultValue: false,
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateIspassword((bool)newVal); });
        public static BindableProperty IsFocusProperty =
            BindableProperty.Create(nameof(Isfocus), typeof(bool), typeof(CustomEntry), defaultValue: false,
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateIsfocus((bool)newVal); });
        #endregion
        #region binding implementation
        public void UpdateText(string data) { entry.Text = data; }
        public void UpdatePlaceholder(string data) { entry.Placeholder = data; }
        public void UpdateImgLeft(string data) 
        {
            if (!string.IsNullOrEmpty(data))
            {
                img_left.Source = data;
                img_left.IsVisible = true;
            }
            else img_left.IsVisible = false;
        }
        public void UpdateImgRight(string data) 
        { 
            if (!string.IsNullOrEmpty(data))
            {
                img_right.Source = data;
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
        public void UpdateIspassword(bool data) { entry.IsPassword = data; }
        public void UpdateIsfocus(bool data) { if (data) entry.Focus(); else entry.Unfocus(); }
        #endregion
        #endregion

        public CustomEntry()
        {
            InitializeComponent();
            entry.Focused += Entry_Focused;
            entry.Unfocused += Entry_Unfocused;

            if (Ispassword) entry.IsPassword = true;
            else entry.IsPassword = false;
        }

        private void Entry_Focused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused)
            {
                line.BackgroundColor = Color.FromHex(App.Primary);
                BackgroundColor = Color.White;
            }
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            var view = (Entry)sender;
            if (!e.IsFocused)
            {
                line.BackgroundColor = Color.DarkGray;
                BackgroundColor = Color.FromHex(App.DimGray2);
            }
        }

        private async void OnShowHideTap(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;

            var src = view.Source.ToString();
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