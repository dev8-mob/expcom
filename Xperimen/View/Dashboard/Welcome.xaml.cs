using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
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
        }

        public void SetupView()
        {
            img_profile.Source = ImageSource.FromStream(() =>
            {
                var stream = converter.BytesToStream(viewmodel.Picture);
                return stream;
            });
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
    }
}