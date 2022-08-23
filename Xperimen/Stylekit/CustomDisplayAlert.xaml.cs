
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
        public bool IsOkay
        {
            get { return (bool)GetValue(IsOkayProperty); }
            set { SetValue(IsOkayProperty, value); }
        }
        public bool IsCancel
        {
            get { return (bool)GetValue(IsCancelProperty); }
            set { SetValue(IsCancelProperty, value); }
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
        public static BindableProperty IsOkayProperty =
            BindableProperty.Create(nameof(IsOkay), typeof(bool), typeof(CustomDisplayAlert), defaultValue: false,
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomDisplayAlert)bindable).UpdateIsOkay((bool)newVal); });
        public static BindableProperty IsCancelProperty =
            BindableProperty.Create(nameof(IsCancel), typeof(bool), typeof(CustomDisplayAlert), defaultValue: false,
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomDisplayAlert)bindable).UpdateIsCancel((bool)newVal); });
        #endregion
        #region binding implementation
        public void UpdateTitle(string data) { lbl_title.Text = data; }
        public void UpdateContent(string data) { lbl_desc.Text = data; }
        public void UpdateTxtBtn1(string data) 
        {
            var isEmpty = string.IsNullOrEmpty(data);
            lbl_btn1.Text = data;
            lbl_btn1.IsVisible = !isEmpty;
        }
        public void UpdateTxtBtn2(string data) 
        {
            var isEmpty = string.IsNullOrEmpty(data);
            lbl_btn2.Text = data;
            lbl_btn2.IsVisible = !isEmpty;
        }
        public void UpdateIsOkay(bool data)
        { IsOkay = data; }
        public void UpdateIsCancel(bool data)
        { IsCancel = data; }
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
            IsOkay = true;
        }

        public async void Btn2Tapped(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            IsCancel = true;
        }

        public async void CloseTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
        }
    }
}