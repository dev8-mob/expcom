using Rg.Plugins.Popup.Extensions;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
using Xperimen.Model;
using Xperimen.Resources;
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
                var content = BuildMediaUI(viewmodel.ImageExpList[0].Picture, 1);
                var badge = BuildMediaBadge(viewmodel.ImageExpList.Count, 1);
                grid_media.Children.Add(content, column, row);
                grid_media.Children.Add(badge, column, row);
                column++;
            }
            if (viewmodel.ImageCommList.Count > 0)
            {
                var content = BuildMediaUI(viewmodel.ImageCommList[0].Picture, 2);
                var badge = BuildMediaBadge(viewmodel.ImageCommList.Count, 2);
                grid_media.Children.Add(content, column, row);
                grid_media.Children.Add(badge, column, row);
            }
        }

        public async void DrawerTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 250);
            view.Scale = 1;
            view.IsEnabled = false;
            var drawer = (DrawerMaster)view.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent;
            drawer.IsPresented = true;
            view.IsEnabled = true;
        }

        public async void TopCommitmentBadgeClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
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
                SetDisplayAlert(AppResources.media_alerttitlenopic, AppResources.media_alertdescnopic, "", "", "");
            view.IsEnabled = true;
        }

        #region build UI
        public StackLayout BuildMediaUI(byte[] picture, int code)
        {
            var container = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 15, 0, 0),
                Spacing = 5
            };
            var frame = new Frame
            {
                Padding = 0,
                HasShadow = false,
                CornerRadius = 8,
                IsClippedToBounds = true,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            var theme = string.Empty;
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("app_theme"))
                theme = Xamarin.Forms.Application.Current.Properties["app_theme"] as string;
            if (theme.Equals("dark")) frame.BackgroundColor = Color.FromHex(App.CharcoalBlack);
            if (theme.Equals("dim")) frame.BackgroundColor = Color.FromHex(App.CharcoalGray);
            if (theme.Equals("light")) frame.BackgroundColor = Color.FromHex(App.DimGray2);

            var img = new Image { Aspect = Aspect.AspectFill };
            img.Source = ImageSource.FromStream(() =>
            {
                var stream = converter.BytesToStream(picture);
                return stream;
            });
            var lbltitle = new XLabel { HorizontalOptions = LayoutOptions.Center };
            if (code == 1) lbltitle.Text = AppResources.media_colexpenses;
            else if (code == 2) lbltitle.Text = AppResources.media_colcommitment;
            frame.Content = img;
            container.Children.Add(frame);
            container.Children.Add(lbltitle);

            var tapexp = new TapGestureRecognizer(); var tapcom = new TapGestureRecognizer();
            tapexp.Tapped += ExpensesHeaderClicked; tapcom.Tapped += CommitmentHeaderClicked;
            if (code == 1) container.GestureRecognizers.Add(tapexp);
            else if (code == 2) container.GestureRecognizers.Add(tapcom);
            return container;
        }

        public Frame BuildMediaBadge(int count, int code)
        {
            var container = new Frame
            {
                Padding = 0,
                HasShadow = false,
                IsClippedToBounds = true,
                BackgroundColor = Color.FromHex(App.CustomRed),
                HeightRequest = 30,
                WidthRequest = 30,
                Margin = new Thickness(0, 0, 15, 0),
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start
            };
            var text = new Label
            {
                Text = count.ToString(),
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            if (Device.RuntimePlatform == Device.Android)
            { text.FontFamily = "Ubuntu-Bold.ttf#Ubuntu Bold"; container.CornerRadius = 30; }
            else if (Device.RuntimePlatform == Device.iOS)
            { text.FontFamily = "Ubuntu-Bold"; container.CornerRadius = 15; }
            container.Content = text;

            var tapexp = new TapGestureRecognizer(); var tapcom = new TapGestureRecognizer();
            tapexp.Tapped += ExpensesBadgeClicked; tapcom.Tapped += CommitmentBadgeClicked;
            if (code == 1) container.GestureRecognizers.Add(tapexp);
            else if (code == 2) container.GestureRecognizers.Add(tapcom);
            return container;
        }

        public Frame BuildMediaList(Expenses exp, SelfCommitment comm)
        {
            var container = new Frame
            {
                Padding = 0,
                HasShadow = false,
                CornerRadius = 8,
                IsClippedToBounds = true,
                Margin = new Thickness(0, 0, 15, 0),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            var stack = new StackLayout 
            { 
                Spacing = 0, 
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            var lblid = new Label { IsVisible = false };
            var img = new Image 
            { 
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            if (exp != null)
            {
                lblid.Text = exp.Id;
                img.Source = ImageSource.FromStream(() =>
                {
                    var stream = converter.BytesToStream(exp.Picture);
                    return stream;
                });
            }
            if (comm != null)
            {
                lblid.Text = comm.Id;
                img.Source = ImageSource.FromStream(() =>
                {
                    var stream = converter.BytesToStream(comm.Picture);
                    return stream;
                });
            }

            var theme = string.Empty;
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("app_theme"))
                theme = Xamarin.Forms.Application.Current.Properties["app_theme"] as string;
            if (theme.Equals("dark")) container.BackgroundColor = Color.FromHex(App.CharcoalBlack);
            if (theme.Equals("dim")) container.BackgroundColor = Color.FromHex(App.CharcoalGray);
            if (theme.Equals("light")) container.BackgroundColor = Color.FromHex(App.DimGray2);
            stack.Children.Add(lblid);
            stack.Children.Add(img);
            container.Content = stack;

            var tapexp = new TapGestureRecognizer(); var tapcom = new TapGestureRecognizer();
            tapexp.Tapped += ExpensesListClicked; tapcom.Tapped += CommitmentListClicked;
            if (exp != null) container.GestureRecognizers.Add(tapexp);
            if (comm != null) container.GestureRecognizers.Add(tapcom);
            return container;
        }
        #endregion

        #region column and badge tap event
        public async void ExpensesHeaderClicked(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            BuildExpensesMedia();
        }

        public async void ExpensesBadgeClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            BuildExpensesMedia();
        }

        public void BuildExpensesMedia()
        {
            stack_piclist.IsVisible = true;
            lbl_selected.Text = AppResources.media_colexpenseselected;
            if (viewmodel.HaveDateRange) { lbl_expdate.IsVisible = false; lbl_expdaterange.IsVisible = true; }
            else if (!viewmodel.HaveDateRange) { lbl_expdate.IsVisible = true; lbl_expdaterange.IsVisible = false; }
            grid_medialist.Children.Clear();
            grid_medialist.ColumnDefinitions.Clear();
            if (viewmodel.ImageExpList.Count > 0)
            {
                int row = 0; int col = 0;
                foreach (var item in viewmodel.ImageExpList)
                {
                    grid_medialist.ColumnDefinitions.Add(new ColumnDefinition { Width = 60 });
                    var content = BuildMediaList(item, null);
                    grid_medialist.Children.Add(content, col, row);
                    col++;
                }
            }
        }

        public async void ExpensesListClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            var stack = (StackLayout)view.Content;
            var lblid = (Label)stack.Children[0];

            view.IsEnabled = false;
            var expdata = viewmodel.ImageExpList.Where(x => x.Id.Equals(lblid.Text)).ToList();
            if (expdata.Count > 0) 
                await Navigation.PushPopupAsync(new ImageDetails(expdata[0], null));
            view.IsEnabled = true;
        }

        public async void CommitmentHeaderClicked(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            BuildCommitmentMedia();
        }

        public async void CommitmentBadgeClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            BuildCommitmentMedia();
        }

        public void BuildCommitmentMedia()
        {
            stack_piclist.IsVisible = true;
            lbl_selected.Text = AppResources.media_colcommitmentselected;
            lbl_expdate.IsVisible = false; lbl_expdaterange.IsVisible = false;
            grid_medialist.Children.Clear();
            grid_medialist.ColumnDefinitions.Clear();
            if (viewmodel.ImageCommList.Count > 0)
            {
                int row = 0; int col = 0;
                foreach (var item in viewmodel.ImageCommList)
                {
                    grid_medialist.ColumnDefinitions.Add(new ColumnDefinition { Width = 60 });
                    var content = BuildMediaList(null, item);
                    grid_medialist.Children.Add(content, col, row);
                    col++;
                }
            }
        }

        public async void CommitmentListClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            var stack = (StackLayout)view.Content;
            var lblid = (Label)stack.Children[0];

            view.IsEnabled = false;
            var commdata = viewmodel.ImageCommList.Where(x => x.Id.Equals(lblid.Text)).ToList();
            if (commdata.Count > 0)
                await Navigation.PushPopupAsync(new ImageDetails(null, commdata[0]));
            view.IsEnabled = true;
        }
        #endregion

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