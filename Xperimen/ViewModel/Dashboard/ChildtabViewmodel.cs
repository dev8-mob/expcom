using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xperimen.Model;
using Xperimen.View.Dashboard;

namespace Xperimen.ViewModel.Dashboard
{
    public class ChildtabViewmodel : BaseViewModel
    {
        #region bindable properties
        int _itemcount;
        ObservableCollection<Clients> _listclients;
        public int ItemCount
        {
            get => _itemcount;
            set { SetProperty(ref _itemcount, value); }
        }
        public ObservableCollection<Clients> ListClients
        {
            get => _listclients;
            set { SetProperty(ref _listclients, value); }
        }
        #endregion

        #region class properties
        public ICommand LoadDataCommand { get; }
        public SQLiteConnection connection;
        private MaintabViewmodel ParentViewmodel;
        private readonly string TabType;
        public bool IsBlocked;
        private string SearchString;
        #endregion

        public ChildtabViewmodel(string TabType)
        {
            this.TabType = TabType;
            IsBlocked = false;
            ListClients = new ObservableCollection<Clients>();
            connection = new SQLiteConnection(App.DB_PATH);
            LoadDataCommand = new Command(LoadData);
            LoadData();

            MessagingCenter.Subscribe<UserlistCell>(this, "UpdateList", (sender) =>
            { LoadData(); });
        }

        public void LoadData()
        {
            if (!IsBlocked)
            {
                try
                {
                    IsLoading = true;
                    ListClients.Clear();
                    if (TabType.Equals("tab_one"))
                    {
                        var users = connection.Table<Clients>().ToList();
                        ListClients = new ObservableCollection<Clients>(users);
                    }
                    else if (TabType.Equals("tab_two"))
                    {
                        //ListClients.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Vein", AppSize = "25.6 MB", AppRating = "3.9", Description = "It took him a month to finish the meal.", CreatedDatetime = DateTime.Now.AddDays(1) });
                        //ListClients.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Pierce", AppSize = "31.5 MB", AppRating = "3.8", Description = "I want more detailed information.", CreatedDatetime = DateTime.Now.AddDays(2) });
                        //ListClients.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Adoption", AppSize = "26.8 MB", AppRating = "3.7", Description = "Combines are no longer just for farms.", CreatedDatetime = DateTime.Now.AddDays(3) });
                        //ListClients.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Stain", AppSize = "56.1 MB", AppRating = "4.8", Description = "Nothing is as cautiously cuddly as a pet porcupine.", CreatedDatetime = DateTime.Now.AddDays(4) });
                    }

                    #region sort in descending order (sample coding)
                    //if (ListData.Count > 0)
                    //{
                    //    try
                    //    {
                    //        foreach (var data in ListData)
                    //        {
                    //            foreach (var item in ListData)
                    //            {
                    //                var result = data.CreatedDatetime.CompareTo(item.CreatedDatetime);
                    //                if (result < 0)
                    //                {
                    //                    ListData.Move(ListData.IndexOf(data), ListData.IndexOf(item));
                    //                    //return;
                    //                }
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        var error = ex.Message;
                    //        var desc = ex.StackTrace;
                    //    }
                    //}
                    //ListData.Move(0, 3);
                    //ListData.Move(1, 2);
                    //ListData.Move(3, 1);
                    //ListData.Reverse();

                    //var tempList = ListData.OrderBy(p => p.CreatedDatetime);
                    //tempList.ToList().ForEach(q =>
                    //{
                    //    ListData.Remove(q);
                    //    ListData.Add(q);
                    //});

                    //Comparison<MobileApp> datas = new Comparison<MobileApp>((p, q) =>
                    //{
                    //    if (p.CreatedDatetime == q.CreatedDatetime) return 0;
                    //    if (p.CreatedDatetime > q.CreatedDatetime) return 1;
                    //    return -1;
                    //});
                    //List<MobileApp> templist = ListData.ToList();
                    //templist.Sort(datas);
                    //ListData = new ObservableCollection<MobileApp>(templist);
                    //ListData.Reverse();
                    #endregion
                }
                catch (Exception ex)
                {
                    var error = ex.Message;
                    var desc = ex.StackTrace;
                }
                finally
                {
                    IsLoading = false;
                    if (!string.IsNullOrEmpty(SearchString))
                    {
                        var search = ListClients.Where(p => p.Username.ToLower().Contains(SearchString.ToLower()) ||
                                    p.Firstname.ToLower().Contains(SearchString.ToLower()) ||
                                    p.Lastname.ToLower().Contains(SearchString.ToLower())).ToList();
                        ListClients.Clear();
                        if (search.Count > 0)
                        {
                            foreach (var data in search)
                                ListClients.Add(data);
                        }
                    }

                    //var sorted = ListData.OrderByDescending(x => x.CreatedDatetime).ToList();
                    //ListData = new ObservableCollection<MobileApp>(sorted);
                    ItemCount = ListClients.Count;
                    if (ParentViewmodel != null)
                        ParentViewmodel.SetSearchResult();
                }
            }
        }

        public void SetParentBinding(MaintabViewmodel parent)
        {
            parent.ChildViewModels[TabType] = this;
            ParentViewmodel = parent;
        }

        public void SearchKeyword(string searchstring)
        { 
            SearchString = searchstring;
            LoadData();
        }

        public int DeleteUser(string userid)
        {
            try
            {
                var query = "DELETE FROM Clients WHERE Id = '" + userid + "'";
                connection.Query<Clients>(query);
                var cek = connection.Table<Clients>().ToList();
                return 1;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
                return 2;
            }
        }

        //public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
        //{
        //    var sortableList = new List<T>(collection);
        //    sortableList.Sort(comparison);

        //    for (int i = 0; i < sortableList.Count; i++)
        //    {
        //        collection.Move(collection.IndexOf(sortableList[i]), i);
        //    }
        //}
    }
}
