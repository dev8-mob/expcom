using Android.Content;
using Xperimen.Stylekit;
using Xperimen.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(EditorFixedBorderless), typeof(EditorFixedBorderlessRenderer))]
namespace Xperimen.Droid.CustomRenderer
{
    public class EditorFixedBorderlessRenderer : EditorRenderer
    {
        public EditorFixedBorderlessRenderer(Context ctx) : base(ctx) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackground(null);
                Control.SetPadding(40, 40, 40, 40);
            }
        }
    }
}