using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
using Xperimen.Stylekit;
using Xperimen.ViewModel.Expense;

namespace Xperimen.View.Expense
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditExpenses : PopupPage
    {
        public EditexpViewmodel viewmodel;

        public EditExpenses(string id)
        {
            InitializeComponent();
            viewmodel = new EditexpViewmodel(id);
            BindingContext = viewmodel;
            SetupView();

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", async (sender, arg) =>
            {
                viewmodel.IsLoading = false;
                if (alert.CodeObject.Equals("success"))
                {
                    var navigation = Application.Current.MainPage.Navigation;
                    await navigation.PopPopupAsync();
                }
            });
        }

        public void SetupView()
        {
            lbl_currentdt.Text = viewmodel.ExpensesDt.ToString("d.MM.yyyy - h:mm tt");
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

        public async void GalleryClicked(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            var result = await viewmodel.PickPhoto();
            if (result == 5) SetDisplayAlert("Permission", "App required permission to access media gallery to pick photos.", "", "", "");
            if (result == 4) SetDisplayAlert("Permission", "App required permission to access photos to pick photos.", "", "", "");
            if (result == 3) SetDisplayAlert("Unavailable", "Photo gallery is not available to pick photo.", "", "", "");
            else if (result == 2) SetDisplayAlert("Alert", "No photo selected.", "", "", "");
            else if (result == 1)
            {
                var picpath = viewmodel.PictureMedia.Path.Split('/');
                var name = picpath[picpath.Length - 1];
                lbl_attach.Text = name;
                viewmodel.HasAttachment = true;
                viewmodel.IsLoading = false;
            }
            view.IsEnabled = true;
        }

        public async void CameraClicked(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            var result = await viewmodel.TakePhoto();
            if (result == 5) SetDisplayAlert("Permission", "App required permission to access media gallery to pick photos.", "", "", "");
            if (result == 4) SetDisplayAlert("Permission", "App required permission to access photos to pick photos.", "", "", "");
            if (result == 3) SetDisplayAlert("Unavailable", "Camera is not available or take photo not supported.", "", "", "");
            else if (result == 2) SetDisplayAlert("Alert", "Take photo cancelled.", "", "", "");
            else if (result == 1)
            {
                var picpath = viewmodel.PictureMedia.Path.Split('/');
                var name = picpath[picpath.Length - 1];
                lbl_attach.Text = name;
                viewmodel.HasAttachment = true;
                viewmodel.IsLoading = false;
            }
            view.IsEnabled = true;
        }

        public async void AttachmentClicked(object sender, EventArgs e)
        {
            var view = (Label)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            if (viewmodel.PictureMedia != null)
            {
                await Navigation.PushPopupAsync(new ImageViewer(viewmodel.PictureMedia.GetStream()));
                view.IsEnabled = true;
                return;
            }
            if (viewmodel.Picture != null)
            {
                var converter = new StreamByteConverter();
                await Navigation.PushPopupAsync(new ImageViewer(converter.BytesToStream(viewmodel.Picture)));
            }
            view.IsEnabled = true;
        }

        public async void AttachmentDeleteClicked(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            viewmodel.Picture = null;
            viewmodel.PictureMedia = null;
            viewmodel.HasAttachment = false;
            view.IsEnabled = false;
        }

        public async void SaveClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            if (viewmodel.Amount == 0) SetDisplayAlert("Alert", "Expenses amount is empty. Please insert expenses amount.", "", "", "");
            else if (string.IsNullOrEmpty(viewmodel.Title)) SetDisplayAlert("Alert", "Expenses title is empty. Please insert any title " +
                "(food, groceries, bills, entertainment, transfer, etc...)", "", "", "");
            else
            {
                var result = viewmodel.UpdateExpenses();
                if (result == 1)
                {
                    SetDisplayAlert("Success", "Expenses updated.", "", "", "success");
                    MessagingCenter.Send(this, "ExpensesUpdated", viewmodel.ExpensesDt.ToString("dd.MM.yyyy"));
                }
                else if (result == 2) SetDisplayAlert("Error", "Technical error adding new expenses.", "", "", "");
            }
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

        public void SetDisplayAlert(string title, string description, string btn1, string btn2, string obj)
        {
            //if string1 empty will not display btn1, if string2 empty will not display btn2
            //if both string1 & string2 empty will not display all buttons
            //all buttons tapped will send 'DisplayAlertSelection' with text of the button
            //close button tapped will send 'DisplayAlertSelection' with empty text
            alert.Title = title;
            alert.Description = description;
            alert.TxtBtn1 = btn1;
            alert.TxtBtn2 = btn2;
            alert.IsVisible = true;
            alert.CodeObject = obj;
        }
    }
}