using SQLite;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Model;

namespace Xperimen.Stylekit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomCalendar : AbsoluteLayout
    {
        public DateTime CurrentDt;
        public SQLiteConnection connection;

        public CustomCalendar()
        {
            InitializeComponent();
            CurrentDt = DateTime.Now;
            connection = new SQLiteConnection(App.DB_PATH);
            SetupView();
        }

        public void SetupView()
        {
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark"))
                {
                    grid_header.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                    grid_calendar.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                }
                if (theme.Equals("dim"))
                {
                    grid_header.BackgroundColor = Color.FromHex(App.CharcoalGray);
                    grid_calendar.BackgroundColor = Color.FromHex(App.CharcoalGray);
                }
                if (theme.Equals("light"))
                {
                    grid_header.BackgroundColor = Color.FromHex(App.DimGray2);
                    grid_calendar.BackgroundColor = Color.FromHex(App.DimGray2);
                }
            }

            lbl_month.Text = string.Format("{0:MMM, yyyy}", CurrentDt);
            var totalDays = DateTime.DaysInMonth(CurrentDt.Year, CurrentDt.Month);
            var firstday = new DateTime(CurrentDt.Year, CurrentDt.Month, 1);

            int startcol = 0;
            if (firstday.DayOfWeek.ToString().Equals("Tuesday")) startcol = 1;
            if (firstday.DayOfWeek.ToString().Equals("Wednesday")) startcol = 2;
            if (firstday.DayOfWeek.ToString().Equals("Thursday")) startcol = 3;
            if (firstday.DayOfWeek.ToString().Equals("Friday")) startcol = 4;
            if (firstday.DayOfWeek.ToString().Equals("Saturday")) startcol = 5;
            if (firstday.DayOfWeek.ToString().Equals("Sunday")) startcol = 6;

            int daysno = 1, row = 0, col = startcol;
            grid_calendar.Children.Clear();
            grid_calendar.RowDefinitions.Clear();
            var newrow = new RowDefinition { Height = GridLength.Auto };
            grid_calendar.RowDefinitions.Add(newrow);
            for (int i = 0; i < totalDays; i++)
            {
                #region create UI
                var day = new DateTime(CurrentDt.Year, CurrentDt.Month, daysno);
                var fontfamily = string.Empty;
                if (Device.RuntimePlatform == Device.Android) fontfamily = "Ubuntu-Regular.ttf#Ubuntu Regular";
                else if (Device.RuntimePlatform == Device.iOS) fontfamily = "Ubuntu-Regular.ttf";

                var count = GetTotalExpenses(day.ToString("dd.MM.yyyy"));
                var stack = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Spacing = 0
                };
                var stackbadge = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Spacing = 0
                };
                var frame = new Frame
                {
                    Padding = 0,
                    HasShadow = false,
                    CornerRadius = 25,
                    HeightRequest = 25,
                    WidthRequest = 25,
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Start,
                    BackgroundColor = Color.FromHex(App.CustomRed)
                };
                var lblday = new XLabel
                {
                    Text = daysno.ToString(),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(5, 15, 5, 15),
                    FontFamily = fontfamily
                };
                var lblcode = new Label { Text = day.ToString("dd.MM.yyyy"), IsVisible = false };
                #endregion

                if (col < 7)
                {
                    if (count > 0)
                    {
                        if (Device.RuntimePlatform == Device.Android) fontfamily = "Ubuntu-Bold.ttf#Ubuntu Bold";
                        else if (Device.RuntimePlatform == Device.iOS) fontfamily = "Ubuntu-Bold.ttf";

                        lblday.FontFamily = fontfamily;
                        stack.Children.Add(lblday);
                        stack.Children.Add(lblcode);

                        var lblbadge = new Label
                        {
                            Text = count.ToString(),
                            TextColor = Color.White,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            FontFamily = fontfamily
                        };
                        stackbadge.Children.Add(lblbadge);
                        stackbadge.Children.Add(lblcode);
                        frame.Content = stackbadge;

                        var tapframe = new TapGestureRecognizer();
                        tapframe.Tapped += DayBadgeTapped;
                        tapframe.NumberOfTapsRequired = 1;
                        frame.GestureRecognizers.Add(tapframe);

                        grid_calendar.Children.Add(stack, col, row);
                        grid_calendar.Children.Add(frame, col, row);
                    }
                    else
                    {
                        stack.Children.Add(lblday);
                        stack.Children.Add(lblcode);

                        var tap = new TapGestureRecognizer();
                        tap.Tapped += DayTapped;
                        tap.NumberOfTapsRequired = 1;
                        stack.GestureRecognizers.Add(tap);

                        grid_calendar.Children.Add(stack, col, row);
                    }
                    col++;
                }
                else
                {
                    row++; col = 0;
                    newrow = new RowDefinition { Height = GridLength.Auto };
                    grid_calendar.RowDefinitions.Add(newrow);

                    if (count > 0)
                    {
                        if (Device.RuntimePlatform == Device.Android) fontfamily = "Ubuntu-Bold.ttf#Ubuntu Bold";
                        else if (Device.RuntimePlatform == Device.iOS) fontfamily = "Ubuntu-Bold.ttf";

                        lblday.FontFamily = fontfamily; 
                        stack.Children.Add(lblday);
                        stack.Children.Add(lblcode);

                        var lblbadge = new Label
                        {
                            Text = count.ToString(),
                            TextColor = Color.White,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            FontFamily = fontfamily
                        };
                        stackbadge.Children.Add(lblbadge);
                        stackbadge.Children.Add(lblcode);

                        var tapframe = new TapGestureRecognizer();
                        tapframe.Tapped += DayBadgeTapped;
                        tapframe.NumberOfTapsRequired = 1;
                        frame.Content = stackbadge;
                        frame.GestureRecognizers.Add(tapframe);
                        grid_calendar.Children.Add(stack, col, row);
                        grid_calendar.Children.Add(frame, col, row);
                    }
                    else
                    {
                        stack.Children.Add(lblday);
                        stack.Children.Add(lblcode);

                        var tap = new TapGestureRecognizer();
                        tap.Tapped += DayTapped;
                        tap.NumberOfTapsRequired = 1;
                        stack.GestureRecognizers.Add(tap);

                        grid_calendar.Children.Add(stack, col, row);
                    }
                    col++;
                }
                daysno++;
            }
        }

        public async void DayTapped(object sender, EventArgs arg)
        {
            try
            {
                var view = (StackLayout)sender;
                await view.ScaleTo(0.8, 100);
                view.Scale = 1;
                view.IsEnabled = false;
                var lblcode = (Label)view.Children[1];
                MessagingCenter.Send(this, "CalendarDateTap", lblcode.Text);
                view.IsEnabled = true;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
            }
        }

        public async void DayBadgeTapped(object sender, EventArgs arg)
        {
            try
            {
                var view = (Frame)sender;
                await view.ScaleTo(0.8, 100);
                view.Scale = 1;
                view.IsEnabled = false;
                var stack = (StackLayout)view.Content;
                var lblcode = (Label)stack.Children[1];
                MessagingCenter.Send(this, "CalendarDateTap", lblcode.Text);
                view.IsEnabled = true;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
            }
        }

        public int GetTotalExpenses(string day)
        {
            var userid = string.Empty;
            if (Application.Current.Properties.ContainsKey("current_login"))
                userid = Application.Current.Properties["current_login"] as string;

            var query = "SELECT * FROM Expenses WHERE ExpenseDateTime = '" + day + "' AND Userid = '" + userid + "'";
            var result = connection.Query<Expenses>(query).ToList();
            return result.Count;
        }
    }
}