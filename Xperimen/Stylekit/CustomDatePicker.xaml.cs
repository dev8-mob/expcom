using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Model;

namespace Xperimen.Stylekit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomDatePicker : PopupPage
    {
        SimpleDateViewer viewmodel;

        public CustomDatePicker()
        {
            InitializeComponent();
            viewmodel = new SimpleDateViewer();
            BindingContext = viewmodel;

            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) stack_bg.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                if (theme.Equals("dim")) stack_bg.BackgroundColor = Color.FromHex(App.CharcoalGray);
                if (theme.Equals("light")) stack_bg.BackgroundColor = Color.FromHex(App.DimGray2);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            // Invoked when a hardware back button is pressed
            // Return true if don't want to close this popup when back button is pressed
            return true;
        }

        protected override bool OnBackgroundClicked()
        {
            // Invoked when background is clicked
            // Return false if don't want to close this popup when background of popup is clicked
            return false;
        }

        public async void DateTapped(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            var lblcode = (Label)view.Children[0];
            MessagingCenter.Send(this, "SelectedDate", lblcode.Text);
            var navigation = Application.Current.MainPage.Navigation;
            await navigation.PopPopupAsync();
            view.IsEnabled = true;
        }

        public async void CancelClicked(object sender, EventArgs e)
        {
            var view = (Label)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            var navigation = Application.Current.MainPage.Navigation;
            await navigation.PopPopupAsync();
            view.IsEnabled = true;
        }
    }

    public class DateView
    {
        public string code { get; set; }
        public string view { get; set; }
        public bool istoday { get; set; }
    }

    public class SimpleDateViewer : BaseViewModel
    {
        #region bindable
        DateTime _currentdate;
        List<DateView> _listdateview;
        public DateTime CurrentDate
        {
            get { return _currentdate; }
            set { _currentdate = value; OnPropertyChanged(); }
        }
        public List<DateView> ListDateView
        {
            get { return _listdateview; }
            set { _listdateview = value; OnPropertyChanged(); }
        }
        #endregion

        public SimpleDateViewer()
        {
            ListDateView = new List<DateView>();
            CurrentDate = DateTime.Now;

            var totalDays = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);
            for (int a = 0; a < totalDays; a++)
            {
                var istoday = false;
                var create = new DateTime(CurrentDate.Year, CurrentDate.Month, a + 1);
                if (create.Day == CurrentDate.Day) istoday = true;
                var datecode = create.ToString("dd.MM.yyyy");
                var viewstring = create.ToString("dddd, dd MMM");
                var model = new DateView { code = datecode, view = viewstring, istoday = istoday };
                ListDateView.Add(model);
            }
        }
    }
}