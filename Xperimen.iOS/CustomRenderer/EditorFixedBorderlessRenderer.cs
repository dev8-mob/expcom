using Xperimen.Stylekit;
using Xperimen.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EditorFixedBorderless), typeof(EditorFixedBorderlessRenderer))]
namespace Xperimen.iOS.CustomRenderer
{
    public class EditorFixedBorderlessRenderer : EditorRenderer
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
                //Control.BorderStyle = UITextBorderStyle.None;
                Control.TextContainerInset = new UIEdgeInsets(top, left, bottom, right);
            }
        }
    }
}