using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.Accounting;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModel
{
    public class CostAccountViewModel : ViewModelBase
    {
        private ObservableCollection<CostAccount> _CostAccounts = new ObservableCollection<CostAccount>();
        private CostAccountCategory _SelectedCategory;

        public CostAccount SelectedCostAccount { get; set; }
        public ObservableCollection<CostAccountCategory> CostAccountCategories { get; set; }
        public ObservableCollection<TaxType> TaxTypes { get; set; }
        public ObservableCollection<CostAccount> FilteredCostAccounts { get; set; } = new ObservableCollection<CostAccount>();
        public CostAccountCategory SelectedCategory {
            get
            {
                return _SelectedCategory;
            }
            set
            {
                if (_SelectedCategory == value) return;
                _SelectedCategory = value;
                FilterCostAccounts();
            }
        }

        public CostAccountViewModel()
        {
            RefreshLists();
        }

        public void RefreshLists()
        {
            DataLayer db = new DataLayer();
            CostAccountCategories = db.CostAccountCategories.GetAll().ToOberservableCollection();
            TaxTypes = db.TaxTypes.GetAll().ToOberservableCollection();
            _CostAccounts = db.CostAccounts.GetAll().ToOberservableCollection();
        }

        private void FilterCostAccounts()
        {
            FilteredCostAccounts.Clear();
            var ids = GetChildIds(SelectedCategory.Id).ToList();
            if (ids is null)
                return;
            ids.Add(SelectedCategory.Id);
            FilteredCostAccounts.AddRange(_CostAccounts.Where(x => ids.Contains(x.RefCostAccountCategoryId)));
        }

        private IEnumerable<int> GetChildIds(int motherId)
        {
            List<int> result = new List<int>();
            var ids = CostAccountCategories.Where(x => x.ParentCategoryId == motherId).Select(x => x.Id);
            result.AddRange(ids);
            if (ids.Any())
            {
                foreach (var id in ids)
                {
                    result.AddRange(GetChildIds(id));
                }
            }
            return result;
        }
    }
}
