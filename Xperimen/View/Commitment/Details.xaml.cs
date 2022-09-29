
using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Forms;
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
                viewmodel.IsLoading = false;
                if (alert.CodeObject.Equals("error"))
                    await Navigation.PopAsync();
            });
        }

        public void SetupView()
        {
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) frame_view.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                if (theme.Equals("dim")) frame_view.BackgroundColor = Color.FromHex(App.CharcoalGray);
                if (theme.Equals("light")) frame_view.BackgroundColor = Color.FromHex(App.DimGray2);
            }

            var result = viewmodel.SetDataCommitment(data);
            if (result == 2) SetDisplayAlert("Not Found", "The selected commitment is not found.", "", "", "error");
            else if (result == 3) SetDisplayAlert("Error", "Technical error retrieving the selected commitment.", "", "", "error");

            //default data, in case of cancelling edit
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

            stack_edit.IsVisible = true;
            await stack_edit.FadeTo(1, 200);
            frame_view.IsVisible = false;
            frame_attachment.IsVisible = false;
            //update selected state in custom checkbox
            checkbox.InitCheckbox(viewmodel.HasAccNo);
        }

        public async void DeleteTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;

            viewmodel.IsLoading = true;
            SetDisplayAlert("Confirmation", "Are you sure to delete this commitment", "Yes", "Cancel", "");
        }

        public async void CancelClicked(object sender, EventArgs e)
        {
            var view = (Label)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;

            viewmodel.Title = _temp.Title;
            viewmodel.Description = _temp.Description;
            viewmodel.Amount = _temp.Amount;
            viewmodel.IsDone = _temp.IsDone;
            viewmodel.HasAccNo = _temp.HasAccNo;
            viewmodel.HasAttachment = _temp.HasAttachment;
            viewmodel.AccountNo = _temp.AccountNo;
            lbl_attach.Text = "image_attachment.jpg";

            await stack_edit.FadeTo(0, 200);
            stack_edit.IsVisible = false;
            frame_view.IsVisible = true;
            if (viewmodel.HasAttachment) frame_attachment.IsVisible = true;
            else if (!viewmodel.HasAttachment) frame_attachment.IsVisible = false;
        }

        public async void GalleryClicked(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;

            viewmodel.IsLoading = true;
            var result = await viewmodel.PickPhoto();
            if (result == 3) SetDisplayAlert("Unavailable", "Photo gallery is not available to pick photo.", "", "", "");
            else if (result == 2) SetDisplayAlert("Alert", "No photo selected.", "", "", "");
            else if (result == 1)
            {
                var picpath = viewmodel.Picture.Path.Split('/');
                var name = picpath[picpath.Length - 1];
                lbl_attach.Text = name;
                viewmodel.HasAttachment = true;
                viewmodel.IsLoading = false;
            }
        }

        public async void CameraClicked(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;

            viewmodel.IsLoading = true;
            var result = await viewmodel.TakePhoto();
            if (result == 3) SetDisplayAlert("Unavailable", "Camera is not available or take photo not supported.", "", "", "");
            else if (result == 2) SetDisplayAlert("Alert", "Take photo cancelled.", "", "", "");
            else if (result == 1)
            {
                var picpath = viewmodel.Picture.Path.Split('/');
                var name = picpath[picpath.Length - 1];
                lbl_attach.Text = name;
                viewmodel.HasAttachment = true;
                viewmodel.IsLoading = false;
            }
        }

        public async void PicAttachmentClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            await Navigation.PushPopupAsync(new ImageViewer(converter.BytesToStream(viewmodel.ProfilePic)));
        }

        public async void LabelAttachmentClicked(object sender, EventArgs e)
        {
            var view = (Label)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;

            if (viewmodel.Picture != null)
                await Navigation.PushPopupAsync(new ImageViewer(viewmodel.Picture.GetStream()));
            else if (viewmodel.Picture == null)
                await Navigation.PushPopupAsync(new ImageViewer(converter.BytesToStream(viewmodel.ProfilePic)));
        }

        public async void AttachmentDeleteClicked(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            viewmodel.HasAttachment = false;
        }

        public async void BackTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            await Navigation.PopAsync();
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