using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}