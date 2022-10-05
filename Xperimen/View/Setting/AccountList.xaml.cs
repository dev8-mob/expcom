using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Stylekit;
using Xperimen.ViewModel.Setting;

namespace Xperimen.View.Setting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountList : PopupPage
    {
        public AccountViewmodel viewmodel;

        public AccountList()
        {
            InitializeComponent();
            viewmodel = new AccountViewmodel();
            BindingContext = viewmodel;

            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) stack_bg.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                if (theme.Equals("dim")) stack_bg.BackgroundColor = Color.FromHex(App.CharcoalGray);
                if (theme.Equals("light")) stack_bg.BackgroundColor = Color.FromHex(App.DimGray2);
            }

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", (sender, arg) =>
            {
                if (arg.Equals("Okay"))
                {
                    var result = viewmodel.DeleteUser(alert.CodeObject);
                    if (result == 1)
                    {
                        SetDisplayAlert("Success", "User deleted.", "", "", "");
                        viewmodel.GetClientsList();
                        viewmodel.IsLoading = false;
                    }
                    else if (result == 2) SetDisplayAlert("Failed", "Technical error. Failed to delete the user.", "", "", "");
                }
                else viewmodel.IsLoading = false;
            });
        }

        protected override bool OnBackButtonPressed()
        {
            // Invoked when a hardware back button is pressed
            // Return true if don't want to close this popup when back button is pressed
            return true;
        }

        protected override bool OnBackgroundClicked()
        {
            // Invoked when background is clicked
            // Return false if don't want to close this popup when background of popup is clicked
            return false;
        }

        private void listview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var view = (ListView)sender;
            if (view.SelectedItem != null) 
                view.SelectedItem = null;
        }

        public async void OkayClicked(object sender, EventArgs e)
        {
            var view = (Label)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            var navigation = Application.Current.MainPage.Navigation;
            await navigation.PopPopupAsync();
            view.IsEnabled = true;
        }

        public void SetDisplayAlert(string title, string description, string btn1, string btn2, string obj)
        {
            //if string1 empty will not display btn1, if string2 empty will not display btn2
            //if both string1 & string2 empty will not display all buttons
            //all buttons tapped will send 'DisplayAlertSelection' with text of the button
            //close button tapped will send 'DisplayAlertSelection' with empty text
            alert.Title = title;
            alert.Description = description;
            alert.TxtBtn1 = btn1;
            alert.TxtBtn2 = btn2;
            alert.IsVisible = true;
            alert.CodeObject = obj;
        }
    }
}