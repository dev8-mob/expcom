using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.ViewModel.Dashboard;

namespace Xperimen.View.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedDashboard : ContentPage
    {
        private readonly MaintabViewmodel viewmodel;

        public TabbedDashboard()
        {
            InitializeComponent();
            BindingContext = viewmodel = new MaintabViewmodel();
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
                stack_one.BackgroundColor = Color.White;
                stack_two.BackgroundColor = Color.Transparent;
                line1.IsVisible = true;
                line2.IsVisible = false;
                tab_one.IsVisible = true;
                tab_two.IsVisible = false;
            }
            else if (tab.Equals("tab_two"))
            {
                stack_one.BackgroundColor = Color.Transparent;
                stack_two.BackgroundColor = Color.White;
                line1.IsVisible = false;
                line2.IsVisible = true;
                tab_one.IsVisible = false;
                tab_two.IsVisible = true;
            }
            viewmodel.SetSelectedTab(tab);
        }
    }
}