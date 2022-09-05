
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.ViewModel.Dashboard;

namespace Xperimen.View.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabOne : Frame
    {
        private readonly ChildtabViewmodel viewmodel;

        public TabOne()
        {
            InitializeComponent();
            BindingContext = viewmodel = new ChildtabViewmodel("tab_one");
        }

        public void BindContextToParent(MaintabViewmodel parent)
        { viewmodel.SetParentBinding(parent); }
    }
}