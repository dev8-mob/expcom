using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;
using Xperimen.Stylekit;

namespace Xperimen.View.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommitmentDetailCell : Grid
    {
        #region bindables
        public byte[] Picture
        {
            get => (byte[])GetValue(PictureProperty);
            set { SetValue(PictureProperty, value); }
        }
        public static BindableProperty PictureProperty =
            BindableProperty.Create(nameof(Picture), typeof(byte[]), typeof(CommitmentDetailCell), defaultValue: null,
                propertyChanged: (bindable, oldVal, newVal) => { ((CommitmentDetailCell)bindable).UpdatePicture((byte[])newVal); });
        public void UpdatePicture(byte[] data)
        {
            if (data != null)
            {
                frame_img.IsVisible = true;
                img_attach.Source = ImageSource.FromStream(() =>
                {
                    var stream = converter.BytesToStream(data);
                    return stream;
                });
            }
        }
        #endregion

        public StreamByteConverter converter;

        public CommitmentDetailCell()
        {
            InitializeComponent();
            converter = new StreamByteConverter();
            SetupView();
        }

        public void SetupView()
        {
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark"))
                {
                    stack_bg.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                    frame_img.BackgroundColor = Color.Black;
                }
                if (theme.Equals("dim"))
                {
                    stack_bg.BackgroundColor = Color.FromHex(App.CharcoalGray);
                    frame_img.BackgroundColor = Color.FromHex(App.SlateGray);
                }
                if (theme.Equals("light"))
                {
                    stack_bg.BackgroundColor = Color.FromHex(App.DimGray2);
                    frame_img.BackgroundColor = Color.FromHex(App.DimGray1);
                }
            }
            else
            {
                stack_bg.BackgroundColor = Color.FromHex(App.DimGray2);
                frame_img.BackgroundColor = Color.FromHex(App.DimGray1);
            }
        }

        public async void ImageTapped(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 250);
            view.Scale = 1;
            view.IsEnabled = false;
            await Navigation.PushPopupAsync(new ImageViewer(converter.BytesToStream(Picture)));
            view.IsEnabled = true;
        }

        public async void SelectTapped(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 250);
            view.Scale = 1;
            view.IsEnabled = false;
            var parent = (CommitmentDetails)view.Parent.Parent.Parent.Parent.Parent.Parent.Parent;
            parent.viewmodel.IsLoading = true;
            parent.SetDisplayAlert("Mark As Done", "Are you sure to mark " + lbl_title.Text + " as done ?", "Yes", "Cancel", lbl_id.Text);
            view.IsEnabled = true;
        }
    }
}