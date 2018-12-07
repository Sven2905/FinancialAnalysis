using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class KontenrahmenViewModel : ViewModelBase
    {
        #region Fields

        private CostAccount _SelectedItem;
        private string _Filter;
        private List<CostAccount> _CostAccounts = new List<CostAccount>();

        #endregion Fields

        #region Constructor

        public KontenrahmenViewModel()
        {
            RefreshCostAccounts();
            RefreshCommand = new DelegateCommand(() =>
            {
                RefreshCostAccounts();
            });
            SelectedCommand = new DelegateCommand(() =>
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
            DataLayer db = new DataLayer();
            _CostAccounts = db.CostAccounts.GetAll().ToList();
        }

        private void FilterList()
        {
            if (!string.IsNullOrEmpty(Filter))
            {
                FilteredList = _CostAccounts.Where(x => x.Description.ToLower().Contains(Filter.ToLower())).ToList();
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
                Messenger.Default.Send(new SelectedCostAccount() { AccountingType = AccountingType, CostAccount = SelectedItem });
        }

        #endregion Methods

        #region Properties

        public List<CostAccount> FilteredList { get; set; }
        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand SelectedCommand { get; }

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
