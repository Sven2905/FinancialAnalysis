﻿using DevExpress.Mvvm;

using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Logic.ViewModels.Accounting;
using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        #endregion Constructor

        #region Methods

        private void GetData()
        {
            CostCenterCategoryList = CostCenterCategories.GetAll().ToSvenTechCollection();
            FixedCostAllocationList = FixedCostAllocations.GetAll().ToSvenTechCollection();

            foreach (var fixedCostAllocation in FixedCostAllocationList)
            {
                foreach (var detail in fixedCostAllocation.FixedCostAllocationDetails)
                {
                    detail.CostCenter.CostCenterCategory = CostCenterCategoryList.Single(x => x.CostCenterCategoryId == detail.CostCenter.RefCostCenterCategoryId);
                }
            }
        }

        private void InitializeCommands()
        {
            AddFixedCostAllocationCommand = new DelegateCommand(AddFixedCostAllocation);

            SaveFixedCostAllocationCommand = new DelegateCommand(SaveFixedCostAllocation,
                () => SelectedFixedCostAllocation?.FixedCostAllocationDetails.Count > 0 && SelectedFixedCostAllocation.FixedCostAllocationDetails.All(x => x.CostCenter != null));

            DeleteFixedCostAllocationCommand = new DelegateCommand(DeleteFixedCostAllocation,
                () => SelectedFixedCostAllocation != null);

            AddFixedCostAllocationDetailCommand = new DelegateCommand(AddFixedCostAllocationDetail, () => SelectedFixedCostAllocation != null && SelectedCostCenter != null && Shares > 0);

            SaveFixedCostAllocationDetailCommand = new DelegateCommand(SaveFixedCostAllocationDetail, () => SelectedFixedCostAllocationDetail != null);

            DeleteFixedCostAllocationDetailCommand = new DelegateCommand(DeleteFixedCostAllocationDetail, () => SelectedFixedCostAllocationDetail != null);
        }

        public void AddFixedCostAllocation()
        {
            FixedCostAllocationList.Add(new FixedCostAllocation() { Name = "Neuer Schlüssel" });
            SelectedFixedCostAllocation = FixedCostAllocationList.Last(x => x.Name == "Neuer Schlüssel");
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
            if (DoesCostCenterAlreadyExist(SelectedCostCenter.CostCenterId))
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Fehler", "Kostenstelle ist bereits enhalten.", MessageBoxImage.Asterisk));
                return;
            }

            FixedCostAllocationDetail newFixedCostAllocationDetail = new FixedCostAllocationDetail()
            {
                CostCenter = SelectedCostCenter,
                RefCostCenterId = SelectedCostCenter.CostCenterId,
                Shares = Shares
            };

            newFixedCostAllocationDetail.CostCenter.CostCenterCategory = SelectedCostCenterCategory;
            SelectedFixedCostAllocation?.FixedCostAllocationDetails.Add(newFixedCostAllocationDetail);
        }

        public void SaveFixedCostAllocationDetail()
        {
            if (DoesCostCenterAlreadyExist(SelectedCostCenter.CostCenterId))
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Fehler", "Kostenstelle ist bereits enhalten.", MessageBoxImage.Asterisk));
                return;
            }

            SelectedFixedCostAllocationDetail.CostCenter = SelectedCostCenter;
            SelectedFixedCostAllocationDetail.RefCostCenterId = SelectedCostCenter.CostCenterId;
            SelectedFixedCostAllocationDetail.CostCenter.CostCenterCategory = SelectedCostCenterCategory;
            SelectedFixedCostAllocationDetail.Shares = Shares;

            if (SelectedFixedCostAllocationDetail.FixedCostAllocationDetailId > 0)
            {
                FixedCostAllocationDetails.Update(SelectedFixedCostAllocationDetail);
            }
        }

        public void DeleteFixedCostAllocationDetail()
        {
            var itemToRemove = SelectedFixedCostAllocation?.FixedCostAllocationDetails.Single(x => x.CostCenter == SelectedFixedCostAllocationDetail.CostCenter);
            SelectedFixedCostAllocation?.FixedCostAllocationDetails.Remove(itemToRemove);
        }

        private bool DoesCostCenterAlreadyExist(int costCenterId)
        {
            return SelectedFixedCostAllocation.FixedCostAllocationDetails.SingleOrDefault(x => x.RefCostCenterId == costCenterId) != null;
        }

        #endregion Methods

        #region Properties

        public DelegateCommand AddFixedCostAllocationCommand { get; set; }
        public DelegateCommand DeleteFixedCostAllocationCommand { get; set; }
        public DelegateCommand SaveFixedCostAllocationCommand { get; set; }
        public DelegateCommand AddFixedCostAllocationDetailCommand { get; set; }
        public DelegateCommand DeleteFixedCostAllocationDetailCommand { get; set; }
        public DelegateCommand SaveFixedCostAllocationDetailCommand { get; set; }
        public SvenTechCollection<CostCenterCategory> CostCenterCategoryList { get; set; }
        public CostCenterCategory SelectedCostCenterCategory { get; set; }
        public CostCenter SelectedCostCenter { get; set; }
        public int Shares { get; set; }

        public FixedCostAllocation SelectedFixedCostAllocation { get; set; }
        public FixedCostAllocationDetail SelectedFixedCostAllocationDetail { get; set; }
        public SvenTechCollection<FixedCostAllocation> FixedCostAllocationList { get; set; }

        #endregion Methods
    }
}
