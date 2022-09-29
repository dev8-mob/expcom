using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.View.NavigationDrawer;
using Xperimen.ViewModel.Dashboard;

namespace Xperimen.View.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminPage : ContentPage
    {
        public readonly MaintabViewmodel viewmodel;

        public AdminPage()
        {
            InitializeComponent();
            viewmodel = new MaintabViewmodel();
            BindingContext = viewmodel;
            tab_one.BindContextToParent(viewmodel);
            tab_two.BindContextToParent(viewmodel);
        }

        public async void OnTabTapped(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;

            var tab = view.ClassId;
            if (tab.Equals("tab_one"))
            {
                //frame_one.BackgroundColor = Color.White;
                //frame_two.BackgroundColor = Color.Transparent;
                line1.IsVisible = true;
                line2.IsVisible = false;
                tab_one.IsVisible = true;
                tab_two.IsVisible = false;
            }
            else if (tab.Equals("tab_two"))
            {
                //frame_one.BackgroundColor = Color.Transparent;
                //frame_two.BackgroundColor = Color.White;
                line1.IsVisible = false;
                line2.IsVisible = true;
                tab_one.IsVisible = false;
                tab_two.IsVisible = true;
            }
            viewmodel.SetSelectedTab(tab);
        }

        public async void DrawerTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            var drawer = (DrawerMaster)view.Parent.Parent.Parent.Parent.Parent.Parent;
            drawer.IsPresented = true;
        }
    }
}