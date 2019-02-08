using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.ViewModels.Accounting;
using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

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
            CostCenterCategories = DataContext.Instance.CostCenterCategories.GetAll().ToSvenTechCollection();
            FixedCostAllocations = DataContext.Instance.FixedCostAllocations.GetAll().ToSvenTechCollection();
        }

        private void InitializeCommands()
        {
            AddFixedCostAllocationCommand = new DelegateCommand(AddFixedCostAllocation, 
                () => (SelectedFixedCostAllocation != null && SelectedFixedCostAllocation.Shares > 0 && SelectedFixedCostAllocation.CostCenter != null));

            DeleteFixedCostAllocationCommand = new DelegateCommand(DeleteFixedCostAllocation, 
                () => (SelectedFixedCostAllocationTable != null && SelectedFixedCostAllocationTable.FixedCostAllocationId > 0));
        }

        public void AddFixedCostAllocation()
        {
            if (FixedCostAllocations.Any(x => x.RefCostCenterId == SelectedFixedCostAllocation.RefCostCenterId))
            {
                return;
            }

            SelectedFixedCostAllocation.RefCostCenterId = SelectedFixedCostAllocation.CostCenter.CostCenterId;

            SelectedFixedCostAllocation.FixedCostAllocationId = DataContext.Instance.FixedCostAllocations.Insert(SelectedFixedCostAllocation);
            FixedCostAllocations.Add(SelectedFixedCostAllocation);
            SelectedFixedCostAllocation = new FixedCostAllocation();
        }

        public void DeleteFixedCostAllocation()
        {
            DataContext.Instance.FixedCostAllocations.Delete(SelectedFixedCostAllocationTable);
            FixedCostAllocations.Remove(SelectedFixedCostAllocationTable);
        }

        #endregion Methods

        #region Properties

        public DelegateCommand AddFixedCostAllocationCommand { get; set; }
        public DelegateCommand DeleteFixedCostAllocationCommand { get; set; }
        public SvenTechCollection<CostCenterCategory> CostCenterCategories { get; set; }
        public CostCenterCategory SelectedCostCenterCategory { get; set; }
        public FixedCostAllocation SelectedFixedCostAllocation { get; set; } = new FixedCostAllocation();
        public FixedCostAllocation SelectedFixedCostAllocationTable { get; set; }
        public SvenTechCollection<FixedCostAllocation> FixedCostAllocations { get; set; }

        #endregion Methods
    }
}
