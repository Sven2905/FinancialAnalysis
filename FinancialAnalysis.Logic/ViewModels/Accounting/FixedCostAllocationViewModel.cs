﻿using DevExpress.Mvvm;

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
                () => (SelectedFixedCostAllocation != null && SelectedFixedCostAllocation.Shares > 0 && SelectedFixedCostAllocation.CostCenter != null));

            DeleteFixedCostAllocationCommand = new DelegateCommand(DeleteFixedCostAllocation, 
                () => (SelectedFixedCostAllocationTable != null && SelectedFixedCostAllocationTable.FixedCostAllocationId > 0));
        }

        public void AddFixedCostAllocation()
        {
            if (FixedCostAllocationList.Any(x => x.RefCostCenterId == SelectedFixedCostAllocation.CostCenter.CostCenterId))
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Hinweis", "Die Kostenstelle ist bereits enthalten.", System.Windows.MessageBoxImage.Asterisk));
                return;
            }

            SelectedFixedCostAllocation.RefCostCenterId = SelectedFixedCostAllocation.CostCenter.CostCenterId;

            SelectedFixedCostAllocation.FixedCostAllocationId = FixedCostAllocations.Insert(SelectedFixedCostAllocation);
            SelectedFixedCostAllocation.CostCenter.CostCenterCategory = CostCenterCategoryList.Single(x => x.CostCenterCategoryId == SelectedFixedCostAllocation.CostCenter.RefCostCenterCategoryId);
            FixedCostAllocationList.Add(SelectedFixedCostAllocation);
            SelectedFixedCostAllocation = new FixedCostAllocation();
        }

        public void DeleteFixedCostAllocation()
        {
            FixedCostAllocations.Delete(SelectedFixedCostAllocationTable.FixedCostAllocationId);
            FixedCostAllocationList.Remove(SelectedFixedCostAllocationTable);
        }

        #endregion Methods

        #region Properties

        public DelegateCommand AddFixedCostAllocationCommand { get; set; }
        public DelegateCommand DeleteFixedCostAllocationCommand { get; set; }
        public SvenTechCollection<CostCenterCategory> CostCenterCategoryList { get; set; }
        public CostCenterCategory SelectedCostCenterCategory { get; set; }
        public FixedCostAllocation SelectedFixedCostAllocation { get; set; } = new FixedCostAllocation();
        public FixedCostAllocation SelectedFixedCostAllocationTable { get; set; }
        public SvenTechCollection<FixedCostAllocation> FixedCostAllocationList { get; set; }

        #endregion Methods
    }
}
