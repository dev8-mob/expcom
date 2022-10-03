
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xperimen.Stylekit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageViewer : PopupPage
    {
        public ImageViewer(Stream image)
        {
            InitializeComponent();
            if (image != null)
            {
                img_data.Source = ImageSource.FromStream(() =>
                {
                    var stream = image;
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