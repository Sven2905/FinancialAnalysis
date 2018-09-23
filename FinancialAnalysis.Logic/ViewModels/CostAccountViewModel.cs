using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.Accounting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class CostAccountViewModel : ViewModelBase
    {
        #region Fields

        private SvenTechCollection<CostAccount> _CostAccounts = new SvenTechCollection<CostAccount>();
        private SvenTechCollection<CostAccountCategory> _CostAccountCategories = new SvenTechCollection<CostAccountCategory>();
        private CostAccountCategory _SelectedCategory;
        private CostAccount _SelectedCostAccount;
        private DataLayer db = new DataLayer();

        #endregion Fields

        #region Constructur

        public CostAccountViewModel()
        {
            RefreshLists();
        }

        #endregion Constructor

        #region Methods

        public void RefreshLists()
        {
            CostAccountCategories = db.CostAccountCategories.GetAll().ToSvenTechCollection();
            CostAccountCategoriesHierachical = CostAccountCategories.ToHierachicalCollection<CostAccountCategory>().ToSvenTechCollection();
            TaxTypes = db.TaxTypes.GetAll().ToSvenTechCollection();
            _CostAccounts = db.CostAccounts.GetAll().ToSvenTechCollection();
        }

        private void FilterCostAccounts()
        {
            FilteredCostAccounts.Clear();
            if (SelectedCategory.IsNull())
            {
                return;
            }

            var ids = GetChildIds(SelectedCategory.CostAccountCategoryId).ToList();
            if (ids.IsNull())
            {
                return;
            }

            ids.Add(SelectedCategory.CostAccountCategoryId);
            FilteredCostAccounts.AddRange(_CostAccounts.Where(x => ids.Contains(x.RefCostAccountCategoryId)));
        }

        private IEnumerable<int> GetChildIds(int motherId)
        {
            List<int> result = new List<int>();
            var ids = CostAccountCategories.Where(x => x.ParentCategoryId == motherId).Select(x => x.CostAccountCategoryId);
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

        #endregion Methods

        #region Properties

        public CostAccount SelectedCostAccount
        {
            get { return _SelectedCostAccount; }
            set
            {
                if (_SelectedCostAccount != null)
                {
                    _SelectedCostAccount.PropertyChanged -= _SelectedCostAccount_PropertyChanged;
                }

                _SelectedCostAccount = value; _SelectedCostAccount.PropertyChanged += _SelectedCostAccount_PropertyChanged;
            }
        }

        private void _SelectedCostAccount_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            db.CostAccounts.Update(SelectedCostAccount);
        }

        public SvenTechCollection<CostAccountCategory> CostAccountCategoriesHierachical { get; set; }
        public SvenTechCollection<TaxType> TaxTypes { get; set; }
        public SvenTechCollection<CostAccount> FilteredCostAccounts { get; set; } = new SvenTechCollection<CostAccount>();
        public CostAccountCategory SelectedCategory
        {
            get { return _SelectedCategory; }
            set { _SelectedCategory = value; FilterCostAccounts(); }
        }

        public SvenTechCollection<CostAccountCategory> CostAccountCategories { get => _CostAccountCategories; set => _CostAccountCategories = value; }

        #endregion Properties

    }
}
