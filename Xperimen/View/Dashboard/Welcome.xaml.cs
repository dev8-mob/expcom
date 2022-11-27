using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
using Xperimen.Resources;
using Xperimen.Stylekit;
using Xperimen.View.Expense;
using Xperimen.View.NavigationDrawer;
using Xperimen.ViewModel.Dashboard;

namespace Xperimen.View.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Welcome : ContentPage
    {
        public DashboardViewmodel viewmodel;
        public StreamByteConverter converter;

        public Welcome()
        {
            InitializeComponent();
            viewmodel = new DashboardViewmodel();
            converter = new StreamByteConverter();
            BindingContext = viewmodel;
            SetupView();

            #region messagingcenter
            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", (sender, arg) =>
            { viewmodel.IsLoading = false; });
            MessagingCenter.Subscribe<Commitment.AddRecord>(this, "CommitmentAdded", (sender) => 
            { viewmodel.SetupData(); BuildProgressbarUI(); });
            MessagingCenter.Subscribe<AddExpenses, string>(this, "ExpensesAdded", (sender, arg) =>
            { viewmodel.SetupData(); BuildProgressbarUI(); });
            MessagingCenter.Subscribe<DashboardViewmodel>(this, "ExpensesDeleted", (sender) =>
            { viewmodel.SetupData(); BuildProgressbarUI(); });
            MessagingCenter.Subscribe<DashboardViewmodel>(this, "CommitmentSetDone", (sender) =>
            { viewmodel.SetupData(); BuildProgressbarUI(); });
            MessagingCenter.Subscribe<AddIncome>(this, "IncomeUpdated", (sender) =>
            { viewmodel.SetupData(); BuildProgressbarUI(); });
            #endregion
        }

        public void SetupView()
        {
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Xamarin.Forms.Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark"))
                {
                    img_profile.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                    frame_expense.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                    frame_noincome.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                    frame_summary.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                }
                if (theme.Equals("dim"))
                {
                    img_profile.BackgroundColor = Color.FromHex(App.CharcoalGray);
                    frame_expense.BackgroundColor = Color.FromHex(App.CharcoalGray);
                    frame_noincome.BackgroundColor = Color.FromHex(App.CharcoalGray);
                    frame_summary.BackgroundColor = Color.FromHex(App.CharcoalGray);
                }
                if (theme.Equals("light"))
                {
                    img_profile.BackgroundColor = Color.FromHex(App.DimGray2);
                    frame_expense.BackgroundColor = Color.FromHex(App.DimGray2);
                    frame_noincome.BackgroundColor = Color.FromHex(App.DimGray2);
                    frame_summary.BackgroundColor = Color.FromHex(App.DimGray2);
                }
            }
            else
            {
                img_profile.BackgroundColor = Color.FromHex(App.DimGray2);
                frame_expense.BackgroundColor = Color.FromHex(App.DimGray2);
                frame_noincome.BackgroundColor = Color.FromHex(App.DimGray2);
                frame_summary.BackgroundColor = Color.FromHex(App.DimGray2);
            }

            if (viewmodel.Picture != null)
            {
                img_profile.Source = ImageSource.FromStream(() =>
                {
                    var stream = converter.BytesToStream(viewmodel.Picture);
                    return stream;
                });
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
            BuildProgressbarUI();
        }

        public void BuildProgressbarUI()
        {
            try // for error: auto-logout to login screen
            {
                var expsize = Math.Round(viewmodel.PercentageExpenses, 2);
                var commitsize = Math.Round(viewmodel.PercentageCommitment, 2);
                var availsize = Math.Round(viewmodel.PercentageAvailable, 2);
                if (commitsize != 0 || availsize != 0 || expsize != 0)
                {
                    var colexp = new ColumnDefinition();
                    var colcommit = new ColumnDefinition();
                    var colavail = new ColumnDefinition();
                    if (expsize >= 100)
                    {
                        colexp = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) };
                        colcommit = new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Star) };
                        colavail = new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Star) };
                    }
                    else if (commitsize >= 100)
                    {
                        colexp = new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Star) };
                        colcommit = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) };
                        colavail = new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Star) };
                    }
                    else if (availsize >= 100)
                    {
                        colexp = new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Star) };
                        colcommit = new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Star) };
                        colavail = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) };
                    }
                    else if (expsize <= 100 || commitsize <= 100 || availsize <= 100)
                    {
                        colexp = new ColumnDefinition() { Width = new GridLength(Math.Round(expsize / 100, 2), GridUnitType.Star) };
                        colcommit = new ColumnDefinition() { Width = new GridLength(Math.Round(commitsize / 100, 2), GridUnitType.Star) };
                        colavail = new ColumnDefinition() { Width = new GridLength(Math.Round(availsize / 100, 2), GridUnitType.Star) };
                    }

                    var grid = new Grid { ColumnSpacing = 0 };
                    grid.ColumnDefinitions.Add(colcommit);
                    grid.ColumnDefinitions.Add(colexp);
                    grid.ColumnDefinitions.Add(colavail);
                    var stackused = new StackLayout
                    {
                        BackgroundColor = Color.FromHex(App.CustomRedLight),
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    var stackexp = new StackLayout
                    {
                        BackgroundColor = Color.FromHex(App.CustomGreenLight),
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    var stackavail = new StackLayout
                    {
                        BackgroundColor = Color.FromHex(App.PrimaryLight),
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    grid.Children.Add(stackused, 0, 0);
                    grid.Children.Add(stackexp, 1, 0);
                    grid.Children.Add(stackavail, 2, 0);
                    frame_progressbar.Content = grid;
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
            }
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

        public async void SetIncomeClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            await Navigation.PushPopupAsync(new AddIncome());
            view.IsEnabled = true;
        }

        public async void ProfilePicClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            if (viewmodel.Picture != null)
            {
                view.IsEnabled = false;
                await Navigation.PushPopupAsync(new ImageViewer(converter.BytesToStream(viewmodel.Picture)));
                view.IsEnabled = true;
            }
        }

        public async void ExpensesColumnClicked(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;
            await view.ScaleTo(0.9, 250);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            if (viewmodel.NoExpenses) SetDisplayAlert(AppResources.dashboard_noexp, AppResources.dashboard_noexpdesc, "", "", "");
            else if (viewmodel.HasExpenses)
            { await Navigation.PushAsync(new ExpensesDetail()); viewmodel.IsLoading = false; }
            view.IsEnabled = true;
        }

        public async void ExpensesBadgeClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 250);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            if (viewmodel.NoExpenses) SetDisplayAlert("No Expenses", "You have no expenses for today.", "", "", "");
            else if (viewmodel.HasExpenses)
            { await Navigation.PushAsync(new ExpensesDetail()); viewmodel.IsLoading = false; }
            view.IsEnabled = true;
        }

        public async void CommitmentColumnClicked(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;
            await view.ScaleTo(0.9, 250);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            try
            {
                if (viewmodel.NoCommitment) SetDisplayAlert("No Commitment", "You have no commitment for this month.", "", "", "");
                else if (viewmodel.HasCommitment)
                {
                    if (viewmodel.AllCommitmentDone) SetDisplayAlert("Completed", "All commitments for this month are completed.", "", "", "");
                    else
                    {
                        await Navigation.PushAsync(new CommitmentDetails());
                        viewmodel.IsLoading = false;
                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
            }
            view.IsEnabled = true;
        }

        public async void CommitmentBadgeClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 250);
            view.Scale = 1;
            view.IsEnabled = false;
            var drawer = (DrawerMaster)view.Parent.Parent.Parent.Parent.Parent.Parent;
            drawer.IsPresented = true;
            view.IsEnabled = true;
        }

        public async void MoreInfoClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 250);
            view.Scale = 1;
            view.IsEnabled = false;
            viewmodel.IsLoading = true;
            var build = "Income :   " + viewmodel.Currency + " " + string.Format("{0:0.00}", viewmodel.Income) 
                + "\r\nTotal Commitment :   " + viewmodel.Currency + " " + string.Format("{0:0.00}", viewmodel.TotalCommitment)
                + "\r\nTotal Expenses :   " + viewmodel.Currency + " " + string.Format("{0:0.00}", viewmodel.TotalExpenses);
            var avail = "Available :   " + viewmodel.Currency + " " + string.Format("{0:0.00}", viewmodel.BalanceAvailable);
            SetDisplayAlert("Summary", build, avail, "", "");
            view.IsEnabled = true;
        }

        public async void AddCommitmentClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            await Navigation.PushAsync(new Commitment.AddRecord());
            view.IsEnabled = true;
        }

        public async void AddExpensesClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            await Navigation.PushAsync(new AddExpenses());
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