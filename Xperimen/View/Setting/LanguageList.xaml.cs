using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.ViewModel.Setting;

namespace Xperimen.View.Setting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LanguageList : PopupPage
    {
        public LanguageViewmodel viewmodel;

        public LanguageList()
        {
            InitializeComponent();
            viewmodel = new LanguageViewmodel();
            BindingContext = viewmodel;

            if (Application.Current.Properties.ContainsKey("app_theme"))
            {
                var theme = Application.Current.Properties["app_theme"] as string;
                if (theme.Equals("dark")) stack_bg.BackgroundColor = Color.FromHex(App.CharcoalBlack);
                if (theme.Equals("dim")) stack_bg.BackgroundColor = Color.FromHex(App.CharcoalGray);
                if (theme.Equals("light")) stack_bg.BackgroundColor = Color.FromHex(App.DimGray2);
            }

            MessagingCenter.Subscribe<LanguageListCell, string>(this, "LanguageSelected", async (sender, arg) =>
            {
                var split = arg.Split(',');
                if (split.Count() > 0)
                {
                    viewmodel.Language = split[0];
                    var success = viewmodel.UpdateLanguage();
                    if (success == 1)
                    {
                        await Task.Delay(300);
                        var navigation = Application.Current.MainPage.Navigation;
                        await navigation.PopPopupAsync();
                        MessagingCenter.Send(this, "LanguageUpdated", arg);
                    }
                }
            });
        }

        public async void CancelClicked(object sender, EventArgs e)
        {
            var view = (Label)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            view.IsEnabled = false;
            var navigation = Application.Current.MainPage.Navigation;
            await navigation.PopPopupAsync();
            view.IsEnabled = true;
        }
    }
}