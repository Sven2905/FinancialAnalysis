using DevExpress.Mvvm;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using Formulas.DepreciationMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using WebApiWrapper.Accounting;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class DepreciationViewModel : ViewModelBase
    {
        public DepreciationViewModel()
        {
            GetData();
            NewDepreciationItemCommand = new DelegateCommand(NewSelectedDepreciationItem);

            SaveDepreciationItemCommand = new DelegateCommand(() => SaveSelectedDepreciationItem(), 
                SelectedDepreciationItem != null && SelectedDepreciationItem.InitialValue > 0 
                && !string.IsNullOrEmpty(SelectedDepreciationItem.Name) && SelectedDepreciationItem.Years > 0 && SelectedDepreciationItem.StartYear > 0);

            DeleteDepreciationItemCommand = new DelegateCommand(() => DeleteSelectedDepreciationItem(), SelectedDepreciationItem != null);
        }

        #region Fields

        private string _FilterText;

        #endregion Fields

        #region Properties

        public DelegateCommand NewDepreciationItemCommand { get; set; }
        public DelegateCommand SaveDepreciationItemCommand { get; set; }
        public DelegateCommand DeleteDepreciationItemCommand { get; set; }
        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(value))
                {
                    FilteredDepreciationItems = new SvenTechCollection<DepreciationItem>();
                    FilteredDepreciationItems.AddRange(DepreciationItemList.Where(x => x.Name.ToLower().Contains(_FilterText.ToLower())));
                }
                else
                    FilteredDepreciationItems = DepreciationItemList;
            }
        }

        private DepreciationItem _SelectedDepreciationItem;
        public SvenTechCollection<DepreciationItem> DepreciationItemList { get; set; }
        public SvenTechCollection<CostAccount> CostAccountList { get; private set; }
        public SvenTechCollection<DepreciationItem> FilteredDepreciationItems { get; set; }
        public DepreciationItem SelectedDepreciationItem
        {
            get { return _SelectedDepreciationItem; }
            set { _SelectedDepreciationItem = value; DepreciationBaseViewModel.DepreciationItem = value; }
        }

        public DepreciationBaseViewModel DepreciationBaseViewModel { get; set; } = new DepreciationBaseViewModel();

        #endregion Properties

        #region Methods

        private void GetData()
        {
            DepreciationItemList = DepreciationItems.GetAll().ToSvenTechCollection();
            CostAccountList = CostAccounts.GetAll().ToSvenTechCollection();
        }

        private void SaveSelectedDepreciationItem()
        {
            if (SelectedDepreciationItem.DepreciationItemId > 0)
                DepreciationItems.Insert(SelectedDepreciationItem);
            else
                DepreciationItems.Update(SelectedDepreciationItem);
        }

        private void DeleteSelectedDepreciationItem()
        {
            DepreciationItemList.Remove(SelectedDepreciationItem);
            FilteredDepreciationItems.Remove(SelectedDepreciationItem);
            if (SelectedDepreciationItem.DepreciationItemId > 0)
                DepreciationItems.Delete(SelectedDepreciationItem.DepreciationItemId);
        }

        private void NewSelectedDepreciationItem()
        {
            SelectedDepreciationItem = new DepreciationItem();
        }

        #endregion Methods
    }
}
