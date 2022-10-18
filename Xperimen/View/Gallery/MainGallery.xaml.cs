using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
using Xperimen.Stylekit;
using Xperimen.View.NavigationDrawer;
using Xperimen.ViewModel.Gallery;

namespace Xperimen.View.Gallery
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainGallery : ContentPage
    {
        public GalleryViewmodel viewmodel;
        public StreamByteConverter converter;

        public MainGallery()
        {
            InitializeComponent();
            viewmodel = new GalleryViewmodel();
            converter = new StreamByteConverter();
            BindingContext = viewmodel;
            SetupView();

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", (sender, arg) =>
            { viewmodel.IsLoading = false; });
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

            int row = 0; int column = 0;
            if (viewmodel.ImageExpList.Count > 0)
            {
                var content = BuildMediaUI(viewmodel.ImageExpList[0]);
                grid_media.Children.Add(content, column, row);
                column++;
            }
            if (viewmodel.ImageCommList.Count > 0)
            {
                var content = BuildMediaUI(viewmodel.ImageCommList[0]);
                grid_media.Children.Add(content, column, row);
            }
        }

        public Frame BuildMediaUI(byte[] picture)
        {
            var container = new Frame
            {
                Padding = 0,
                HasShadow = false,
                CornerRadius = 8,
                IsClippedToBounds = true,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            var theme = string.Empty;
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("app_theme"))
                theme = Xamarin.Forms.Application.Current.Properties["app_theme"] as string;
            if (theme.Equals("dark")) container.BackgroundColor = Color.FromHex(App.CharcoalBlack);
            if (theme.Equals("dim")) container.BackgroundColor = Color.FromHex(App.CharcoalGray);
            if (theme.Equals("light")) container.BackgroundColor = Color.FromHex(App.DimGray2);

            var img = new Image
            {
                HeightRequest = 70,
                WidthRequest = 70,
                Aspect = Aspect.AspectFill
            };
            img.Source = ImageSource.FromStream(() =>
            {
                var stream = converter.BytesToStream(picture);
                return stream;
            });
            container.Content = img;
            return container;
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

        public async void NoPicturesClicked(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;
            await view.ScaleTo(0.9, 250);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            if (viewmodel.NoMedia)
                SetDisplayAlert("No Pictures", "You have not attach any pictures in Expenses or Commitment.", "", "", "");
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