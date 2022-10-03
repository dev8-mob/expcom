
using Rg.Plugins.Popup.Extensions;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
using Xperimen.Stylekit;
using Xperimen.View.NavigationDrawer;
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
            SetupView();

            #region messagingcenter
            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", (sender, arg) =>
            { 
                if (arg.Equals("Yes"))
                {
                    if (!string.IsNullOrEmpty(alert.CodeObject))
                    {
                        var result = viewmodel.DeleteExpense(alert.CodeObject);
                        if (result == 1)
                        {
                            calendar.SetupView();
                            var getlist = viewmodel.GetExpensesOnDate(viewmodel.SelectedDate);
                            if (getlist == 1)
                            {
                                if (!string.IsNullOrEmpty(viewmodel.SelectedDate))
                                {
                                    var sampledate = new DateTime();
                                    var split = viewmodel.SelectedDate.Split('.');
                                    if (split.Count() > 0)
                                        sampledate = new DateTime(Convert.ToInt32(split[2]), Convert.ToInt32(split[1]), Convert.ToInt32(split[0]));
                                    lbl_ondateselect.Text = "on " + sampledate.ToString("MMM d");
                                    lbl_ondateselect_zero.Text = sampledate.ToString("MMM d");
                                }
                                SetDisplayAlert("Success", "Selected expenses deleted.", "", "", "");
                            }
                            if (getlist == 2) SetDisplayAlert("Error", "Technical error retrieving expenses for selected date.", "", "", "");
                        }
                        else if (result == 2) SetDisplayAlert("Error", "Technical error deleting selected expenses.", "", "", "");
                    }
                }
                else viewmodel.IsLoading = false; 
            });
            MessagingCenter.Subscribe<AddRecord, string>(this, "ExpensesAdded", (sender, arg) =>
            { 
                calendar.SetupView();
                viewmodel.IsLoading = true;
                var result = viewmodel.GetExpensesOnDate(arg);
                if (result == 1)
                {
                    if (!string.IsNullOrEmpty(arg))
                    {
                        var sampledate = new DateTime();
                        var split = arg.Split('.');
                        if (split.Count() > 0)
                            sampledate = new DateTime(Convert.ToInt32(split[2]), Convert.ToInt32(split[1]), Convert.ToInt32(split[0]));
                        lbl_ondateselect.Text = "on " + sampledate.ToString("MMM d");
                        lbl_ondateselect_zero.Text = sampledate.ToString("MMM d");
                        lbl_intro.Text = "Expenses Summary";
                    }
                    viewmodel.IsLoading = false;
                }
                if (result == 2) SetDisplayAlert("Error", "Technical error retrieving expenses for selected date.", "", "", "");
            });
            MessagingCenter.Subscribe<CustomCalendar, string>(this, "CalendarDateTap", (sender, arg) =>
            {
                viewmodel.IsLoading = true;
                var result = viewmodel.GetExpensesOnDate(arg);
                if (result == 1)
                {
                    if (!string.IsNullOrEmpty(arg))
                    {
                        var sampledate = new DateTime();
                        var split = arg.Split('.');
                        if (split.Count() > 0)
                            sampledate = new DateTime(Convert.ToInt32(split[2]), Convert.ToInt32(split[1]), Convert.ToInt32(split[0]));
                        lbl_ondateselect.Text = "on " + sampledate.ToString("MMM d");
                        lbl_ondateselect_zero.Text = sampledate.ToString("MMM d");
                        if (viewmodel.NoExpenses)
                        {
                            viewmodel.GetUserTotalExpense();
                            lbl_intro.Text = "No expenses yet for";
                        }
                    }
                    viewmodel.IsLoading = false;
                }
                if (result == 2) SetDisplayAlert("Error", "Technical error retrieving expenses for selected date.", "", "", "");
            });
            MessagingCenter.Subscribe<ListCell, string>(this, "ExpenseImageTap", (sender, arg) =>
            {
                var convert = new StreamByteConverter();
                var result = viewmodel.SetAttachmentPicture(arg);
                if (result == 1) Navigation.PushPopupAsync(new ImageViewer(convert.BytesToStream(viewmodel.Attachment)));
                else if (result == 2)
                {
                    viewmodel.IsLoading = true;
                    SetDisplayAlert("Not Found", "The attachment picture is not found.", "", "", "");
                }
                else if (result == 2)
                {
                    viewmodel.IsLoading = true;
                    SetDisplayAlert("Error", "Technical error retrieving selected attachment file.", "", "", "");
                }
            });
            MessagingCenter.Subscribe<ListCell, string>(this, "DeleteImageTap", (sender, arg) =>
            {
                viewmodel.IsLoading = true;
                SetDisplayAlert("Confirmation", "Are you sure to delete the expenses ?", "Yes", "Cancel", arg);
            });
            #endregion
        }

        public void SetupView()
        {
            viewmodel.IsLoading = true;
            lbl_intro.Text = "Expenses Summary";
            lbl_ondateselect_zero.Text = string.Empty;
            var result = viewmodel.GetUserTotalExpense();
            if (result == 1) viewmodel.IsLoading = false;
            else if (result == 2) SetDisplayAlert("No Data", "User do not have any expenses yet for this month.", "", "", "");
            else if (result == 3) SetDisplayAlert("Error", "Technical error retrieving all user expenses.", "", "", "");
        }

        public async void DrawerTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            var drawer = (DrawerMaster)view.Parent.Parent.Parent.Parent.Parent.Parent;
            drawer.IsPresented = true;
            view.IsEnabled = true;
        }

        public async void RefreshClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            SetupView();
            view.IsEnabled = true;
        }

        public async void AddExpensesClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            await Navigation.PushAsync(new AddRecord());
            view.IsEnabled = true;
        }

        public async void SaveIncomeClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
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