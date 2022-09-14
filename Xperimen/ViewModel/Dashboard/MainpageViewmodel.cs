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
                AppName = "Dictate",
                AppIcon = "", 
                AppRating = "3.9", 
                AppSize = "35.1 MB", 
                Description = "The fox in the tophat whispered into the ear of the rabbit.",
                CreatedDatetime = new DateTime(1993,8,4)
            });
            AppsList.Add(new MobileApp {
                Id = Guid.NewGuid().ToString(),
                AppName = "Cabin",
                AppIcon = "",
                AppRating = "4.1",
                AppSize = "24.6 MB",
                Description = "Your girlfriend bought your favorite cookie crisp cereal but forgot to get milk.",
            });
            AppsList.Add(new MobileApp {
                Id = Guid.NewGuid().ToString(),
                AppName = "Eyebrow",
                AppIcon = "",
                AppRating = "3.8",
                AppSize = "49.2 MB",
                Description = "As time wore on, simple dog commands turned into full paragraphs explaining why the dog couldn’t do something.",
                CreatedDatetime = new DateTime(1993, 8, 5)
            });
            AppsList.Add(new MobileApp {
                Id = Guid.NewGuid().ToString(),
                AppName = "Tease",
                AppIcon = "",
                AppRating = "4.5",
                AppSize = "18.5 MB",
                Description = "The clouds formed beautiful animals in the sky that eventually created a tornado to wreak havoc."
            });
            AppsList.Add(new MobileApp {
                Id = Guid.NewGuid().ToString(),
                AppName = "Depart",
                AppIcon = "",
                AppRating = "4.7",
                AppSize = "35 MB",
                Description = "Grape jelly was leaking out the hole in the roof.",
                CreatedDatetime = new DateTime(1993, 8, 6)
            });
            AppsList.Add(new MobileApp {
                Id = Guid.NewGuid().ToString(),
                AppName = "Assignment",
                AppIcon = "",
                AppRating = "3.7",
                AppSize = "27.8 MB",
                Description = "He quietly entered the museum as the super bowl started."
            });
            await Task.Delay(3000);
            IsLoading = false;
        }
    }
}
