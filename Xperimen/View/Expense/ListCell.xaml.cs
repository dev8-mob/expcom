using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xperimen.View.Expense
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListCell : StackLayout
    {
        public ListCell()
        {
            InitializeComponent();

            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) BackgroundColor = Color.FromHex(App.CharcoalBlack);
                if (theme.Equals("dim")) BackgroundColor = Color.FromHex(App.CharcoalGray);
                if (theme.Equals("light")) BackgroundColor = Color.FromHex(App.DimGray2);
            }
        }

        public async void HeaderTapped(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;

            if (!stack_details.IsVisible)
            {
                if (Application.Current.Properties.ContainsKey("app_theme"))
                {
                    var theme = Application.Current.Properties["app_theme"] as string;
                    if (theme.Equals("dark")) img_arrow.Source = "white_up.png";
                    else img_arrow.Source = "black_up.png";
                }
                stack_details.IsVisible = true;
            }
            else if (stack_details.IsVisible)
            {
                if (Application.Current.Properties.ContainsKey("app_theme"))
                {
                    var theme = Application.Current.Properties["app_theme"] as string;
                    if (theme.Equals("dark")) img_arrow.Source = img_arrow.Source2;
                    else img_arrow.Source = img_arrow.Source1;
                }
                stack_details.IsVisible = false;
            }
        }

        public async void PicAttachmentClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
        }
    }
}