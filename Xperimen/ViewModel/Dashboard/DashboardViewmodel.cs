using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xperimen.Model;

namespace Xperimen.ViewModel.Dashboard
{
    public class DashboardViewmodel : BaseViewModel
    {
        #region properties
        string _firstname;
        string _lastname;
        byte[] _picture;
        DateTime _currentdt;
        double _income;
        List<SelfCommitment> _listcommitments;
        List<Expenses> _listexpenses;
        bool _nocommitment;
        bool _hascommitment;
        bool _hascommitmentdonenohide;
        int _commitmentnotdone;
        bool _allcommitmentdone;
        bool _noexpenses;
        bool _hasexpenses;
        int _expensescount;
        double _todayexpenses;
        bool _isnotsetincome;
        bool _ishaveincome;
        public string Firstname
        {
            get { return _firstname; }
            set { _firstname = value; OnPropertyChanged(); }
        }
        public string Lastname
        {
            get { return _lastname; }
            set { _lastname = value; OnPropertyChanged(); }
        }
        public byte[] Picture
        {
            get { return _picture; }
            set { _picture = value; OnPropertyChanged(); }
        }
        public DateTime CurrentDt
        {
            get { return _currentdt; }
            set { _currentdt = value; OnPropertyChanged(); }
        }
        public double Income
        {
            get { return _income; }
            set { _income = value; OnPropertyChanged(); }
        }
        public List<SelfCommitment> ListCommitments
        {
            get { return _listcommitments; }
            set { _listcommitments = value; OnPropertyChanged(); }
        }
        public List<Expenses> ListExpenses
        {
            get { return _listexpenses; }
            set { _listexpenses = value; OnPropertyChanged(); }
        }
        public bool NoCommitment
        {
            get { return _nocommitment; }
            set { _nocommitment = value; OnPropertyChanged(); }
        }
        public bool HasCommitment
        {
            get { return _hascommitment; }
            set { _hascommitment = value; OnPropertyChanged(); }
        }
        public bool HasCommitmentDoneNoHide
        {
            get { return _hascommitmentdonenohide; }
            set { _hascommitmentdonenohide = value; OnPropertyChanged(); }
        }
        public int CommitmentNotDone
        {
            get { return _commitmentnotdone; }
            set { _commitmentnotdone = value; OnPropertyChanged(); }
        }
        public bool AllCommitmentDone
        {
            get { return _allcommitmentdone; }
            set { _allcommitmentdone = value; OnPropertyChanged(); }
        }
        public bool NoExpenses
        {
            get { return _noexpenses; }
            set { _noexpenses = value; OnPropertyChanged(); }
        }
        public bool HasExpenses
        {
            get { return _hasexpenses; }
            set { _hasexpenses = value; OnPropertyChanged(); }
        }
        public int ExpensesCount
        {
            get { return _expensescount; }
            set { _expensescount = value; OnPropertyChanged(); }
        }
        public double TodayTotalExpenses
        {
            get { return _todayexpenses; }
            set { _todayexpenses = value; OnPropertyChanged(); }
        }
        public bool IsNotSetIncome
        {
            get { return _isnotsetincome; }
            set { _isnotsetincome = value; OnPropertyChanged(); }
        }
        public bool IsHaveIncome
        {
            get { return _ishaveincome; }
            set { _ishaveincome = value; OnPropertyChanged(); }
        }
        #endregion

        public SQLiteConnection connection;
        public string userid;

        public DashboardViewmodel()
        {
            Firstname = string.Empty;
            Lastname = string.Empty;
            Picture = null;
            CurrentDt = DateTime.Now;
            Income = 0;
            ListCommitments = new List<SelfCommitment>();
            ListExpenses = new List<Expenses>();
            NoCommitment = false;
            HasCommitment = false;
            HasCommitmentDoneNoHide = false;
            CommitmentNotDone = 0;
            AllCommitmentDone = false;
            NoExpenses = false;
            HasExpenses = false;
            ExpensesCount = 0;
            TodayTotalExpenses = 0;
            IsNotSetIncome = false;
            IsHaveIncome = false;
            connection = new SQLiteConnection(App.DB_PATH);
            userid = string.Empty;
            SetupData();
        }

        public void SetupData()
        {
            if (Application.Current.Properties.ContainsKey("current_login"))
            {
                userid = Application.Current.Properties["current_login"] as string;
                string query = "SELECT * FROM Clients WHERE Id = '" + userid + "'";
                var user = connection.Query<Clients>(query).ToList();
                if (user.Count > 0)
                {
                    Firstname = user[0].Firstname;
                    Lastname = user[0].Lastname;
                    Picture = user[0].ProfileImage;
                }
            }
            GetCommitmentList();
            GetTodayExpenses();
        }

        public int SetupIncome()
        {
            try
            {
                Income = 0;
                string getuser = "SELECT * FROM Clients WHERE Id = '" + userid + "'";
                var user = connection.Query<Clients>(getuser).ToList();
                if (user.Count > 0) Income = user[0].Income;

                if (Income == 0) { IsNotSetIncome = true; IsHaveIncome = false; }
                else { IsNotSetIncome = false; IsHaveIncome = true; }
                return 1;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 2;
            }
        }

        public int GetCommitmentList()
        {
            try
            {
                NoCommitment = false; HasCommitment = false; HasCommitmentDoneNoHide = false;
                CommitmentNotDone = 0; AllCommitmentDone = false;
                ListCommitments = new List<SelfCommitment>();
                string query = "SELECT * FROM SelfCommitment WHERE Userid = '" + userid + "'";
                ListCommitments = connection.Query<SelfCommitment>(query).ToList();

                if (ListCommitments.Count > 0)
                {
                    var checkdone = 0;
                    foreach (var data in ListCommitments)
                    {
                        if (!data.IsDone) CommitmentNotDone++;
                        if (data.IsDone) checkdone++;
                    }
                    if (CommitmentNotDone > 0) 
                    { NoCommitment = false; HasCommitment = true; HasCommitmentDoneNoHide = true; }
                    if (checkdone == ListCommitments.Count)
                    { AllCommitmentDone = true; HasCommitment = true; HasCommitmentDoneNoHide = false; }
                }
                else
                { NoCommitment = true; HasCommitment = false; HasCommitmentDoneNoHide = false; }
                return 1;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 2;
            }
        }

        public int GetTodayExpenses()
        {
            try
            {
                TodayTotalExpenses = 0; ExpensesCount = 0;
                string query = "SELECT * FROM Expenses WHERE Userid = '" + userid + "' AND ExpenseDateTime = '" + CurrentDt.ToString("dd.MM.yyyy") + "'";
                ListExpenses = connection.Query<Expenses>(query).ToList();
                if (ListExpenses.Count > 0)
                {
                    NoExpenses = false; HasExpenses = true;
                    ExpensesCount = ListExpenses.Count;
                    foreach (var data in ListExpenses) TodayTotalExpenses += data.Amount;
                    return 1;
                }
                else
                {
                    NoExpenses = true; HasExpenses = false;
                    return 2;
                }
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
