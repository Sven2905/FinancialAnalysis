using FinancialAnalysis.Models.Models;
using FinancialAnalysis.Models.Models.Accounting;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModel
{
    public class KontenrahmenViewModel : ViewModelBase
    {
        #region Fields

        private CostAccount _SelectedItem;
        private string _Filter;
        private List<CostAccount> _CostAccounts;

        #endregion Fields

        #region Constructor

        public KontenrahmenViewModel()
        {
            RefreshCostAccounts();
            RefreshCommand = new RelayCommand(() =>
            {
                RefreshCostAccounts();
            });
            SelectedCommand = new RelayCommand(() =>
            {
                SendSelectedToParent();
                CloseAction();
            });

            FilterList();
        }

        #endregion Constructor

        #region Methods

        private void RefreshCostAccounts()
        {

        }

        private void FilterList()
        {
            if (!string.IsNullOrEmpty(Filter))
            {
                FilteredList = _CostAccounts.Where(x => x.Name.ToLower().Contains(Filter.ToLower())).ToList();
                RaisePropertyChanged("FilteredList");
            }
            else
            {
                FilteredList = _CostAccounts;
                RaisePropertyChanged("FilteredList");
            }
        }

        public void SendSelectedToParent()
        {
            if (SelectedItem != null)
                MessengerInstance.Send<SelectedCostAccount>(new SelectedCostAccount() { AccountingType = AccountingType, CostAccount = SelectedItem });
        }

        #endregion Methods

        #region Properties

        public List<CostAccount> FilteredList { get; set; }
        public RelayCommand RefreshCommand { get; }
        public RelayCommand SelectedCommand { get; }

        public string Filter
        {
            get { return _Filter; }
            set { if (_Filter == value) return; _Filter = value; RaisePropertyChanged(); FilterList(); }
        }

        public CostAccount SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; RaisePropertyChanged(); }
        }

        public AccountingType AccountingType { get; set; }

        public Action CloseAction { get; set; }

        #endregion Properties
    }
}
