
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xperimen.Stylekit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomDisplayAlert : Grid
    {
        #region bindables
        #region properties
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public string TxtBtn1
        {
            get { return (string)GetValue(TxtBtn1Property); }
            set { SetValue(TxtBtn1Property, value); }
        }
        public string TxtBtn2
        {
            get { return (string)GetValue(TxtBtn2Property); }
            set { SetValue(TxtBtn2Property, value); }
        }
        public string CodeObject
        {
            get { return (string)GetValue(CodeObjectProperty); }
            set { SetValue(CodeObjectProperty, value); }
        }
        #endregion
        #region properties binding
        public static BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomDisplayAlert), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomDisplayAlert)bindable).UpdateTitle((string)newVal); });
        public static BindableProperty DescriptionProperty =
            BindableProperty.Create(nameof(Description), typeof(string), typeof(CustomDisplayAlert), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomDisplayAlert)bindable).UpdateContent((string)newVal); });
        public static BindableProperty TxtBtn1Property =
            BindableProperty.Create(nameof(TxtBtn1), typeof(string), typeof(CustomDisplayAlert), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomDisplayAlert)bindable).UpdateTxtBtn1((string)newVal); });
        public static BindableProperty TxtBtn2Property =
            BindableProperty.Create(nameof(TxtBtn2), typeof(string), typeof(CustomDisplayAlert), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomDisplayAlert)bindable).UpdateTxtBtn2((string)newVal); });
        public static BindableProperty CodeObjectProperty =
            BindableProperty.Create(nameof(CodeObject), typeof(string), typeof(CustomDisplayAlert), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomDisplayAlert)bindable).UpdateCodeObject((string)newVal); });
        #endregion
        #region binding implementation
        public void UpdateTitle(string data) { lbl_title.Text = data; }
        public void UpdateContent(string data) { lbl_desc.Text = data; }
        public void UpdateTxtBtn1(string data) 
        {
            var isEmpty = string.IsNullOrEmpty(data);
            lbl_btn1.Text = data;
            stack_btn1.IsVisible = !isEmpty;

            if (string.IsNullOrEmpty(TxtBtn1) && string.IsNullOrEmpty(TxtBtn2)) stack_buttons.IsVisible = false;
            else stack_buttons.IsVisible = true;
        }
        public void UpdateTxtBtn2(string data) 
        {
            var isEmpty = string.IsNullOrEmpty(data);
            lbl_btn2.Text = data;
            stack_btn2.IsVisible = !isEmpty;

            if (string.IsNullOrEmpty(TxtBtn1) && string.IsNullOrEmpty(TxtBtn2)) stack_buttons.IsVisible = false;
            else stack_buttons.IsVisible = true;
        }
        public void UpdateCodeObject(string data) { }
        #endregion
        #endregion

        public CustomDisplayAlert()
        {
            InitializeComponent();
        }

        public async void Btn1Tapped(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            await this.FadeTo(0, 500);
            IsVisible = false;
            MessagingCenter.Send(this, "DisplayAlertSelection", lbl_btn1.Text);
            Opacity = 1;
        }

        public async void Btn2Tapped(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            await this.FadeTo(0, 500);
            IsVisible = false;
            MessagingCenter.Send(this, "DisplayAlertSelection", lbl_btn2.Text);
            Opacity = 1;
        }

        public async void CloseTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            await this.FadeTo(0, 500);
            IsVisible = false;
            MessagingCenter.Send(this, "DisplayAlertSelection", "");
            Opacity = 1;
        }
    }
}