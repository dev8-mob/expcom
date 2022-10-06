using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
using Xperimen.Stylekit;
using Xperimen.ViewModel.Dashboard;

namespace Xperimen.View.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpensesDetail : ContentPage
    {
        public DashboardViewmodel viewmodel;
        public StreamByteConverter converter;

        public ExpensesDetail()
        {
            InitializeComponent();
            viewmodel = new DashboardViewmodel();
            converter = new StreamByteConverter();
            BindingContext = viewmodel;

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", (sender, arg) =>
            { 
                if (arg.Equals("Yes")) 
                {
                    var result = viewmodel.DeleteTodayExpenses(alert.CodeObject);
                    if (result == 1)
                    {
                        SetDisplayAlert("Success", "Expenses delete.", "", "", "");
                        MessagingCenter.Send(this, "ExpensesDeleted");
                        viewmodel.GetTodayExpenses();
                        if (viewmodel.ListExpenses.Count == 0) Navigation.PopAsync();
                    }
                    else if (result == 2) SetDisplayAlert("Error", "Technical error deleting selected expenses.", "", "", "");
                }
                else viewmodel.IsLoading = false; 
            });
        }

        public async void BackTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 250);
            view.Scale = 1;
            view.IsEnabled = false;
            await Navigation.PopAsync();
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