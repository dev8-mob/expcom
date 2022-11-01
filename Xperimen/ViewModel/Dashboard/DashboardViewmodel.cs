using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        List<SelfCommitment> _listcommitmentsnotdone;
        List<Expenses> _listexpenses;
        bool _nocommitment;
        bool _hascommitment;
        bool _hascommitmentdoneshowbadge;
        int _commitmentnotdone;
        bool _allcommitmentdone;
        double _totalcommitment;
        bool _noexpenses;
        bool _hasexpenses;
        bool _nototalexpenses;
        bool _hastotalexpenses;
        int _expensescount;
        double _todayexpenses;
        double _totalexpenses;
        bool _exptodayhasvalue;
        bool _exptodaynovalue;
        double _diffytdtoday;
        double _percentageytdtoday;
        bool _isnotsetincome;
        bool _ishaveincome;
        double _balanceavailable;
        bool _hasbalanceavailable;
        double _percentagecommitment;
        double _percentageexpenses;
        double _percentageavailable;
        string _currency;
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
        public List<SelfCommitment> ListCommitmentsNotDone
        {
            get { return _listcommitmentsnotdone; }
            set { _listcommitmentsnotdone = value; OnPropertyChanged(); }
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
        public bool HasCommitmentDoneShowBadge
        {
            get { return _hascommitmentdoneshowbadge; }
            set { _hascommitmentdoneshowbadge = value; OnPropertyChanged(); }
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
        public double TotalCommitment
        {
            get { return _totalcommitment; }
            set { _totalcommitment = value; OnPropertyChanged(); }
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
        public bool NoTotalExpenses
        {
            get { return _nototalexpenses; }
            set { _nototalexpenses = value; OnPropertyChanged(); }
        }
        public bool HasTotalExpenses
        {
            get { return _hastotalexpenses; }
            set { _hastotalexpenses = value; OnPropertyChanged(); }
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
        public double TotalExpenses
        {
            get { return _totalexpenses; }
            set { _totalexpenses = value; OnPropertyChanged(); }
        }
        public bool ExpTodayHasValue
        {
            get { return _exptodayhasvalue; }
            set { _exptodayhasvalue = value; OnPropertyChanged(); }
        }
        public bool ExpTodayNoValue
        {
            get { return _exptodaynovalue; }
            set { _exptodaynovalue = value; OnPropertyChanged(); }
        }
        public double DiffYtdToday
        {
            get { return _diffytdtoday; }
            set { _diffytdtoday = value; OnPropertyChanged(); }
        }
        public double PercentageYtdToday
        {
            get { return _percentageytdtoday; }
            set { _percentageytdtoday = value; OnPropertyChanged(); }
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
        public double BalanceAvailable
        {
            get { return _balanceavailable; }
            set { _balanceavailable = value; OnPropertyChanged(); }
        }
        public bool HasBalanceAvailable
        {
            get { return _hasbalanceavailable; }
            set { _hasbalanceavailable = value; OnPropertyChanged(); }
        }
        public double PercentageCommitment
        {
            get { return _percentagecommitment; }
            set { _percentagecommitment = value; OnPropertyChanged(); }
        }
        public double PercentageExpenses
        {
            get { return _percentageexpenses; }
            set { _percentageexpenses = value; OnPropertyChanged(); }
        }
        public double PercentageAvailable
        {
            get { return _percentageavailable; }
            set { _percentageavailable = value; OnPropertyChanged(); }
        }
        public string Currency
        {
            get { return _currency; }
            set { _currency = value; OnPropertyChanged(); }
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
            ListCommitmentsNotDone = new List<SelfCommitment>();
            ListExpenses = new List<Expenses>();
            NoCommitment = false;
            HasCommitment = false;
            HasCommitmentDoneShowBadge = false;
            CommitmentNotDone = 0;
            AllCommitmentDone = false;
            TotalCommitment = 0;
            NoExpenses = false;
            HasExpenses = false;
            NoTotalExpenses = false;
            HasTotalExpenses = false;
            ExpensesCount = 0;
            ExpTodayHasValue = false;
            ExpTodayNoValue = false;
            TodayTotalExpenses = 0;
            TotalExpenses = 0;
            DiffYtdToday = 0;
            PercentageYtdToday = 0;
            IsNotSetIncome = false;
            IsHaveIncome = false;
            BalanceAvailable = 0;
            HasBalanceAvailable = false;
            connection = new SQLiteConnection(App.DB_PATH);
            userid = string.Empty;
            PercentageCommitment = 0;
            PercentageExpenses = 0;
            PercentageAvailable = 0;
            Currency = string.Empty;
            SetupData();
        }

        public async void SetupData()
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
            await ResetAllCommitment();
            SetupIncome();
            GetExpensesList();
            GetCommitmentList();
            CalculateAllPercentage();
            GetTodayExpenses();
            GetBalanceAvailable();
        }

        public int SetupIncome()
        {
            try
            {
                Income = 0; Currency = string.Empty;
                string getuser = "SELECT * FROM Clients WHERE Id = '" + userid + "'";
                var user = connection.Query<Clients>(getuser).ToList();
                if (user.Count > 0)
                { Income = user[0].Income; Currency = user[0].Currency; }

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

        public async Task<int> ResetAllCommitment()
        {
            try
            {
                var totalDays = DateTime.DaysInMonth(CurrentDt.Year, CurrentDt.Month);
                if (CurrentDt.Day == totalDays)
                {
                    Application.Current.Properties["firstmonth_isreset"] = "false";
                    await Application.Current.SavePropertiesAsync();
                }
                if (CurrentDt.Day == 1)
                {
                    var isreset = string.Empty;
                    if (Application.Current.Properties.ContainsKey("firstmonth_isreset"))
                        isreset = Application.Current.Properties["firstmonth_isreset"] as string;

                    if (isreset.Equals("false"))
                    {
                        var query = "SELECT * FROM SelfCommitment WHERE Userid = '" + userid + "'";
                        var result = connection.Query<SelfCommitment>(query).ToList();
                        if (result.Count > 0)
                        {
                            var update = string.Empty;
                            foreach (var data in result)
                            {
                                update = "UPDATE SelfCommitment SET IsDone = FALSE WHERE Id = '" + data.Id + "'";
                                connection.Query<SelfCommitment>(update);
                            }
                            result = connection.Query<SelfCommitment>(query).ToList();
                        }
                        MessagingCenter.Send(this, "CommitmentReset");
                        Application.Current.Properties["firstmonth_isreset"] = "true";
                        await Application.Current.SavePropertiesAsync();
                    }
                    return 1;
                }
                return 2;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 3;
            }
        }

        public int GetExpensesList()
        {
            try
            {
                TotalExpenses = 0; NoTotalExpenses = false; HasTotalExpenses = false;
                string query = "SELECT * FROM Expenses WHERE Userid = '" + userid + "'";
                var allexpenses = connection.Query<Expenses>(query).ToList();
                if (allexpenses.Count > 0)
                {
                    NoTotalExpenses = false; HasTotalExpenses = true;
                    foreach (var item in allexpenses)
                        TotalExpenses += item.Amount;
                }
                else { NoTotalExpenses = true; HasTotalExpenses = false; }
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
                TotalCommitment = 0;
                NoCommitment = false; HasCommitment = false; HasCommitmentDoneShowBadge = false;
                CommitmentNotDone = 0; AllCommitmentDone = false;
                ListCommitments = new List<SelfCommitment>(); ListCommitmentsNotDone = new List<SelfCommitment>();
                string query = "SELECT * FROM SelfCommitment WHERE Userid = '" + userid + "'";
                ListCommitments = connection.Query<SelfCommitment>(query).ToList();

                if (ListCommitments.Count > 0)
                {
                    var checkdone = 0;
                    foreach (var data in ListCommitments)
                    {
                        TotalCommitment += data.Amount;
                        if (!data.IsDone)
                        { ListCommitmentsNotDone.Add(data); CommitmentNotDone++; }
                        if (data.IsDone) checkdone++;
                    }
                    if (CommitmentNotDone > 0) 
                    { NoCommitment = false; HasCommitment = true; HasCommitmentDoneShowBadge = true; }
                    if (checkdone == ListCommitments.Count)
                    { AllCommitmentDone = true; HasCommitment = true; HasCommitmentDoneShowBadge = false; }
                }
                else
                { NoCommitment = true; HasCommitment = false; HasCommitmentDoneShowBadge = false; }

                var success = SaveNetBalance();
                if (success == 1) return 1;
                else return 2;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 2;
            }
        }

        public void CalculateAllPercentage()
        {
            var balance = Income - TotalCommitment - TotalExpenses;
            if (HasCommitment && NoTotalExpenses)
            { 
                PercentageExpenses = 0; 
                PercentageCommitment = Math.Round(TotalCommitment / Income * 100, 3); 
            }
            if (HasCommitment && HasTotalExpenses)
            {
                PercentageExpenses = Math.Round(TotalExpenses / Income * 100, 3);
                PercentageCommitment = Math.Round(TotalCommitment / Income * 100, 3);
            }
            if (NoCommitment && NoTotalExpenses) 
            { PercentageExpenses = 0; PercentageCommitment = 0; PercentageAvailable = 0; }
            if (NoCommitment && HasTotalExpenses)
            {
                PercentageExpenses = Math.Round(TotalExpenses / Income * 100, 3);
                PercentageCommitment = 0;
            }
            PercentageAvailable = Math.Round(balance / Income * 100, 3);
        }

        public int SetCommitmentDonePaid(string data, bool status)
        {
            try
            {
                string query = "UPDATE SelfCommitment SET IsDone = " + status + " WHERE Id = '" + data + "'";
                connection.Query<SelfCommitment>(query);
                MessagingCenter.Send(this, "CommitmentSetDone");
                return 1;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 2;
            }
        }

        public int SetAllCommitmentDonePaid()
        {
            try
            {
                string query = "SELECT * FROM SelfCommitment WHERE Userid = '" + userid + "'";
                var result = connection.Query<SelfCommitment>(query).ToList();
                if (result.Count > 0)
                {
                    foreach (var data in result)
                    {
                        query = "UPDATE SelfCommitment SET IsDone = " + true + " WHERE Id = '" + data.Id + "'";
                        connection.Query<SelfCommitment>(query);
                    }
                    MessagingCenter.Send(this, "CommitmentSetDone");
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

        public int GetTodayExpenses()
        {
            try
            {
                DiffYtdToday = 0;
                TodayTotalExpenses = 0; ExpensesCount = 0;
                string query = "SELECT * FROM Expenses WHERE Userid = '" + userid + "' AND ExpenseDateTime = '" + CurrentDt.ToString("dd.MM.yyyy") + "'";
                ListExpenses = connection.Query<Expenses>(query).ToList();
                if (ListExpenses.Count > 0)
                {
                    NoExpenses = false; HasExpenses = true;
                    ExpensesCount = ListExpenses.Count;
                    foreach (var data in ListExpenses) TodayTotalExpenses += data.Amount;

                    var ytd = GetYesterdayExpenses();
                    DiffYtdToday = TodayTotalExpenses - ytd;
                    if (ytd > 0) PercentageYtdToday = DiffYtdToday / ytd * 100;
                    else PercentageYtdToday = DiffYtdToday;

                    if (DiffYtdToday > 0) { ExpTodayHasValue = true; ExpTodayNoValue = false; }
                    else if (DiffYtdToday < 0) { ExpTodayHasValue = true; ExpTodayNoValue = false; }
                    else if (DiffYtdToday == 0) { ExpTodayHasValue = false; ExpTodayNoValue = true; }
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

        public int DeleteTodayExpenses(string data)
        {
            try
            {
                string query = "DELETE FROM Expenses WHERE Id = '" + data + "'";
                connection.Query<Expenses>(query);
                MessagingCenter.Send(this, "ExpensesDeleted");
                return 1;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 2;
            }
        }

        public double GetYesterdayExpenses()
        {
            try
            {
                string query = "SELECT * FROM Expenses WHERE Userid = '" + userid + "' AND ExpenseDateTime = '" + CurrentDt.AddDays(-1).ToString("dd.MM.yyyy") + "'";
                var ytdexp = connection.Query<Expenses>(query).ToList();
                if (ytdexp.Count > 0)
                {
                    double totalytd = 0;
                    foreach (var data in ytdexp) totalytd += data.Amount;
                    return totalytd;
                }
                else return 0;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 0;
            }
        }

        public void GetBalanceAvailable()
        {
            HasBalanceAvailable = false;
            BalanceAvailable = Income - TotalCommitment - TotalExpenses;
            if (BalanceAvailable != 0) HasBalanceAvailable = true;
        }

        public int SaveNetBalance()
        {
            try
            {
                var query = "UPDATE Clients SET NetIncome = " + (Income - TotalCommitment) + ", TotalCommitment = " + TotalCommitment + " WHERE Id = '" + userid + "'";
                connection.Query<Clients>(query);
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
