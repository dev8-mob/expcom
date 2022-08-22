
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xperimen.Stylekit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomDisplayAlert : Frame
    {
        #region bindables
        #region properties
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        #endregion
        #region properties binding
        public static BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomDisplayAlert), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomDisplayAlert)bindable).UpdateTitle((string)newVal); });
        public static BindableProperty DescriptionProperty =
            BindableProperty.Create(nameof(Description), typeof(string), typeof(CustomDisplayAlert), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomDisplayAlert)bindable).UpdateContent((string)newVal); });
        #endregion
        #region binding implementation
        public void UpdateTitle(string data) { lbl_title.Text = data; }
        public void UpdateContent(string data) { lbl_desc.Text = data; }
        #endregion
        #endregion

        public CustomDisplayAlert()
        {
            InitializeComponent();
        }
    }
}