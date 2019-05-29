using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Utilities;
using WebApiWrapper.Accounting;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class CostAccountViewModel : ViewModelBase
    {
        #region Constructur

        public CostAccountViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            RefreshLists();
        }

        #endregion Constructur

        #region Fields

        private SvenTechCollection<CostAccount> _CostAccounts = new SvenTechCollection<CostAccount>();
        private CostAccountCategory _SelectedCategory;
        private CostAccount _SelectedCostAccount;

        #endregion Fields

        #region Methods

        public void RefreshLists()
        {
            CostAccountCategoryList = CostAccountCategories.GetAll().ToSvenTechCollection();
            CostAccountCategoriesHierachical = CostAccountCategoryList.ToHierachicalCollection<CostAccountCategory>()
                .ToSvenTechCollection();
            TaxTypeList = TaxTypes.GetAll().ToSvenTechCollection();
            _CostAccounts = CostAccounts.GetAll().ToSvenTechCollection();
        }

        private void FilterCostAccounts()
        {
            FilteredCostAccounts.Clear();
            if (SelectedCategory.IsNull())
            {
                return;
            }

            List<int> ids = GetChildIds(SelectedCategory.CostAccountCategoryId).ToList();
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
            IEnumerable<int> ids = CostAccountCategoryList.Where(x => x.ParentCategoryId == motherId)
                .Select(x => x.CostAccountCategoryId);
            result.AddRange(ids);
            if (ids.Any())
            {
                foreach (int id in ids)
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
            get => _SelectedCostAccount;
            set
            {
                if (_SelectedCostAccount != null)
                {
                    _SelectedCostAccount.PropertyChanged -= _SelectedCostAccount_PropertyChanged;
                }

                _SelectedCostAccount = value;
                if (_SelectedCostAccount != null)
                {
                    _SelectedCostAccount.PropertyChanged += _SelectedCostAccount_PropertyChanged;
                }
            }
        }

        private void _SelectedCostAccount_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CostAccounts.Update(SelectedCostAccount);
        }

        public SvenTechCollection<CostAccountCategory> CostAccountCategoriesHierachical { get; set; }
        public SvenTechCollection<TaxType> TaxTypeList { get; set; }

        public SvenTechCollection<CostAccount> FilteredCostAccounts { get; set; } =
            new SvenTechCollection<CostAccount>();

        public CostAccountCategory SelectedCategory
        {
            get => _SelectedCategory;
            set
            {
                _SelectedCategory = value;
                FilterCostAccounts();
            }
        }

        public SvenTechCollection<CostAccountCategory> CostAccountCategoryList { get; set; } =
            new SvenTechCollection<CostAccountCategory>();

        #endregion Properties
    }
}