
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xperimen.Stylekit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomEditorExpandable : Frame
    {
        #region bindables
        #region custom properties
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public string GetText() { return editor.Text; }
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }
        public bool Isfocus
        {
            get { return (bool)GetValue(IsFocusProperty); }
            set { SetValue(IsFocusProperty, value); }
        }
        #endregion
        #region custom properties binding
        public static BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomEditorExpandable), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEditorExpandable)bindable).UpdateText((string)newVal); });
        public static BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(CustomEditorExpandable), defaultValue: "",
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEditorExpandable)bindable).UpdatePlaceholder((string)newVal); });
        public static BindableProperty IsFocusProperty =
            BindableProperty.Create(nameof(Isfocus), typeof(bool), typeof(CustomEditorExpandable), defaultValue: false,
                propertyChanged: (bindable, oldVal, newVal) => { ((CustomEditorExpandable)bindable).UpdateIsfocus((bool)newVal); });
        #endregion
        #region binding implementation
        public void UpdateText(string data) { editor.Text = data; }
        public void UpdatePlaceholder(string data) { editor.Placeholder = data; }
        public void UpdateIsfocus(bool data) { if (data) editor.Focus(); else editor.Unfocus(); }
        #endregion
        #endregion

        public CustomEditorExpandable()
        {
            InitializeComponent();
            editor.Focused += Editor_Focused;
            editor.Unfocused += Editor_Unfocused;
        }

        private void Editor_Focused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused)
            {
                line.BackgroundColor = Color.FromHex(App.Primary);
                BackgroundColor = Color.White;
            }
        }

        private void Editor_Unfocused(object sender, FocusEventArgs e)
        {
            if (!e.IsFocused)
            {
                line.BackgroundColor = Color.DarkGray;
                BackgroundColor = Color.FromHex(App.DimGray2);
            }
        }
    }
}