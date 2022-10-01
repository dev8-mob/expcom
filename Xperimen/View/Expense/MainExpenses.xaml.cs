
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Stylekit;
using Xperimen.ViewModel.Expense;

namespace Xperimen.View.Expense
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainExpenses : ContentPage
    {
        public ExpensesViewmodel viewmodel;
        public CustomCalendar calendar;

        public MainExpenses()
        {
            InitializeComponent();
            viewmodel = new ExpensesViewmodel();
            BindingContext = viewmodel;
            calendar = customcalendar;

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", (sender, arg) =>
            { viewmodel.IsLoading = false; });
            MessagingCenter.Subscribe<AddRecord>(this, "ExpensesAdded", (sender) =>
            { calendar.SetupView(); });
            MessagingCenter.Subscribe<CustomCalendar, string>(this, "CalendarDateTap", (sender, arg) =>
            {
                viewmodel.IsLoading = true;
                var result = viewmodel.GetExpensesList(arg);
                if (result == 1)
                {
                    if (!string.IsNullOrEmpty(arg))
                    {
                        var sampledate = new DateTime();
                        var split = arg.Split('.');
                        if (split.Count() > 0)
                            sampledate = new DateTime(Convert.ToInt32(split[2]), Convert.ToInt32(split[1]), Convert.ToInt32(split[0]));
                        lbl_ondateselect.Text = "on " + sampledate.ToString("MMM d");
                    }
                    viewmodel.IsLoading = false;
                }
                if (result == 2) SetDisplayAlert("Error", "Technical error retrieving expenses for selected date.", "", "", "");
            });
        }

        public async void BackTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            await Navigation.PopAsync();
        }

        public async void AddExpensesClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            await Navigation.PushAsync(new AddRecord());
        }

        public async void ItemExpenseTapped(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
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