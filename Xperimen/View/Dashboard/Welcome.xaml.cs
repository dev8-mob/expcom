using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
using Xperimen.Stylekit;
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
            MessagingCenter.Subscribe<Commitment.AddRecord>(this, "CommitmentAdded", (sender) => 
            { viewmodel.SetupData(); });
            MessagingCenter.Subscribe<AddExpenses, string>(this, "ExpensesAdded", (sender, arg) =>
            { viewmodel.SetupData(); });
            #endregion
        }

        public void SetupView()
        {
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark"))
                {
                    img_profile.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                    frame_commit.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                    frame_expense.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                }
                if (theme.Equals("dim"))
                {
                    img_profile.BackgroundColor = Color.FromHex(App.CharcoalGray);
                    frame_commit.BackgroundColor = Color.FromHex(App.CharcoalGray);
                    frame_expense.BackgroundColor = Color.FromHex(App.CharcoalGray);
                }
                if (theme.Equals("light"))
                {
                    img_profile.BackgroundColor = Color.FromHex(App.DimGray2);
                    frame_commit.BackgroundColor = Color.FromHex(App.DimGray2);
                    frame_expense.BackgroundColor = Color.FromHex(App.DimGray2);
                }
            }
            else
            {
                img_profile.BackgroundColor = Color.FromHex(App.DimGray2);
                frame_commit.BackgroundColor = Color.FromHex(App.DimGray2);
                frame_expense.BackgroundColor = Color.FromHex(App.DimGray2);
            }

            img_profile.Source = ImageSource.FromStream(() =>
            {
                var stream = converter.BytesToStream(viewmodel.Picture);
                return stream;
            });
        }

        public async void DrawerTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 250);
            view.Scale = 1;
            view.IsEnabled = false;
            var drawer = (DrawerMaster)view.Parent.Parent.Parent.Parent.Parent.Parent;
            drawer.IsPresented = true;
            view.IsEnabled = true;
        }

        public async void ProfilePicClicked(object sender, EventArgs e)
        {
            if (viewmodel.Picture != null)
            {
                var view = (Frame)sender;
                await view.ScaleTo(0.9, 100);
                view.Scale = 1;
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
            await Navigation.PushAsync(new ExpensesDetail());
            view.IsEnabled = true;
        }

        public async void CommitmentColumnClicked(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;
            await view.ScaleTo(0.9, 250);
            view.Scale = 1;
            view.IsEnabled = false;
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
            await Navigation.PushPopupAsync(new AddExpenses());
            view.IsEnabled = true;
        }
    }
}