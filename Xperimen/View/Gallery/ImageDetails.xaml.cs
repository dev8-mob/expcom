using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
using Xperimen.Model;

namespace Xperimen.View.Gallery
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageDetails : PopupPage
    {
        public StreamByteConverter converter;
        public SelfCommitment comm;
        public Expenses exp;

        public ImageDetails(Expenses exp, SelfCommitment comm)
        {
            InitializeComponent();
            converter = new StreamByteConverter();
            this.exp = exp;
            this.comm = comm;
            SetupView();
        }

        public void SetupView()
        {
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark"))
                {
                    frame_exp.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                    frame_comm.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                }
                if (theme.Equals("dim"))
                {
                    frame_exp.BackgroundColor = Color.FromHex(App.CharcoalGray);
                    frame_comm.BackgroundColor = Color.FromHex(App.CharcoalGray);
                }
                if (theme.Equals("light"))
                {
                    frame_exp.BackgroundColor = Color.FromHex(App.DimGray2);
                    frame_comm.BackgroundColor = Color.FromHex(App.DimGray2);
                }
            }
            if (exp != null)
            {
                frame_exp.IsVisible = true;
                lbl_exptitle.Text = exp.Title;
                lbl_expamount.Text = exp.Currency + " " + string.Format("{0:0.00}", exp.Amount);
                lbl_expdt.Text = string.Format("{0:d, MMM yyyy - h:mm tt}", exp.ExpensesDt);
                img_data.Source = ImageSource.FromStream(() =>
                {
                    var stream = converter.BytesToStream(exp.Picture);
                    return stream;
                });
            }
            if (comm != null)
            {
                frame_comm.IsVisible = true;
                lbl_commtitle.Text = comm.Title;
                lbl_commdesc.Text = comm.Description;
                lbl_commamount.Text = comm.Currency + " " + string.Format("{0:0.00}", comm.Amount);
                if (comm.HasAccNo) { lbl_commaccno.Text = string.Format("Acc. No : {0}", comm.AccountNo); lbl_commaccno.IsVisible = true; }
                if (comm.IsDone) stack_isdone.IsVisible = true;
                img_data.Source = ImageSource.FromStream(() =>
                {
                    var stream = converter.BytesToStream(comm.Picture);
                    return stream;
                });
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

        public async void CloseTapped(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            var navigation = Application.Current.MainPage.Navigation;
            await navigation.PopPopupAsync();
        }
    }
}