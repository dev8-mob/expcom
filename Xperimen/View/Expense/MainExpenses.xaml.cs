﻿
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
                            SetDisplayAlert("Success", "Selected expenses deleted.", "", "", "");
                            var success = viewmodel.GetExpensesOnDate(viewmodel.SelectedDate);
                            if (success == 1)
                            {
                                if (viewmodel.NoExpenses) SetupView();
                                else
                                {
                                    lbl_intro.Text = "No expenses yet for";
                                    if (!string.IsNullOrEmpty(viewmodel.SelectedDate))
                                    {
                                        var sampledate = new DateTime();
                                        var split = viewmodel.SelectedDate.Split('.');
                                        if (split.Count() > 0)
                                            sampledate = new DateTime(Convert.ToInt32(split[2]), Convert.ToInt32(split[1]), Convert.ToInt32(split[0]));
                                        lbl_ondateselect.Text = "on " + sampledate.ToString("MMM d");
                                        lbl_ondateselect_zero.Text = sampledate.ToString("MMM d");
                                    }
                                }
                            }
                            if (success == 2) SetDisplayAlert("Error", "Technical error retrieving expenses for selected date.", "", "", "");
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

                        if (viewmodel.NoExpenses)
                        {
                            SetupView();
                            lbl_intro.Text = "No expenses yet for";
                            lbl_ondateselect_zero.Text = sampledate.ToString("MMM d");
                        }
                        if (viewmodel.HasExpenses)
                            lbl_ondateselect.Text = "on " + sampledate.ToString("MMM d");
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
            MessagingCenter.Subscribe<AddIncome>(this, "IncomeUpdated", (sender) => { SetupView(); });
            #endregion
        }

        public void SetupView()
        {
            viewmodel.IsLoading = true;
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark"))
                {
                    stack_bgnoincome.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                    stack_bgpercentage.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                    frame_netincome.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                }
                if (theme.Equals("dim"))
                {
                    stack_bgnoincome.BackgroundColor = Color.FromHex(App.CharcoalGray);
                    stack_bgpercentage.BackgroundColor = Color.FromHex(App.CharcoalGray);
                    frame_netincome.BackgroundColor = Color.FromHex(App.CharcoalGray);
                }
                if (theme.Equals("light"))
                {
                    stack_bgnoincome.BackgroundColor = Color.FromHex(App.DimGray2);
                    stack_bgpercentage.BackgroundColor = Color.FromHex(App.DimGray2);
                    frame_netincome.BackgroundColor = Color.FromHex(App.DimGray2);
                }
            }

            lbl_intro.Text = "Expenses Summary";
            lbl_ondateselect_zero.Text = string.Empty;
            var result = viewmodel.GetUserTotalExpense();
            if (result == 1) viewmodel.IsLoading = false;
            else if (result == 2) SetDisplayAlert("No Data", "You do not have any expenses yet for this month.", "", "", "");
            else if (result == 3) SetDisplayAlert("Error", "Technical error retrieving all expenses.", "", "", "");
            BuildProgressbarUI();
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

        public async void SetIncomeClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            await Navigation.PushPopupAsync(new AddIncome());
            view.IsEnabled = true;
        }

        public void BuildProgressbarUI()
        {
            var usedsize = Math.Round(viewmodel.PercentageUsed, 2);
            var availsize = Math.Round(viewmodel.PercentageAvailable, 2);
            if (usedsize != 0 || availsize != 0)
            {
                var colused = new ColumnDefinition();
                var colavail = new ColumnDefinition();
                if (usedsize >= 100)
                {
                    colused = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) };
                    colavail = new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Star) };
                }
                else if (availsize >= 100)
                {
                    colused = new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Star) };
                    colavail = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) };
                }
                else if (usedsize <= 100 || availsize <= 100)
                {
                    colused = new ColumnDefinition() { Width = new GridLength(Math.Round(usedsize / 100, 2), GridUnitType.Star) };
                    colavail = new ColumnDefinition() { Width = new GridLength(Math.Round(availsize / 100, 2), GridUnitType.Star) };
                }

                var grid = new Grid { ColumnSpacing = 0 };
                grid.ColumnDefinitions.Add(colused);
                grid.ColumnDefinitions.Add(colavail);
                var stackused = new StackLayout
                {
                    BackgroundColor = Color.FromHex(App.CustomRedLight),
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                var stackavail = new StackLayout
                {
                    BackgroundColor = Color.FromHex(App.CustomGreenLight),
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                grid.Children.Add(stackused, 0, 0);
                grid.Children.Add(stackavail, 1, 0);
                frame_progressbar.Content = grid;
            }
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