
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xperimen.View.Setting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LanguageListCell : StackLayout
    {
        public LanguageListCell()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<LanguageListCell, string>(this, "LanguageSelected", (sender, arg) =>
            {
                frame_select.IsVisible = false;
                img_select.IsVisible = false;

                if (!string.IsNullOrEmpty(arg))
                {
                    var split = arg.Split(',');
                    if (lbl_code.Text.Equals(split[0]))
                    {
                        frame_select.IsVisible = true;
                        img_select.IsVisible = true;
                    }
                }
            });
        }

        public async void CellTapped(object sender, EventArgs e)
        {
            var view = (Grid)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            var build = lbl_code.Text + "," + lbl_langname.Text;
            MessagingCenter.Send(this, "LanguageSelected", build);
            //view.IsEnabled = true;
        }
    }
}