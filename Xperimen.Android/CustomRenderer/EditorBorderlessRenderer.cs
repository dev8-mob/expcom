using Android.Content;
using Xperimen.Stylekit;
using Xperimen.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

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