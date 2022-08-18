using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xperimen.Helper;
using Xperimen.Model;

namespace Xperimen.ViewModel.Dashboard
{
    class MainpageViewmodel : BaseViewModel
    {
        public ICommand LoadItems { get; set; }

        ObservableCollection<MobileApp> _appslist;
        public ObservableCollection<MobileApp> AppsList
        {
            get { return _appslist; }
            set { _appslist = value; OnPropertyChanged(); }
        }

        public MainpageViewmodel()
        {
            LoadItems = new Command(SetupData);
            SetupData();
        }

        public async void SetupData()
        {
            IsLoading = true;
            AppsList = new ObservableCollection<MobileApp>();
            AppsList.Add(new MobileApp {
                Id = Guid.NewGuid().ToString(),
                AppIcon = "", 
                AppRating = "test value == null", 
                AppSize = "35.1 MB", 
                Description = "The fox in the tophat whispered into the ear of the rabbit.",
                CreatedDatetime = new DateTime(1993,8,4)
            });
            AppsList.Add(new MobileApp {
                Id = Guid.NewGuid().ToString(),
                AppName = "commonsense",
                AppIcon = "",
                AppRating = "test value != null",
                AppSize = "24.6 MB",
                Description = "Your girlfriend bought your favorite cookie crisp cereal but forgot to get milk.",
            });
            AppsList.Add(new MobileApp {
                Id = Guid.NewGuid().ToString(),
                AppName = "NULL",
                AppIcon = "",
                AppRating = "text.Contains(\"NULL\")",
                AppSize = "49.2 MB",
                Description = "As time wore on, simple dog commands turned into full paragraphs explaining why the dog couldn’t do something.",
                CreatedDatetime = new DateTime(1993, 8, 5)
            });
            AppsList.Add(new MobileApp {
                Id = Guid.NewGuid().ToString(),
                AppName = "",
                AppIcon = "",
                AppRating = "text == string.Empty",
                AppSize = "18.5 MB",
                Description = "The clouds formed beautiful animals in the sky that eventually created a tornado to wreak havoc."
            });
            AppsList.Add(new MobileApp {
                Id = Guid.NewGuid().ToString(),
                AppName = "depart",
                AppIcon = "",
                AppRating = "hero",
                AppSize = "35 MB",
                Description = "Grape jelly was leaking out the hole in the roof.",
                CreatedDatetime = new DateTime(1993, 8, 6)
            });
            AppsList.Add(new MobileApp {
                Id = Guid.NewGuid().ToString(),
                AppName = "assignment",
                AppIcon = "",
                AppRating = "cylinder",
                AppSize = "27.8 MB",
                Description = "He quietly entered the museum as the super bowl started."
            });
            await Task.Delay(3000);
            IsLoading = false;
        }
    }
}
