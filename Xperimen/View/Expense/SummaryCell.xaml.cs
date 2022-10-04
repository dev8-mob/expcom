using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xperimen.View.Expense
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SummaryCell : StackLayout
    {
        DateTime currentDt;

        public SummaryCell()
        {
            InitializeComponent();
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
        }
    }
}