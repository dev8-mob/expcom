using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.View.Dashboard;
using Xperimen.ViewModel;

namespace Xperimen.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public LoginViewmodel model;

        public Login()
        {
            InitializeComponent();
            model = new LoginViewmodel();
            BindingContext = model;
        }

        public async void Submit_Clicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 50);
            view.Scale = 1;

            var user = entry_username.GetText();
            var password = entry_password.GetText();

            if (string.IsNullOrEmpty(user))
            {
                model.IsLoading = true;
                SetDisplayAlert("Username empty !?", "Where is your username !? Please insert your username.", "Okay", "Dont Want");
                entry_username.Isfocus = true;
            }
            else if (string.IsNullOrEmpty(password))
            {
                model.IsLoading = true;
                SetDisplayAlert("Password Empty !?", "Where is your password !? Please insert your password.", "Okay", "Dont Want");
                entry_password.Isfocus = true;
            }
            else await Navigation.PushAsync(new MainPage());
        }

        #region custom DisplayAlert implementation
        public void SetDisplayAlert(string title, string description, string btn1, string btn2)
        {
            alert.Title = title;
            alert.Description = description;
            alert.TxtBtn1 = btn1;
            alert.TxtBtn2 = btn2;
            alert.IsVisible = true;

            var tapClose = new TapGestureRecognizer();
            tapClose.Tapped += CloseTapped;
            var grid = (Grid)alert;
            var btnClose = (Image)grid.Children[1];
            btnClose.GestureRecognizers.Add(tapClose);

            if (!string.IsNullOrEmpty(btn1))
            {
                var frame = (Frame)grid.Children[0];
                var stack = (StackLayout)frame.Content;
                var stack2 = (StackLayout)stack.Children[2];
                var stackBtn = (StackLayout)stack2.Children[0];

                var tapBtn1 = new TapGestureRecognizer();
                tapBtn1.Tapped += Btn1Tapped;
                stackBtn.GestureRecognizers.Add(tapBtn1);
            }

            if (!string.IsNullOrEmpty(btn2))
            {
                var frame = (Frame)grid.Children[0];
                var stack = (StackLayout)frame.Content;
                var stack2 = (StackLayout)stack.Children[2];
                var stackBtn = (StackLayout)stack2.Children[2];

                var tapBtn2 = new TapGestureRecognizer();
                tapBtn2.Tapped += Btn2Tapped;
                stackBtn.GestureRecognizers.Add(tapBtn2);
            }
        }

        public async void Btn1Tapped(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
        }

        public async void Btn2Tapped(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
        }

        public async void CloseTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            model.IsLoading = false;
            alert.IsVisible = false;
        }
        #endregion
    }
}