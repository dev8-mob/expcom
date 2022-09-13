﻿using System;
using System.Collections.Generic;
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
                    ListData.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Donatello", AppSize = "25.6 MB", AppRating = "4.9", Description = "There's a gross fly on the ceiling.", CreatedDatetime = DateTime.Now.AddDays(1) });
                    ListData.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Michelangelo", AppSize = "31.5 MB", AppRating = "4.8", Description = "I have an organic banana bread maker.", CreatedDatetime = DateTime.Now.AddDays(2) });
                    ListData.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Raphaello", AppSize = "26.8 MB", AppRating = "4.7", Description = "I need to cook lunch.", CreatedDatetime = DateTime.Now.AddDays(3) });
                    ListData.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Leonardo", AppSize = "56.1 MB", AppRating = "5.0", Description = "What a big house you have!", CreatedDatetime = DateTime.Now.AddDays(4) });
                }
                else if (TabType.Equals("tab_two"))
                {
                    ListData.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Vein", AppSize = "25.6 MB", AppRating = "3.9", Description = "It took him a month to finish the meal.", CreatedDatetime = DateTime.Now.AddDays(1) });
                    ListData.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Pierce", AppSize = "31.5 MB", AppRating = "3.8", Description = "I want more detailed information.", CreatedDatetime = DateTime.Now.AddDays(2) });
                    ListData.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Adoption", AppSize = "26.8 MB", AppRating = "3.7", Description = "Combines are no longer just for farms.", CreatedDatetime = DateTime.Now.AddDays(3) });
                    ListData.Add(new MobileApp { Id = Guid.NewGuid().ToString(), AppName = "Stain", AppSize = "56.1 MB", AppRating = "4.8", Description = "Nothing is as cautiously cuddly as a pet porcupine.", CreatedDatetime = DateTime.Now.AddDays(4) });
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

                try
                {
                    if (!string.IsNullOrEmpty(SearchString))
                    {
                        var search = ListData.Where(p => p.AppName.ToLower().Contains(SearchString.ToLower())).ToList();
                        ListData.Clear();
                        if (search.Count > 0)
                        {
                            foreach (var data in search)
                                ListData.Add(data);
                        }
                    }

                    //var sorted = ListData.OrderByDescending(x => x.CreatedDatetime).ToList();
                    //ListData = new ObservableCollection<MobileApp>(sorted);
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
