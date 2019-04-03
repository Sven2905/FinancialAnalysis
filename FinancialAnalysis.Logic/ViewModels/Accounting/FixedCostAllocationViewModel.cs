using DevExpress.Mvvm;

using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Logic.ViewModels.Accounting;
using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using WebApiWrapper.Accounting;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class FixedCostAllocationViewModel : ViewModelBase
    {
        #region Constructor

        public FixedCostAllocationViewModel()
        {
            if (IsInDesignMode)
                return;

            GetData();
            InitializeCommands();
        }

        #endregion

        #region Methods

        private void GetData()
        {
            CostCenterCategoryList = CostCenterCategories.GetAll().ToSvenTechCollection();
            FixedCostAllocationList = FixedCostAllocations.GetAll().ToSvenTechCollection();
        }

        private void InitializeCommands()
        {
            AddFixedCostAllocationCommand = new DelegateCommand(AddFixedCostAllocation,
                () => SelectedFixedCostAllocation?.FixedCostAllocationDetails.Count > 0 && SelectedFixedCostAllocation.FixedCostAllocationDetails.All(x => x.CostCenter != null));

            DeleteFixedCostAllocationCommand = new DelegateCommand(DeleteFixedCostAllocation,
                () => SelectedFixedCostAllocation != null);
        }

        public void AddFixedCostAllocation()
        {
            SelectedFixedCostAllocation = new FixedCostAllocation();
            FixedCostAllocationList.Add(SelectedFixedCostAllocation);
        }

        public void SaveFixedCostAllocation()
        {
            if (SelectedFixedCostAllocation.FixedCostAllocationId > 0)
            {
                FixedCostAllocations.Update(SelectedFixedCostAllocation);
                foreach (var item in SelectedFixedCostAllocation.FixedCostAllocationDetails)
                {
                    if (item.RefFixedCostAllocationId == 0)
                    {
                        item.RefFixedCostAllocationId = SelectedFixedCostAllocation.FixedCostAllocationId;
                        FixedCostAllocationDetails.Insert(item);
                    }
                    else
                    {
                        FixedCostAllocationDetails.Update(item);
                    }
                }
            }
            else
            {
                int id = FixedCostAllocations.Insert(SelectedFixedCostAllocation);
                foreach (var item in SelectedFixedCostAllocation.FixedCostAllocationDetails)
                {
                    item.RefFixedCostAllocationId = id;
                    FixedCostAllocationDetails.Insert(item);
                }
            }
        }

        public void DeleteFixedCostAllocation()
        {
            if (SelectedFixedCostAllocation.FixedCostAllocationId != 0)
            {
                FixedCostAllocations.Delete(SelectedFixedCostAllocation.FixedCostAllocationId);
            }
            FixedCostAllocationList.Remove(SelectedFixedCostAllocation);
        }

        public void AddFixedCostAllocationDetail()
        {
            SelectedFixedCostAllocation?.FixedCostAllocationDetails.Add(new FixedCostAllocationDetail());
        }

        public void DeleteFixedCostAllocationDetail()
        {
            SelectedFixedCostAllocation?.FixedCostAllocationDetails.Remove(SelectedFixedCostAllocationDetail);
        }

        #endregion Methods

        #region Properties

        public DelegateCommand AddFixedCostAllocationCommand { get; set; }
        public DelegateCommand DeleteFixedCostAllocationCommand { get; set; }
        public DelegateCommand SaveFixedCostAllocationCommand { get; set; }
        public DelegateCommand AddFixedCostAllocationDetailCommand { get; set; }
        public DelegateCommand DeleteFixedCostAllocationDetailCommand { get; set; }

        public SvenTechCollection<CostCenterCategory> CostCenterCategoryList { get; set; }
        public FixedCostAllocation SelectedFixedCostAllocation { get; set; }
        public FixedCostAllocationDetail SelectedFixedCostAllocationDetail { get; set; }
        public SvenTechCollection<FixedCostAllocation> FixedCostAllocationList { get; set; }

        #endregion Methods
    }
}
