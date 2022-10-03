
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xperimen.View.NavigationDrawer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawerMenuCell : ViewCell
    {
        //DrawerMenu parent;

        public DrawerMenuCell()
        {
            InitializeComponent();
        }

        public async void CellTapped(object sender, EventArgs e)
        {
            var view = (Grid)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            var title = lbl_title.Text;
            var parent = (DrawerMaster)view.Parent.Parent.Parent.Parent.Parent;
            var item = parent.viewmodel.MenuList.Where(x => x.Title.Equals(title)).ToList();
            if (item.Count() > 0)
            {
                Type page = item[0].Contentpage;
                var openPage = (Page)Activator.CreateInstance(page);
                parent.viewmodel.SetupData(title);
                MessagingCenter.Send(this, "DrawerMenuSelected");
                parent.IsPresented = false;
                parent.Detail = new NavigationPage(openPage);
            }
            view.IsEnabled = true;
        }
    }
}