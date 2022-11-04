
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xperimen.View.Setting
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CurrencyListCell : StackLayout
    {
        public CurrencyListCell ()
		{
			InitializeComponent ();

            MessagingCenter.Subscribe<CurrencyListCell, string>(this, "CurrencySelected", (sender, arg) =>
            {
                frame_select.IsVisible = false;
                img_select.IsVisible = false;
                if (lbl_shortform.Text.Equals(arg))
                {
                    frame_select.IsVisible = true;
                    img_select.IsVisible = true;
                }
            });
        }

        public async void CellTapped(object sender, EventArgs e)
        {
            var view = (Grid)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            var build = lbl_shortform.Text + "," + lbl_currcode.Text + "," + lbl_currname.Text;
            MessagingCenter.Send(this, "CurrencySelected", build);
            view.IsEnabled = true;
        }
    }
}