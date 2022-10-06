
using Xamarin.Forms;

namespace Xperimen.Stylekit
{
    public class XImageNoRecord : Image
    {
        public XImageNoRecord()
        {
            Aspect = Aspect.AspectFit;
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) Source = "norecord_dark";
                if (theme.Equals("dim")) Source = "norecord_dim";
                if (theme.Equals("light")) Source = "norecord_light";
            }
            else Source = "norecord_light";
        }
    }
}
