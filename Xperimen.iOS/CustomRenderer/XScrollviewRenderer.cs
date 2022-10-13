
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xperimen.iOS.CustomRenderer;
using Xperimen.Stylekit;

[assembly: ExportRenderer(typeof(XScrollview), typeof(XScrollviewRenderer))]
namespace Xperimen.iOS.CustomRenderer
{
    public class XScrollviewRenderer : ScrollViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null) return;

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Never;
        }
    }
}