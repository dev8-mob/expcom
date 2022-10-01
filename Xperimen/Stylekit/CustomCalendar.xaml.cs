using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                if (theme.Equals("dark")) grid_calendar.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                if (theme.Equals("dim")) grid_calendar.BackgroundColor = Color.FromHex(App.CharcoalGray);
                if (theme.Equals("light")) grid_calendar.BackgroundColor = Color.FromHex(App.DimGray2);
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

            int daysno = 1, row = 2, col = startcol;
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
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
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
                    Margin = new Thickness(0,8,0,8),
                    FontFamily = fontfamily
                };

                var tap = new TapGestureRecognizer();
                tap.Tapped += DayTapped;
                tap.NumberOfTapsRequired = 1;
                stack.GestureRecognizers.Add(tap);
                var tapframe = new TapGestureRecognizer();
                tapframe.Tapped += DayBadgeTapped;
                tapframe.NumberOfTapsRequired = 1;
                frame.GestureRecognizers.Add(tapframe);
                #endregion

                if (col < 7)
                {
                    if (count > 0)
                    {
                        if (Device.RuntimePlatform == Device.Android) fontfamily = "Ubuntu-Bold.ttf#Ubuntu Bold";
                        else if (Device.RuntimePlatform == Device.iOS) fontfamily = "Ubuntu-Bold.ttf";
                        lblday.FontFamily = fontfamily; lblday.Margin = 0;
                        lblday.HorizontalOptions = LayoutOptions.Start;
                        lblday.VerticalOptions = LayoutOptions.End;
                        lblday.VerticalTextAlignment = TextAlignment.End;
                        stack.HorizontalOptions = LayoutOptions.StartAndExpand;
                        stack.VerticalOptions = LayoutOptions.EndAndExpand;

                        var lblbadge = new Label
                        {
                            Text = count.ToString(),
                            TextColor = Color.White,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            FontFamily = fontfamily
                        };
                        frame.Content = lblbadge;
                        stack.Children.Add(lblday);
                        grid_calendar.Children.Add(stack, col, row);
                        grid_calendar.Children.Add(frame, col, row);
                    }
                    else
                    {
                        stack.Children.Add(lblday);
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
                        lblday.FontFamily = fontfamily; lblday.Margin = 0;
                        lblday.HorizontalOptions = LayoutOptions.Start;
                        lblday.VerticalOptions = LayoutOptions.End;
                        lblday.VerticalTextAlignment = TextAlignment.End;
                        stack.HorizontalOptions = LayoutOptions.StartAndExpand;
                        stack.VerticalOptions = LayoutOptions.EndAndExpand;

                        var lblbadge = new Label
                        {
                            Text = count.ToString(),
                            TextColor = Color.White,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            FontFamily = fontfamily
                        };
                        frame.Content = lblbadge;
                        stack.Children.Add(lblday);
                        grid_calendar.Children.Add(stack, col, row);
                        grid_calendar.Children.Add(frame, col, row);
                    }
                    else
                    {
                        stack.Children.Add(lblday);
                        grid_calendar.Children.Add(stack, col, row);
                    }
                    col++;
                }
                daysno++;
            }
        }

        public async void DayTapped(object sender, EventArgs arg)
        {
            var view = (StackLayout)sender;
            await view.ScaleTo(0.8, 100);
            view.Scale = 1;
            MessagingCenter.Send(this, "TestCalendarTap");
        }

        public async void DayBadgeTapped(object sender, EventArgs arg)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.8, 100);
            view.Scale = 1;
            MessagingCenter.Send(this, "TestCalendarTap");
        }

        public int GetTotalExpenses(string day)
        {
            var query = "SELECT * FROM Expenses WHERE ExpenseDateTime = '" + day + "'";
            var result = connection.Query<Expenses>(query).ToList();
            return result.Count;
        }
    }
}