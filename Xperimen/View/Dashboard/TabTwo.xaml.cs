
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.ViewModel.Dashboard;

namespace Xperimen.View.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabTwo : Frame
    {
        private readonly ChildtabViewmodel viewmodel;

        public TabTwo()
        {
            InitializeComponent();
            BindingContext = viewmodel = new ChildtabViewmodel("tab_two");
        }

        public void BindContextToParent(MaintabViewmodel parent)
        { viewmodel.SetParentBinding(parent); }

        private async void listview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var view = (ListView)sender;
            if (view.SelectedItem != null)
            {
                await Navigation.PushAsync(new DetailsPage());
                view.SelectedItem = null;
            }
        }
    }
}