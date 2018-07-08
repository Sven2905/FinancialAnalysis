using FinancialAnalysis.Logic.Models;
using FinancialAnalysis.Logic.Models.Accounting;
using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FinancialAnalysis.Logic.Model.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<CostAccountCategory> CostAccountCategories { get; set; }
        public ObservableCollection<CostAccount> CostAccounts { get; set; }
        private CostAccountCategory _SelectedItem;
        public CostAccountCategory SelectedItem { get { return _SelectedItem; } set { _SelectedItem = value; FillCostAccounts(); } }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            CostAccountCategories = new ObservableCollection<CostAccountCategory>();
            CostAccounts = new ObservableCollection<CostAccount>();
            Seeder.SeedAll();
            FillCostAccountCategories();
            Import.ImportKontenrahmen();
        }

        private void FillCostAccountCategories()
        {
            FinanceContext ctx = new FinanceContext();
            foreach (var item in ctx.CostAccountCategories)
            {
                CostAccountCategories.Add(item);
            }
        }

        private void FillCostAccounts()
        {
            CostAccounts.Clear();
            FinanceContext ctx = new FinanceContext();
            var accounts = ctx.CostAccounts.Where(x => x.CostAccountCategoryId == SelectedItem.CostAccountCategoryId).ToList();
            foreach (var item in accounts)
            {
                CostAccounts.Add(item);
            }
            
        }

    }
}