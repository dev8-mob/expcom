using Android.Content;
using Xperimen.Stylekit;
using Xperimen.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(EditorExpandableBorderless), typeof(EditorExpandableBorderlessRenderer))]
namespace Xperimen.Droid.CustomRenderer
{
    public class EditorExpandableBorderlessRenderer : EditorRenderer
    {
        public EditorExpandableBorderlessRenderer(Context ctx) : base(ctx) { }

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