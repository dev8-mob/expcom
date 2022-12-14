
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
using Xperimen.Resources;
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
                if (arg.Equals("okay"))
                {
                    if (!string.IsNullOrEmpty(alert.CodeObject))
                    {
                        var result = viewmodel.DeleteExpense(alert.CodeObject);
                        if (result == 1)
                        {
                            calendar.SetupView();
                            SetDisplayAlert(AppResources.app_success, AppResources.exp_deleted, "", "", "");
                            var success = viewmodel.GetExpensesOnDate(viewmodel.SelectedDate);
                            if (success == 1)
                            {
                                if (viewmodel.NoExpenses) SetupView();
                                else
                                {
                                    lbl_intro.Text = AppResources.exp_noexpyet;
                                    if (!string.IsNullOrEmpty(viewmodel.SelectedDate))
                                    {
                                        var sampledate = new DateTime();
                                        var split = viewmodel.SelectedDate.Split('.');
                                        if (split.Count() > 0)
                                            sampledate = new DateTime(Convert.ToInt32(split[2]), Convert.ToInt32(split[1]), Convert.ToInt32(split[0]));
                                        lbl_ondateselect.Text = AppResources.exp_ondate + " " + sampledate.ToString("MMM d");
                                        lbl_ondateselect_zero.Text = sampledate.ToString("MMM d");
                                    }
                                }
                            }
                            if (success == 2) SetDisplayAlert(AppResources.app_error, AppResources.exp_errorgetexp, "", "", "");
                        }
                        else if (result == 2) SetDisplayAlert(AppResources.app_error, AppResources.exp_errordelexp, "", "", "");
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
                        lbl_ondateselect.Text = AppResources.exp_ondate + " " + sampledate.ToString("MMM d");
                        lbl_ondateselect_zero.Text = sampledate.ToString("MMM d");
                        lbl_intro.Text = AppResources.exp_summary;
                    }
                    viewmodel.IsLoading = false;
                }
                if (result == 2) SetDisplayAlert(AppResources.app_error, AppResources.exp_errorgetexp, "", "", "");
            });
            MessagingCenter.Subscribe<EditExpenses, string>(this, "ExpensesUpdated", (sender, arg) =>
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
                        lbl_ondateselect.Text = AppResources.exp_ondate + " " + sampledate.ToString("MMM d");
                        lbl_ondateselect_zero.Text = sampledate.ToString("MMM d");
                        lbl_intro.Text = AppResources.exp_summary;
                    }
                    viewmodel.IsLoading = false;
                }
                if (result == 2) SetDisplayAlert(AppResources.app_error, AppResources.exp_errorgetexp, "", "", "");
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
                            lbl_intro.Text = AppResources.exp_noexpyet;
                            lbl_ondateselect_zero.Text = sampledate.ToString("MMM d");
                        }
                        if (viewmodel.HasExpenses)
                            lbl_ondateselect.Text = AppResources.exp_ondate + " " + sampledate.ToString("MMM d");
                    }
                    viewmodel.IsLoading = false;
                }
                if (result == 2) SetDisplayAlert(AppResources.app_error, AppResources.exp_errorgetexp, "", "", "");
            });
            MessagingCenter.Subscribe<ListCell, string>(this, "ExpenseImageTap", (sender, arg) =>
            {
                var convert = new StreamByteConverter();
                var result = viewmodel.SetAttachmentPicture(arg);
                if (result == 1) Navigation.PushPopupAsync(new ImageViewer(convert.BytesToStream(viewmodel.Attachment)));
                else if (result == 2)
                {
                    viewmodel.IsLoading = true;
                    SetDisplayAlert(AppResources.app_notfound, AppResources.exp_picnotfound, "", "", "");
                }
                else if (result == 2)
                {
                    viewmodel.IsLoading = true;
                    SetDisplayAlert(AppResources.app_error, AppResources.exp_errorgetfile, "", "", "");
                }
            });
            MessagingCenter.Subscribe<ListCell, Dictionary<string, string>>(this, "DeleteImageTap", (sender, arg) =>
            {
                viewmodel.IsLoading = true;
                if (arg.Count > 0)
                { 
                    var id = arg.Where(x => x.Key == "id").FirstOrDefault().Value;
                    var amount = arg.Where(x => x.Key == "amount").FirstOrDefault().Value;
                    SetDisplayAlert(AppResources.app_confirm, AppResources.exp_asksuredelete + " " + amount + " " + AppResources.exp_fromexp, AppResources.app_okay, AppResources.app_cancel, id);
                }
            });
            MessagingCenter.Subscribe<AddIncome>(this, "IncomeUpdated", (sender) => { SetupView(); });
            #endregion
        }

        public void SetupView()
        {
            viewmodel.IsLoading = true;
            viewmodel.GetCommitmentList();
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Xamarin.Forms.Application.Current.Properties["app_theme"] as string;
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

            // setup for different iphone screen sizes
            if (Device.RuntimePlatform == Device.iOS)
            {
                var lowerscreen = DependencyService.Get<IDeviceInfo>().IsLowerIphoneDevice();
                if (lowerscreen)
                {
                    var safeInsets = On<Xamarin.Forms.PlatformConfiguration.iOS>().SafeAreaInsets();
                    safeInsets.Top = -20;
                    Padding = safeInsets;
                }
            }

            lbl_intro.Text = AppResources.exp_summary;
            lbl_ondateselect_zero.Text = string.Empty;
            var result = viewmodel.GetUserTotalExpense();
            if (result == 1) viewmodel.IsLoading = false;
            else if (result == 2) SetDisplayAlert(AppResources.app_notfound, AppResources.exp_thismonthnoexp, "", "", "");
            else if (result == 3) SetDisplayAlert(AppResources.app_error, AppResources.exp_errorgetallexp, "", "", "");
            BuildProgressbarUI();
        }

        public async void DrawerTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 250);
            view.Scale = 1;
            view.IsEnabled = false;
            var drawer = (DrawerMaster)view.Parent.Parent.Parent.Parent.Parent.Parent.Parent;
            drawer.IsPresented = true;
            view.IsEnabled = true;
        }

        public async void TopCommitmentBadgeClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 250);
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
            try // for error: auto-logout to login screen, if no total expenses for the month
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
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
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