
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Resources;
using Xperimen.Stylekit;
using Xperimen.ViewModel.Expense;

namespace Xperimen.View.Expense
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddIncome : PopupPage
    {
        public ExpensesViewmodel viewmodel;

        public AddIncome()
        {
            InitializeComponent();
            viewmodel = new ExpensesViewmodel();
            BindingContext = viewmodel;

            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) stack_bg.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                if (theme.Equals("dim")) stack_bg.BackgroundColor = Color.FromHex(App.CharcoalGray);
                if (theme.Equals("light")) stack_bg.BackgroundColor = Color.FromHex(App.DimGray2);
            }

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", async (sender, arg) =>
            { viewmodel.IsLoading = false; });
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

        public async void SaveClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            if (viewmodel.Income == 0) SetDisplayAlert(AppResources.app_empty, AppResources.exp_emptydesc, "", "", "");
            else
            {
                var result = viewmodel.UpdateIncome();
                if (result == 1)
                {
                    MessagingCenter.Send(this, "IncomeUpdated");
                    var navigation = Application.Current.MainPage.Navigation;
                    await navigation.PopPopupAsync();
                }
                else if (result == 2) SetDisplayAlert(AppResources.app_error, AppResources.exp_errorupdincome, "", "", "");
            }
            view.IsEnabled = true;
        }

        public async void CancelClicked(object sender, EventArgs e)
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