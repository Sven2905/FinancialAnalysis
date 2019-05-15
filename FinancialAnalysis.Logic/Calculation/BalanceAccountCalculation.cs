using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using WebApiWrapper.Accounting;

namespace FinancialAnalysis.Logic.Calculation
{
    public class BalanceAccountCalculation : BindableBase
    {
        public SvenTechCollection<BalanceAccountResultItem> ActiveAccountList { get; set; }
        public SvenTechCollection<BalanceAccountResultDetailItem> ActiveAccountDetailedList { get; set; }
        public SvenTechCollection<BalanceAccountResultItem> PassiveAccountList { get; set; }
        public SvenTechCollection<BalanceAccountResultDetailItem> PassiveAccountDetailedList { get; set; }
        public decimal SumActiveAccounts
        {
            get
            {
                if (ActiveAccountList == null)
                {
                    return 0;
                }
                //return ActiveAccountList.Where(x => x.ParentId == 0).Sum(x => x.Amount);
                return ActiveAccountList.Sum(x => x.Amount);
            }
        }
        public decimal SumPassiveAccounts
        {
            get
            {
                if (PassiveAccountList == null)
                {
                    return 0;
                }
                //return PassiveAccountList.Where(x => x.ParentId == 0).Sum(x => x.Amount);
                return PassiveAccountList.Sum(x => x.Amount);
            }
        }

        public void GetAndCalculateData(DateTime StartDate, DateTime EndDate)
        {
            var newEndDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 59);
            ActiveAccountList = BalanceAccounts.GetActiveAccounts(StartDate, newEndDate).ToSvenTechCollection();
            //ActiveAccountDetailedList = BalanceAccounts.GetActiveAccountsDetailed(StartDate, newEndDate).ToSvenTechCollection();
            PassiveAccountList = BalanceAccounts.GetPassiveAccounts(StartDate, newEndDate).ToSvenTechCollection();
            //PassiveAccountDetailedList = BalanceAccounts.GetPassiveAccountsDetailed(StartDate, newEndDate).ToSvenTechCollection();

            //CalculateSums();
            //CalculateCompensation();
            //GetParentCategories();

            RaisePropertyChanged("SumActiveAccounts");
            RaisePropertyChanged("SumPassiveAccounts");
            //RaisePropertyChanged("ActiveAccountDetailedList");
            //RaisePropertyChanged("PassiveAccountDetailedList");
        }

        private void CalculateCompensation()
        {
            if (SumActiveAccounts > SumPassiveAccounts)
            {

            }
            else if(SumPassiveAccounts < SumPassiveAccounts)
            {

            }
        }

        private void CalculateSums()
        {
            for (int i = ActiveAccountList.Count - 1; i >= 0; i--)
            {
                if (ActiveAccountList.Any(x => x.ParentId == ActiveAccountList[i].BalanceAccountId))
                {
                    ActiveAccountList[i].Amount += GetSumOfChilds(ActiveAccountList[i].BalanceAccountId, ActiveAccountList);
                }
            }

            for (int i = PassiveAccountList.Count - 1; i >= 0; i--)
            {
                if (PassiveAccountList.Any(x => x.ParentId == PassiveAccountList[i].BalanceAccountId))
                {
                    PassiveAccountList[i].Amount += GetSumOfChilds(PassiveAccountList[i].BalanceAccountId, PassiveAccountList);
                }
            }
        }

        private decimal GetSumOfChilds(int parentId, SvenTechCollection<BalanceAccountResultItem> collection)
        {
            return collection.Where(x => x.ParentId == parentId).Sum(x => x.Amount);
        }

        private void GetParentCategories()
        {
            var allCategories = BalanceAccounts.GetAll();
            var parent = new BalanceAccount();
            var mainParent = new BalanceAccount();

            foreach (var item in ActiveAccountDetailedList)
            {
                if (item.ParentId != 0)
                {
                    parent = allCategories.Single(x => x.BalanceAccountId == item.ParentId);
                    item.Sub1CategoryName = parent.Name;
                    if (parent.ParentId != 0)
                    {
                        mainParent = allCategories.Single(x => x.BalanceAccountId == parent.ParentId);
                        item.MainCategoryName = mainParent.Name;
                    }
                }
            }

            foreach (var item in PassiveAccountDetailedList)
            {
                if (item.ParentId != 0)
                {
                    parent = allCategories.Single(x => x.BalanceAccountId == item.ParentId);
                    item.Sub1CategoryName = parent.Name;
                    if (parent.ParentId != 0)
                    {
                        mainParent = allCategories.Single(x => x.BalanceAccountId == parent.ParentId);
                        item.MainCategoryName = mainParent.Name;
                    }
                }
            }
        }
    }
}
