
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xperimen.Model;

namespace Xperimen.ViewModel.Dashboard
{
    public class MaintabViewmodel : BaseViewModel
    {
        #region bindable properties
        int _itemCount;
        string _dataString;
        bool _isFound;

        public string DataString
        {
            get => _dataString;
            set
            {
                SetProperty(ref _dataString, value);
                IsFound = !string.IsNullOrEmpty(_dataString);
            }
        }

        public bool IsFound
        {
            get => _isFound;
            set { SetProperty(ref _isFound, value); }
        }

        public int ItemCount
        {
            get => _itemCount;
            set { SetProperty(ref _itemCount, value); }
        }
        #endregion

        #region class properties
        private string SelectedTab = "tab_one";
        public Dictionary<string, ChildtabViewmodel> ChildViewModels { get; set; }
        public ICommand LoadDataCommand { get; }
        public ICommand ClearSearch { get; }
        #endregion

        public MaintabViewmodel()
        {
            ChildViewModels = new Dictionary<string, ChildtabViewmodel>();
            LoadDataCommand = new Command(LoadData);
            ClearSearch = new Command(ClearSearchResult);
        }

        public void LoadData()
        { ChildViewModels[SelectedTab].SearchKeyword(DataString); }

        public void SetSelectedTab(string SelectedTab)
        {
            this.SelectedTab = SelectedTab;
            LoadData();
        }

        public void SetSearchResult()
        { ItemCount = ChildViewModels[SelectedTab].ItemCount; }

        public void ClearSearchResult()
        { 
            DataString = string.Empty;
            LoadData();
        }
    }
}
