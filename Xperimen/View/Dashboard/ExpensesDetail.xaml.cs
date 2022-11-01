using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
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
            SetupView();

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", async (sender, arg) =>
            { 
                if (arg.Equals("Yes")) 
                {
                    var result = viewmodel.DeleteTodayExpenses(alert.CodeObject);
                    if (result == 1)
                    {
                        var success = viewmodel.GetTodayExpenses();
                        if (success == 1)
                        { SetDisplayAlert("Success", "Expenses delete.", "", "", ""); SetupView(); }
                        else if (success == 2) await Navigation.PopAsync();
                        else if (success == 3) SetDisplayAlert("Error", "Technical error deleting selected expenses.", "", "", "");
                    }
                    else if (result == 2) SetDisplayAlert("Error", "Technical error deleting selected expenses.", "", "", "");
                }
                else viewmodel.IsLoading = false; 
            });
        }

        public void SetupView()
        {
            var theme = string.Empty;
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("app_theme"))
                theme = Xamarin.Forms.Application.Current.Properties["app_theme"] as string;
            if (theme.Equals("dim"))
            { 
                lbl_percentmore.TextColor = Color.Black; lbl_percentless.TextColor = Color.Black;
                //lbl_percentmorevalue.TextColor = Color.Black; lbl_percentlessvalue.TextColor = Color.Black;
            }

            if (viewmodel.DiffYtdToday > 0)
            {
                stack_more.IsVisible = true;
                stack_less.IsVisible = false;
                stack_same.IsVisible = false;
            }
            else if (viewmodel.DiffYtdToday < 0)
            {
                stack_more.IsVisible = false;
                stack_less.IsVisible = true;
                stack_same.IsVisible = false;
            }
            else if (viewmodel.DiffYtdToday == 0)
            {
                stack_more.IsVisible = false;
                stack_less.IsVisible = false;
                stack_same.IsVisible = true;
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