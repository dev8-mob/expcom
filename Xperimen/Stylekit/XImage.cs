
using SQLite;
using Xamarin.Forms;
using Xperimen.ViewModel;

namespace Xperimen.Stylekit
{
    public class XImage : Image
    {
        #region bindables
        #region custom properties
        public string Source1
        {
            get { return (string)GetValue(Source1Property); }
            set { SetValue(Source1Property, value); }
        }
        public string Source2
        {
            get { return (string)GetValue(Source2Property); }
            set { SetValue(Source2Property, value); }
        }
        #endregion
        #region custom properties binding
        public static BindableProperty Source1Property =
            BindableProperty.Create(nameof(Source1), typeof(string), typeof(XImage), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((XImage)bindable).UpdateSource1(); });
        public static BindableProperty Source2Property =
            BindableProperty.Create(nameof(Source2), typeof(string), typeof(XImage), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((XImage)bindable).UpdateSource2(); });
        #endregion
        #region binding implementation
        public void UpdateSource1() { SetupView(); }
        public void UpdateSource2() { SetupView(); }
        #endregion
        #endregion

        public SQLiteConnection Connection;

        public XImage() 
        {
            MessagingCenter.Subscribe<CreateaccViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
            MessagingCenter.Subscribe<LoginViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
        }

        public void SetupView()
        {
            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) Source = Source2;
                else Source = Source1;
            }
            else Source = Source1;
        }
    }
}
