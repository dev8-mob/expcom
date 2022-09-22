
using Xamarin.Forms;

namespace Xperimen.View
{
    public class Logout : ContentPage
    {
        public Logout()
        {
            Application.Current.Properties.Remove("current_login");
            Application.Current.MainPage = new NavigationPage(new Login());
        }
    }
}
