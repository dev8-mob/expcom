using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xperimen.Model;

namespace Xperimen.ViewModel.Setting
{
    public class LanguageViewmodel : BaseViewModel
    {
        #region bindable properties
        public string _language;
        ObservableCollection<Language> _listlanguage;
        public string Language
        {
            get => _language;
            set { SetProperty(ref _language, value); }
        }
        public ObservableCollection<Language> ListLanguage
        {
            get => _listlanguage;
            set { SetProperty(ref _listlanguage, value); }
        }
        #endregion

        public SQLiteConnection connection;

        public LanguageViewmodel()
        {
            Language = string.Empty;
            ListLanguage = new ObservableCollection<Language>();
            connection = new SQLiteConnection(App.DB_PATH);
            SetupData();
        }

        public void SetupData()
        {
            var userid = Application.Current.Properties["current_login"] as string;
            string query = "SELECT * FROM Clients WHERE Id = '" + userid + "'";
            var result = connection.Query<Clients>(query).ToList();
            if (result.Count > 0) Language = result[0].Language;

            ListLanguage.Add(new Language { Code = "ms", Name = "Bahasa Malaysia", IsSelected = false });
            ListLanguage.Add(new Language { Code = "en", Name = "English", IsSelected = false });
            ListLanguage.Add(new Language { Code = "fil", Name = "Filipino", IsSelected = false });
            ListLanguage.Add(new Language { Code = "hi", Name = "Hindi", IsSelected = false });
            ListLanguage.Add(new Language { Code = "ta", Name = "Tamil", IsSelected = false });

            foreach (var item in ListLanguage)
            {
                if (item.Code.Equals(Language))
                    item.IsSelected = true;
            }
        }

        public int UpdateLanguage()
        {
            try
            {
                var userid = Application.Current.Properties["current_login"] as string;
                var query = "UPDATE Clients SET Language = '" + Language + "' WHERE Id = '" + userid + "'";
                connection.Query<Clients>(query);
                var cek = connection.Query<Clients>("SELECT * FROM Clients WHERE Id = '" + userid + "'").ToList();
                return 1;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 2;
            }
        }
    }
}
