using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xperimen.Droid.CustomRenderer;
using Xperimen.Stylekit;

[assembly: ExportRenderer(typeof(PickerBorderless), typeof(PickerBorderlessRenderer))]
namespace Xperimen.Droid.CustomRenderer
{
    public class PickerBorderlessRenderer : PickerRenderer
    {
        public PickerBorderlessRenderer(Context ctx) : base(ctx) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = null;

                var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                layoutParams.SetMargins(0, 0, 0, 0);
                LayoutParameters = layoutParams;
                Control.LayoutParameters = layoutParams;
                Control.SetPadding(40, 0, 40, 0);
            }
        }
    }
}