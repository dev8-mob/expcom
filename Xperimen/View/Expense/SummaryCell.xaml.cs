using SQLite;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Model;
using Xperimen.Resources;

namespace Xperimen.View.Expense
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SummaryCell : StackLayout
    {
        public SQLiteConnection connection;
        DateTime currentDt;

        public SummaryCell()
        {
            InitializeComponent();
            connection = new SQLiteConnection(App.DB_PATH);
            SetupView();
        }

        public void SetupView()
        {
            currentDt = DateTime.Now;
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) BackgroundColor = Color.FromHex(App.CharcoalBlack);
                if (theme.Equals("dim")) BackgroundColor = Color.FromHex(App.CharcoalGray);
                if (theme.Equals("light")) BackgroundColor = Color.FromHex(App.DimGray2);
            }
            lbl_month.Text = "(" + currentDt.ToString("MMM, yyyy") + ")";

            try
            {
                var userid = string.Empty;
                if (Application.Current.Properties.ContainsKey("current_login"))
                    userid = Application.Current.Properties["current_login"] as string;

                string query = "SELECT * FROM SelfCommitment WHERE Userid = '" + userid + "'";
                var result = connection.Query<SelfCommitment>(query).ToList();
                lbl_commitment.Text = "-" + result.Count + " " + AppResources.exp_minuscomm;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
            }
        }
    }
}