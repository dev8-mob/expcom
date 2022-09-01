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
            viewmodel.SetSelectedTab(view.ClassId);
        }
    }
}