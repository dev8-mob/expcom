
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Stylekit;
using Xperimen.ViewModel.Dashboard;

namespace Xperimen.View.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserList : Frame
    {
        public readonly ChildtabViewmodel viewmodel;

        public UserList()
        {
            InitializeComponent();
            BindingContext = viewmodel = new ChildtabViewmodel("tab_one");

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", (sender, arg) =>
            {
                if (arg.Equals("Okay"))
                {
                    var result = viewmodel.DeleteUser(alert.CodeObject);
                    if (result == 1)
                    {
                        SetDisplayAlert("Success", "User deleted.", "", "", "");
                        viewmodel.IsBlocked = false;
                        viewmodel.IsLoading = false;
                        viewmodel.LoadData();
                    }
                    else if (result == 2) SetDisplayAlert("Failed", "Technical error. Failed to delete the user.", "", "", "");
                }
                viewmodel.IsBlocked = false;
                viewmodel.IsLoading = false;
            });
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

        public void SetDisplayAlert(string title, string description, string btn1, string btn2, string obj)
        {
            //if string1 empty will not display btn1, if string2 empty will not display btn2
            //if both string1 & string2 empty will not display all buttons
            //all buttons tapped will send 'DisplayAlertSelection' with text of the button
            //close button tapped will send 'DisplayAlertSelection' with empty text
            alert.Title = title;
            alert.Description = description;
            alert.TxtBtn1 = btn1;
            alert.TxtBtn2 = btn2;
            alert.IsVisible = true;
            alert.CodeObject = obj;
        }
    }
}