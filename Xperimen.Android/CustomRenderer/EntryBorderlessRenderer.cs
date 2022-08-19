using Android.Content;
using Xperimen.Stylekit;
using Xperimen.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(EntryBorderless), typeof(EntryBorderlessRenderer))]
namespace Xperimen.Droid.CustomRenderer
{
    public class EntryBorderlessRenderer : EntryRenderer
    {
        public EntryBorderlessRenderer(Context ctx) : base(ctx) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackground(null);
                Control.SetPadding(40, 0, 40, 0);
            }
        }
    }
}