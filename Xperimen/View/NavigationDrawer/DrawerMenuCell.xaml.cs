
using SQLite;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Model;
using Xperimen.View.Commitment;
using Xperimen.ViewModel.Dashboard;

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

                if (lbl_code.Text.Equals(arg))
                {
                    //set here and DrawerViewModel SetSelectedMenu
                    frame_bg.IsVisible = true;
                    lbl_title.TextColor = Color.White;
                }
            });
            MessagingCenter.Subscribe<Details>(this, "CommitmentUpdated", (sender) => 
            { 
                if (lbl_title.Text.Equals("Commitment")) 
                    GetCommitmentList();
            });
            MessagingCenter.Subscribe<AddRecord>(this, "CommitmentAdded", (sender) => 
            {
                if (lbl_title.Text.Equals("Commitment"))
                    GetCommitmentList();
            });
            MessagingCenter.Subscribe<Details>(this, "CommitmentDeleted", (sender) =>
            {
                if (lbl_title.Text.Equals("Commitment"))
                    GetCommitmentList();
            });
            MessagingCenter.Subscribe<DashboardViewmodel>(this, "CommitmentReset", (sender) =>
            {
                if (lbl_title.Text.Equals("Commitment"))
                    GetCommitmentList();
            });
        }

        public async void CellTapped(object sender, EventArgs e)
        {
            var view = (Grid)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            MessagingCenter.Send(this, "SelectedMenu", lbl_code.Text);

            var code = lbl_code.Text;
            var parent = (DrawerMenu)view.Parent.Parent.Parent.Parent;
            var drawer = (DrawerMaster)parent.Parent;
            var item = parent.viewmodel.MenuList.Where(x => x.Code.Equals(code)).ToList();
            if (item.Count() > 0)
            {
                Type page = item[0].Contentpage;
                var openPage = (Page)Activator.CreateInstance(page);
                drawer.Detail = new NavigationPage(openPage);
                await Task.Delay(250);
                drawer.IsPresented = false;
            }
            view.IsEnabled = true;
        }

        public void GetCommitmentList()
        {
            int commitment = 0;
            var userid = string.Empty;
            if (Application.Current.Properties.ContainsKey("current_login"))
                userid = Application.Current.Properties["current_login"] as string;

            try
            {
                var connection = new SQLiteConnection(App.DB_PATH);
                string query = "SELECT * FROM SelfCommitment WHERE Userid = '" + userid + "'";
                var data = connection.Query<SelfCommitment>(query).ToList();

                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        if (!item.IsDone)
                            commitment++;
                    }
                    if (commitment != 0)
                    {
                        lbl_count.Text = commitment.ToString();
                        frame_count.IsVisible = true;
                    }
                    else frame_count.IsVisible = false;
                }
                else frame_count.IsVisible = false;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
            }
        }
    }
}