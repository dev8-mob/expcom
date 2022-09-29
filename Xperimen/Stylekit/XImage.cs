
using Xamarin.Forms;
using Xperimen.Helper;
using Xperimen.ViewModel;
using Xperimen.ViewModel.Setting;

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
        public byte[] ImageByte
        {
            get => (byte[])GetValue(ImageByteProperty);
            set { SetValue(ImageByteProperty, value); }
        }
        #endregion
        #region custom properties binding
        public static BindableProperty Source1Property =
            BindableProperty.Create(nameof(Source1), typeof(string), typeof(XImage), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((XImage)bindable).UpdateSource1(); });
        public static BindableProperty Source2Property =
            BindableProperty.Create(nameof(Source2), typeof(string), typeof(XImage), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((XImage)bindable).UpdateSource2(); });
        public static BindableProperty ImageByteProperty =
            BindableProperty.Create(nameof(ImageByte), typeof(byte[]), typeof(XImage), defaultValue: null,
                propertyChanged: (bindable, oldVal, newVal) => { ((XImage)bindable).UpdateImageByte((byte[])newVal); });
        #endregion
        #region binding implementation
        public void UpdateSource1() { SetupView(); }
        public void UpdateSource2() { SetupView(); }
        public void UpdateImageByte(byte[] data)
        {
            if (data != null)
            {
                Source = ImageSource.FromStream(() =>
                {
                    var stream = converter.BytesToStream(data);
                    return stream;
                });
            }
        }
        #endregion
        #endregion

        public StreamByteConverter converter;

        public XImage() 
        {
            converter = new StreamByteConverter();
            MessagingCenter.Subscribe<CreateaccViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
            MessagingCenter.Subscribe<LoginViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
            MessagingCenter.Subscribe<SettingViewmodel>(this, "AppThemeUpdated", (sender) => { SetupView(); });
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
