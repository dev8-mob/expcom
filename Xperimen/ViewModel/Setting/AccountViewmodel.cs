using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xperimen.Model;
using Xperimen.View;

namespace Xperimen.ViewModel.Setting
{
    public class AccountViewmodel : BaseViewModel
    {
        #region bindable properties
        ObservableCollection<Clients> _listclients;
        public ObservableCollection<Clients> ListClients
        {
            get => _listclients;
            set { SetProperty(ref _listclients, value); }
        }
        #endregion

        public SQLiteConnection connection;

        public AccountViewmodel()
        {
            ListClients = new ObservableCollection<Clients>();
            connection = new SQLiteConnection(App.DB_PATH);
            GetClientsList();
        }

        public int GetClientsList()
        {
            try
            {
                var users = connection.Table<Clients>().ToList();
                ListClients = new ObservableCollection<Clients>(users);
                return 1;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 2;
            }
        }

        public int DeleteUser(string userid)
        {
            try
            {
                var queryUser = "DELETE FROM Clients WHERE Id = '" + userid + "'";
                var queryCommitment = "DELETE FROM SelfCommitment WHERE Userid = '" + userid + "'";
                var queryExpense = "DELETE FROM Expenses WHERE Userid = '" + userid + "'";

                connection.Query<Clients>(queryUser);
                connection.Query<SelfCommitment>(queryCommitment);
                connection.Query<Expenses>(queryCommitment);
                return 1;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 2;
            }
        }

        public async Task<int> DeleteMyAccount()
        {
            try
            {
                if (Application.Current.Properties.ContainsKey("current_login"))
                {
                    var userid = Application.Current.Properties["current_login"] as string;
                    var queryUser = "DELETE FROM Clients WHERE Id = '" + userid + "'";
                    var queryCommitment = "DELETE FROM SelfCommitment WHERE Userid = '" + userid + "'";
                    var queryExpense = "DELETE FROM Expenses WHERE Userid = '" + userid + "'";
                    connection.Query<Clients>(queryUser);
                    connection.Query<SelfCommitment>(queryCommitment);
                    connection.Query<Expenses>(queryExpense);

                    Application.Current.Properties.Remove("current_login");
                    await Application.Current.SavePropertiesAsync();
                    return 1;
                }
                else return 2;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 3;
            }
        }
    }
}
