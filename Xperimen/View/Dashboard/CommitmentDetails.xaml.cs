
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
    public partial class CommitmentDetails : ContentPage
    {
        public DashboardViewmodel viewmodel;
        public StreamByteConverter converter;

        public CommitmentDetails()
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
                    if (alert.CodeObject.Equals("alldone"))
                    {
                        var result = viewmodel.SetAllCommitmentDonePaid();
                        if (result == 1) { viewmodel.IsLoading = false; await Navigation.PopAsync(); }
                        else if (result == 2) SetDisplayAlert("No Data", "User commitment list not found.", "", "", "");
                        else if (result == 3) SetDisplayAlert("Error", "Technical error retrieving users commitment list.", "", "", "");
                    }
                    else
                    {
                        var result = viewmodel.SetCommitmentDonePaid(alert.CodeObject, true);
                        if (result == 1) { viewmodel.IsLoading = false; await Navigation.PopAsync(); }
                        else if (result == 2) SetDisplayAlert("Error", "Technical error set the commitment to done.", "", "", "");
                    }
                }
                else viewmodel.IsLoading = false; 
            });
        }

        public void SetupView()
        {
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

        public async void AllDonePaidClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 250);
            view.Scale = 1;
            view.IsEnabled = false;
            viewmodel.IsLoading = true;
            SetDisplayAlert("Confirmation", "Mark all commitment as done ?", "Yes", "Cancel", "alldone");
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