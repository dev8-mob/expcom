using CoreGraphics;
using Xperimen.Stylekit;
using Xperimen.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EntryBorderless), typeof(EntryBorderlessRenderer))]
namespace Xperimen.iOS.CustomRenderer
{
    public class EntryBorderlessRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Layer.BorderWidth = 0;
                Control.BorderStyle = UITextBorderStyle.None;
                //use 'Done' on keyboard
                UITextField textfield = Control;
                textfield.ReturnKeyType = UIReturnKeyType.Done;
                //no auto-correct
                textfield.EnablesReturnKeyAutomatically = true;
                textfield.AutocorrectionType = UITextAutocorrectionType.No;
                textfield.SpellCheckingType = UITextSpellCheckingType.No;
                //textfield.AutocapitalizationType = UITextAutocapitalizationType.Words;

                textfield.LeftView = new UIView(new CGRect(0, 0, 15, 0));
                textfield.LeftViewMode = UITextFieldViewMode.Always;
                textfield.RightView = new UIView(new CGRect(0, 0, 15, 0));
                textfield.RightViewMode = UITextFieldViewMode.Always;
            }
        }
    }
}