using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.View.Dashboard;

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
            view.IsEnabled = false;

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
            view.IsEnabled = true;
        }

        public async void EditTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.8, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            await Navigation.PushPopupAsync(new EditExpenses(lbl_id.Text));
            view.IsEnabled = true;
        }

        public async void DeleteTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.8, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            var codes = new Dictionary<string, string>();
            codes.Add("id", lbl_id.Text);
            codes.Add("amount", lbl_amount.Text);
            MessagingCenter.Send(this, "DeleteImageTap", codes);
            view.IsEnabled = true;
        }

        public async void PicAttachmentClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            MessagingCenter.Send(this, "ExpenseImageTap", lbl_id.Text);
            view.IsEnabled = true;
        }
    }
}