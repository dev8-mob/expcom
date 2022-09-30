using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xperimen.iOS.CustomRenderer;
using Xperimen.Stylekit;

[assembly: ExportRenderer(typeof(TimePickerBorderless), typeof(TimePickerBorderlessRenderer))]
namespace Xperimen.iOS.CustomRenderer
{
    public class TimePickerBorderlessRenderer : TimePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Layer.BorderWidth = 0;
                Control.BorderStyle = UITextBorderStyle.None;
            }
        }
    }
}