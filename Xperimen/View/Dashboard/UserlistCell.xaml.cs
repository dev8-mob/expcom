
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.Helper;

namespace Xperimen.View.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserlistCell : ViewCell
    {
        #region bindables
        public byte[] Picture
        {
            get { return (byte[])GetValue(PictureProperty); }
            set { SetValue(PictureProperty, value); }
        }
        public static BindableProperty PictureProperty =
            BindableProperty.Create(nameof(Picture), typeof(byte[]), typeof(UserlistCell), defaultValue: null,
                propertyChanged: (bindable, oldVal, newVal) => { ((UserlistCell)bindable).UpdatePicture((byte[])newVal); });
        public void UpdatePicture(byte[] data) 
        {
            img_profile.Source = ImageSource.FromStream(() =>
            {
                var stream = converter.BytesToStream(data);
                return stream;
            });
        }
        #endregion

        public StreamByteConverter converter;

        public UserlistCell()
        {
            InitializeComponent();
            converter = new StreamByteConverter();

            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) img_profile.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                if (theme.Equals("dim")) img_profile.BackgroundColor = Color.FromHex(App.CharcoalGray);
                if (theme.Equals("light")) img_profile.BackgroundColor = Color.FromHex(App.DimGray2);
            }
            else img_profile.BackgroundColor = Color.FromHex(App.DimGray2);
        }
    }
}