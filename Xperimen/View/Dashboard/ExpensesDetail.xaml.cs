using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
using Xperimen.View.NavigationDrawer;
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
    }
}