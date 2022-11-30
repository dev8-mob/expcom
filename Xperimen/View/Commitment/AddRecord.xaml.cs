using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Resources;
using Xperimen.Stylekit;
using Xperimen.ViewModel.Commitment;

namespace Xperimen.View.Commitment
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddRecord : ContentPage
    {
        public CommitmentViewmodel viewmodel;

        public AddRecord()
        {
            InitializeComponent();
            viewmodel = new CommitmentViewmodel();
            BindingContext = viewmodel;

            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) stack_bg.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                if (theme.Equals("dim")) stack_bg.BackgroundColor = Color.FromHex(App.CharcoalGray);
                if (theme.Equals("light")) stack_bg.BackgroundColor = Color.FromHex(App.DimGray2);
            }

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", async (sender, arg) =>
            {
                viewmodel.IsLoading = false;
                if (alert.CodeObject.Equals("success"))
                    await Navigation.PopAsync();
            });
        }

        public async void GalleryClicked(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            var result = await viewmodel.PickPhoto();
            if (result == 5) SetDisplayAlert(AppResources.app_permission, AppResources.camgal_permmedia, "", "", "");
            if (result == 4) SetDisplayAlert(AppResources.app_permission, AppResources.camgal_permphoto, "", "", "");
            if (result == 3) SetDisplayAlert(AppResources.app_unavailable, AppResources.camgal_photounavailable, "", "", "");
            else if (result == 2) SetDisplayAlert(AppResources.app_alert, AppResources.camgal_nophotoselect, "", "", "");
            else if (result == 1)
            {
                var picpath = viewmodel.Picture.Path.Split('/');
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
            if (result == 5) SetDisplayAlert(AppResources.app_permission, AppResources.camgal_permmedia, "", "", "");
            if (result == 4) SetDisplayAlert(AppResources.app_permission, AppResources.camgal_permphoto, "", "", "");
            if (result == 3) SetDisplayAlert(AppResources.app_unavailable, AppResources.camgal_camunavailable, "", "", "");
            else if (result == 2) SetDisplayAlert(AppResources.app_alert, AppResources.camgal_camcancel, "", "", "");
            else if (result == 1)
            {
                var picpath = viewmodel.Picture.Path.Split('/');
                var name = picpath[picpath.Length - 1];
                lbl_attach.Text = name;
                viewmodel.HasAttachment = true;
                viewmodel.IsLoading = false;
            }
            view.IsEnabled = true;
        }

        public async void SaveCommitmentClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            if (string.IsNullOrEmpty(viewmodel.Title)) SetDisplayAlert(AppResources.app_alert, AppResources.comm_emptytitle, "", "", "");
            else if (viewmodel.Amount == 0) SetDisplayAlert(AppResources.app_alert, AppResources.comm_emptyamount, "", "", "");
            else if (viewmodel.HasAccNo && viewmodel.AccountNo == 0) SetDisplayAlert(AppResources.app_alert, AppResources.comm_emptyaccno, "", "", "");
            else
            {
                var result = viewmodel.AddCommitment();
                if (result == 1)
                {
                    SetDisplayAlert(AppResources.app_success, AppResources.comm_commadded, "", "", "success");
                    MessagingCenter.Send(this, "CommitmentAdded");
                }
                else if (result == 2) SetDisplayAlert(AppResources.app_error, AppResources.comm_erroraddcomm, "", "", "");
            }
            view.IsEnabled = true;
        }

        public async void CancelClicked(object sender, EventArgs e)
        {
            var view = (Label)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            await Navigation.PopAsync();
            view.IsEnabled = true;
        }

        public async void AttachmentClicked(object sender, EventArgs e)
        {
            if (viewmodel.Picture != null)
            {
                var view = (Label)sender;
                await view.ScaleTo(0.9, 100);
                view.Scale = 1;
                view.IsEnabled = false;
                await Navigation.PushPopupAsync(new ImageViewer(viewmodel.Picture.GetStream()));
                view.IsEnabled = true;
            }
        }

        public async void AttachmentDeleteClicked(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            viewmodel.Picture = null;
            viewmodel.HasAttachment = false;
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