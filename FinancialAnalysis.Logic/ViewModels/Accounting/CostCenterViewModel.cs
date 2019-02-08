using System;
using System.Windows;
using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Accounting;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class CostCenterViewModel : ViewModelBase
    {
        #region Constructor

        public CostCenterViewModel()
        {
            if (IsInDesignMode) return;

            Messenger.Default.Register<SelectedCostCenterCategory>(this, ChangeSelectedCostCenterCategory);

            SetCommands();
            LoadCostCenters();
        }

        #endregion Constructor

        #region Properties

        public SvenTechCollection<CostCenter> CostCenters { get; set; }
        public SvenTechCollection<CostCenterCategory> CostCenterCategories { get; set; }
        public SvenTechCollection<CostCenterCategoryFlatStructure> CostCenterCategoriesFlatStructure { get; set; } = new SvenTechCollection<CostCenterCategoryFlatStructure>();
        public CostCenterCategoryFlatStructure SelectedCostCenterCategoryFlatStructure { get; set; }

        public DelegateCommand NewCostCenterCommand { get; set; }
        public DelegateCommand SaveCostCenterCommand { get; set; }
        public DelegateCommand DeleteCostCenterCommand { get; set; }
        public DelegateCommand OpenCostCenterCategoriesWindowCommand { get; set; }

        #endregion Properties

        #region Methods

        private void SetCommands()
        {
            NewCostCenterCommand = new DelegateCommand(NewCostCenter);
            SaveCostCenterCommand = new DelegateCommand(SaveCostCenter, () => Validation());
            DeleteCostCenterCommand = new DelegateCommand(DeleteCostCenter, () => SelectedCostCenterCategoryFlatStructure != null);
            OpenCostCenterCategoriesWindowCommand = new DelegateCommand(OpenCostCenterCategoriesWindow);
        }

        private void OpenCostCenterCategoriesWindow()
        {
            Messenger.Default.Send(new OpenCostCenterCategoriesWindowMessage());
        }

        private void LoadCostCenters()
        {
            CostCenters = DataContext.Instance.CostCenters.GetAll().ToSvenTechCollection();
            CostCenterCategories = DataContext.Instance.CostCenterCategories.GetAll().ToSvenTechCollection();
            CreateFlatCostCenterCategoriesList();
        }

        private void NewCostCenter()
        {
            SelectedCostCenterCategoryFlatStructure = new CostCenterCategoryFlatStructure();
            CostCenters.Add(SelectedCostCenterCategoryFlatStructure.CostCenter);
            CostCenterCategoriesFlatStructure.Add(SelectedCostCenterCategoryFlatStructure);
        }

        private void DeleteCostCenter()
        {
            if (SelectedCostCenterCategoryFlatStructure == null) return;

            if (SelectedCostCenterCategoryFlatStructure.CostCenter.CostCenterId == 0)
            {
                CostCenters.Remove(SelectedCostCenterCategoryFlatStructure.CostCenter);
                SelectedCostCenterCategoryFlatStructure.CostCenter = null;
                return;
            }

            DataContext.Instance.CostCenters.Delete(SelectedCostCenterCategoryFlatStructure.CostCenter.CostCenterId);
            CostCenters.Remove(SelectedCostCenterCategoryFlatStructure.CostCenter);
            SelectedCostCenterCategoryFlatStructure.CostCenter = null;
            LoadCostCenters();
        }

        private void SaveCostCenter()
        {
            if (SelectedCostCenterCategoryFlatStructure.CostCenter.CostCenterId != 0)
                DataContext.Instance.CostCenters.Update(SelectedCostCenterCategoryFlatStructure.CostCenter);
            else
                DataContext.Instance.CostCenters.Insert(SelectedCostCenterCategoryFlatStructure.CostCenter);
            LoadCostCenters();
        }

        private bool Validation()
        {
            if (SelectedCostCenterCategoryFlatStructure.CostCenter == null) return false;
            return !string.IsNullOrEmpty(SelectedCostCenterCategoryFlatStructure.CostCenter.Name);
        }

        private void CreateFlatCostCenterCategoriesList()
        {
            int id = 0;
            CostCenterCategoriesFlatStructure.Clear();

            foreach (var category in CostCenterCategories)
            {
                CostCenterCategoriesFlatStructure.Add(new CostCenterCategoryFlatStructure()
                {
                    Id = category.CostCenterCategoryId,
                    ParentId = 0,
                    CostCenterCategory = category,
                    CostCenter = null
                });
                id = category.CostCenterCategoryId + 1;
            }

            foreach (var category in CostCenterCategories)
            {
                foreach (var costCenter in category.CostCenters)
                {
                    CostCenterCategoriesFlatStructure.Add(new CostCenterCategoryFlatStructure()
                    {
                        Id = id,
                        ParentId = costCenter.RefCostCenterCategoryId,
                        CostCenterCategory = null,
                        CostCenter = costCenter
                    });
                    id++;
                }
            }
        }

        private void ChangeSelectedCostCenterCategory(SelectedCostCenterCategory SelectedCostCenterCategory)
        {
            CostCenterCategories = DataContext.Instance.CostCenterCategories.GetAll().ToSvenTechCollection();
            SelectedCostCenter.CostCenterCategory = SelectedCostCenterCategory.CostCenterCategory;
            SelectedCostCenter.RefCostCenterCategoryId =
                SelectedCostCenterCategory.CostCenterCategory.CostCenterCategoryId;
            RaisePropertyChanged("SelectedCostCenter");
        }

        #endregion Methods
    }
}