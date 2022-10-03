
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xperimen.View.NavigationDrawer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawerMenuCell : ViewCell
    {
        public DrawerMenuCell()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<DrawerMenuCell, string>(this, "SelectedMenu", (sender, arg) =>
            {
                frame_bg.IsVisible = false;
                var theme = string.Empty;
                if (Application.Current.Properties.ContainsKey("app_theme"))
                {
                    theme = Application.Current.Properties["app_theme"] as string;
                    if (theme.Equals("dark")) lbl_title.TextColor = Color.White;
                    if (theme.Equals("dim")) lbl_title.TextColor = Color.Black;
                    if (theme.Equals("light")) lbl_title.TextColor = (Color)Application.Current.Resources["LabelTextColor"];
                }

                if (lbl_title.Text.Equals(arg))
                {
                    //set here and DrawerViewModel SetSelectedMenu
                    frame_bg.IsVisible = true;
                    lbl_title.TextColor = Color.White;
                }
            });
        }

        public async void CellTapped(object sender, EventArgs e)
        {
            var view = (Grid)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            MessagingCenter.Send(this, "SelectedMenu", lbl_title.Text);

            var title = lbl_title.Text;
            var parent = (DrawerMenu)view.Parent.Parent.Parent.Parent;
            var drawer = (DrawerMaster)parent.Parent;
            var item = parent.viewmodel.MenuList.Where(x => x.Title.Equals(title)).ToList();
            if (item.Count() > 0)
            {
                Type page = item[0].Contentpage;
                var openPage = (Page)Activator.CreateInstance(page);
                drawer.Detail = new NavigationPage(openPage);
                await Task.Delay(300);
                drawer.IsPresented = false;
            }
            view.IsEnabled = true;
        }
    }
}