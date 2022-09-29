using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xperimen.Droid.CustomRenderer;
using Xperimen.Stylekit;

[assembly: ExportRenderer(typeof(EditorBorderless), typeof(EditorBorderlessRenderer))]
namespace Xperimen.Droid.CustomRenderer
{
    public class EditorBorderlessRenderer : EditorRenderer
    {
        public EditorBorderlessRenderer(Context ctx) : base(ctx) { }

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