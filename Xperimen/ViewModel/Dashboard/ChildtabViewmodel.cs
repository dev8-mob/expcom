using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xperimen.Model;

namespace Xperimen.ViewModel.Dashboard
{
    public class ChildtabViewmodel : BaseViewModel
    {
        #region class properties
        private MaintabViewmodel ParentViewmodel;
        private readonly string TabType;
        private string SearchString;
        public ObservableCollection<MobileApp> ListData { get; set; }
        public ICommand LoadDataCommand { get; }
        #endregion

        #region bindable properties
        int itemCount;
        public int ItemCount
        {
            get => itemCount;
            set { SetProperty(ref itemCount, value); }
        }
        #endregion

        public ChildtabViewmodel(string TabType)
        {
            this.TabType = TabType;
            ListData = new ObservableCollection<MobileApp>();
            LoadDataCommand = new Command(LoadData);
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                IsLoading = true;
                ListData.Clear();
                if (TabType.Equals("tab_one"))
                {
                    ListData.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Donatello", AppSize = "25.6 MB", AppRating = "4.9", Description = "There's a gross fly on the ceiling." });
                    ListData.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Michelangelo", AppSize = "31.5 MB", AppRating = "4.8", Description = "I have an organic banana bread maker." });
                    ListData.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Raphaello", AppSize = "26.8 MB", AppRating = "4.7", Description = "I need to cook lunch." });
                    ListData.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Leonardo", AppSize = "56.1 MB", AppRating = "5.0", Description = "What a big house you have!" });
                }
                else if (TabType.Equals("tab_two"))
                {
                    ListData.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Vein", AppSize = "25.6 MB", AppRating = "3.9", Description = "It took him a month to finish the meal." });
                    ListData.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Pierce", AppSize = "31.5 MB", AppRating = "3.8", Description = "I want more detailed information." });
                    ListData.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Adoption", AppSize = "26.8 MB", AppRating = "3.7", Description = "Combines are no longer just for farms." });
                    ListData.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Stain", AppSize = "56.1 MB", AppRating = "4.8", Description = "Nothing is as cautiously cuddly as a pet porcupine." });
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
            }
            finally
            {
                IsLoading = false;

                try
                {
                    if (!string.IsNullOrEmpty(SearchString))
                    {
                        var search = ListData.Where(p => p.AppName.ToLower().Contains(SearchString.ToLower())).ToList();
                        if (search.Count > 0)
                        {
                            ListData.Clear();
                            foreach (var data in search)
                                ListData.Add(data);
                        }
                    }
                    ItemCount = ListData.Count;
                    ParentViewmodel.SetSearchResult();
                }
                catch (Exception ex)
                {
                    var error = ex.Message;
                    var desc = ex.StackTrace;
                }
            }
        }

        public void SetParentBinding(MaintabViewmodel parent)
        {
            parent.ChildViewModels[TabType] = this;
            ParentViewmodel = parent;
        }

        public void SearchKeyword(string SearchString)
        { 
            this.SearchString = SearchString;
            LoadData();
        }
    }
}
