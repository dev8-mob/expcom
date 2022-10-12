using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xperimen.iOS.CustomRenderer;
using Xperimen.Stylekit;

[assembly: ExportRenderer(typeof(EditorBorderless), typeof(EditorBorderlessRenderer))]
namespace Xperimen.iOS.CustomRenderer
{
    public class EditorBorderlessRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                int top = 15;
                int left = 15;
                int bottom = 15;
                int right = 15;
                Control.Layer.BorderWidth = 0;
                Control.BackgroundColor = null;
                //Control.BorderStyle = UITextBorderStyle.None;
                Control.TextContainerInset = new UIEdgeInsets(top, left, bottom, right);
            }
        }
    }
}