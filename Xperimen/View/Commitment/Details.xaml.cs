
using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
using Xperimen.Model;
using Xperimen.Stylekit;
using Xperimen.ViewModel.Commitment;

namespace Xperimen.View.Commitment
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Details : ContentPage
    {
        public CommitmentViewmodel viewmodel;
        public StreamByteConverter converter;
        public string data;
        public SelfCommitment _temp;

        public Details(string data)
        {
            InitializeComponent();
            this.data = data;
            viewmodel = new CommitmentViewmodel();
            converter = new StreamByteConverter();
            _temp = new SelfCommitment();
            BindingContext = viewmodel;
            SetupView();

            MessagingCenter.Subscribe<CustomDisplayAlert, string>(this, "DisplayAlertSelection", async (sender, arg) =>
            {
                if (alert.CodeObject.Equals("nomedia")) viewmodel.IsLoading = false;
                else if (alert.CodeObject.Equals("error"))
                {
                    viewmodel.IsLoading = false;
                    await Navigation.PopAsync();
                }
                else if (alert.CodeObject.Equals("success"))
                {
                    SetupView();
                    stack_edit.IsVisible = false;
                    frame_walletlike.IsVisible = false;
                    frame_view.IsVisible = true;
                    if (viewmodel.HasAttachment) frame_attachment.IsVisible = true;
                    else if (!viewmodel.HasAttachment) frame_attachment.IsVisible = false;
                    stack_donebtns.IsVisible = true;
                    viewmodel.IsLoading = false;
                }
                else if (alert.CodeObject.Equals("delete"))
                {
                    if (arg.Equals("Yes"))
                    {
                        var result = viewmodel.DeleteCommitment(data);
                        if (result == 1)
                        {
                            MessagingCenter.Send(this, "CommitmentDeleted");
                            await Navigation.PopAsync();
                        }
                        if (result == 2) SetDisplayAlert("Error", "Technical error deleting the selected commitment.", "", "", "error");
                    }
                    else if (arg.Equals("Cancel")) viewmodel.IsLoading = false;
                    else viewmodel.IsLoading = false;
                }
                else if (alert.CodeObject.Equals("markdone"))
                {
                    viewmodel.IsLoading = false;
                    SetupView();
                }
            });
        }

        public void SetupView()
        {
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Xamarin.Forms.Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark"))
                {
                    frame_view.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                    stack_bg.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                }
                if (theme.Equals("dim"))
                {
                    frame_view.BackgroundColor = Color.FromHex(App.CharcoalGray);
                    stack_bg.BackgroundColor = Color.FromHex(App.CharcoalGray);
                }
                if (theme.Equals("light"))
                {
                    frame_view.BackgroundColor = Color.FromHex(App.DimGray2);
                    stack_bg.BackgroundColor = Color.FromHex(App.DimGray2);
                }
            }

            // setup for different iphone screen sizes
            if (Device.RuntimePlatform == Device.iOS)
            {
                var lowerscreen = DependencyService.Get<IDeviceInfo>().IsLowerIphoneDevice();
                if (lowerscreen)
                {
                    var safeInsets = On<Xamarin.Forms.PlatformConfiguration.iOS>().SafeAreaInsets();
                    safeInsets.Top = -20;
                    Padding = safeInsets;
                }
            }

            var result = viewmodel.SetDataCommitment(data);
            if (result == 2) SetDisplayAlert("Not Found", "The selected commitment is not found.", "", "", "error");
            else if (result == 3) SetDisplayAlert("Error", "Technical error retrieving the selected commitment.", "", "", "error");

            // set done paid button visibility
            if (viewmodel.IsDone) frame_donepaid.IsVisible = false;
            else frame_donepaid.IsVisible = true;

            // set default data, in case of cancelling edit
            _temp.Title = viewmodel.Title;
            _temp.Description = viewmodel.Description;
            _temp.Amount = viewmodel.Amount;
            _temp.IsDone = viewmodel.IsDone;
            _temp.HasAccNo = viewmodel.HasAccNo;
            _temp.HasAttachment = viewmodel.HasAttachment;
            _temp.AccountNo = viewmodel.AccountNo;
            _temp.Picture = null;
        }

        public async void EditTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            stack_edit.IsVisible = true;
            frame_walletlike.IsVisible = true;
            frame_view.IsVisible = false;
            frame_attachment.IsVisible = false;
            stack_donebtns.IsVisible = false;
            //update selected state in custom checkbox
            checkbox.InitCheckbox(viewmodel.HasAccNo);
            view.IsEnabled = true;
        }

        public async void DeleteTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            SetDisplayAlert("Confirmation", "Are you sure to delete this commitment", "Yes", "Cancel", "delete");
            view.IsEnabled = true;
        }

        public async void CancelClicked(object sender, EventArgs e)
        {
            var view = (Label)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.Title = _temp.Title;
            viewmodel.Description = _temp.Description;
            viewmodel.Amount = _temp.Amount;
            viewmodel.IsDone = _temp.IsDone;
            viewmodel.HasAccNo = _temp.HasAccNo;
            viewmodel.HasAttachment = _temp.HasAttachment;
            viewmodel.AccountNo = _temp.AccountNo;
            lbl_attach.Text = "image_attachment.jpg";

            stack_edit.IsVisible = false;
            frame_walletlike.IsVisible = false;
            frame_view.IsVisible = true;
            if (viewmodel.HasAttachment) frame_attachment.IsVisible = true;
            else if (!viewmodel.HasAttachment) frame_attachment.IsVisible = false;
            stack_donebtns.IsVisible = true;
            view.IsEnabled = true;
        }

        public async void GalleryClicked(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            var result = await viewmodel.PickPhoto();
            if (result == 4) SetDisplayAlert("Permission", "App required permission to access media gallery to pick photos.", "", "", "");
            if (result == 3) SetDisplayAlert("Unavailable", "Photo gallery is not available to pick photo.", "", "", "");
            else if (result == 2) SetDisplayAlert("Alert", "No photo selected.", "", "", "nomedia");
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
            if (result == 4) SetDisplayAlert("Permission", "App required permission to access camera to take photos.", "", "", "");
            if (result == 3) SetDisplayAlert("Unavailable", "Camera is not available or take photo not supported.", "", "", "");
            else if (result == 2) SetDisplayAlert("Alert", "Take photo cancelled.", "", "", "nomedia");
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

        public async void PicAttachmentClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            try
            {
                var bytes = converter.BytesToStream(viewmodel.ProfilePic);
                await Navigation.PushPopupAsync(new ImageViewer(bytes));
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
            }
            view.IsEnabled = true;
        }

        public async void LabelAttachmentClicked(object sender, EventArgs e)
        {
            var view = (Label)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            if (viewmodel.Picture != null)
                await Navigation.PushPopupAsync(new ImageViewer(viewmodel.Picture.GetStream()));
            else if (viewmodel.Picture == null)
                await Navigation.PushPopupAsync(new ImageViewer(converter.BytesToStream(viewmodel.ProfilePic)));
            view.IsEnabled = true;
        }

        public async void AttachmentDeleteClicked(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            viewmodel.HasAttachment = false;
            viewmodel.Picture = null;
            view.IsEnabled = true;
        }

        public async void UpdateCommitmentClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            if (string.IsNullOrEmpty(viewmodel.Title)) SetDisplayAlert("Alert", "Title is empty. Please insert any title (bill, rent, investment, etc...)", "", "", "");
            else if (viewmodel.Amount == 0) SetDisplayAlert("Alert", "Commitment amount is empty. Please insert commitment amount.", "", "", "");
            else if (viewmodel.HasAccNo && viewmodel.AccountNo == 0) SetDisplayAlert("Alert", "Account number  is empty. Please insert account number.", "", "", "");
            else
            {
                var result = viewmodel.UpdateCommitment(data);
                if (result == 1)
                {
                    SetDisplayAlert("Success", "Commitment details updated.", "", "", "success");
                    MessagingCenter.Send(this, "CommitmentUpdated");
                }
                else if (result == 2) SetDisplayAlert("Error", "Technical error when updating commitment.", "", "", "error");
            }
            view.IsEnabled = true;
        }

        public async void DonePaidClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            var result = viewmodel.SetStatusDonePaid(data, true);
            if (result == 1)
            {
                SetDisplayAlert("Done", "Commitment marked as paid.", "", "", "markdone");
                MessagingCenter.Send(this, "CommitmentUpdated");
            }
            view.IsEnabled = true;
        }

        public async void NotDoneClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;

            viewmodel.IsLoading = true;
            var result = viewmodel.SetStatusDonePaid(data, false);
            if (result == 1)
            {
                SetDisplayAlert("Undone", "Commitment marked as not done yet.", "", "", "markdone");
                MessagingCenter.Send(this, "CommitmentUpdated");
            }
            view.IsEnabled = true;
        }

        public async void BackTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            await Navigation.PopAsync();
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