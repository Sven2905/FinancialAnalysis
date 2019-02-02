using System;
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
            if (IsInDesignMode)
                return;

            Messenger.Default.Register<SelectedCostCenterCategory>(this, ChangeSelectedCostCenterCategory);

            SetCommands();
            LoadCostCenters();
        }

        #endregion Constructor

        #region Fields



        #endregion Fields

        #region Properties

        public SvenTechCollection<CostCenter> CostCenters { get; set; }
        public SvenTechCollection<CostCenterCategory> CostCenterCategories { get; set; }
        public CostCenter SelectedCostCenter { get; set; }
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
            DeleteCostCenterCommand = new DelegateCommand(DeleteCostCenter, () => (SelectedCostCenter != null));
            OpenCostCenterCategoriesWindowCommand = new DelegateCommand(OpenCostCenterCategoriesWindow);
        }

        private void OpenCostCenterCategoriesWindow()
        {
            Messenger.Default.Send(new OpenCostCenterCategoriesWindowMessage());
        }

        private void LoadCostCenters()
        {
            try
            {
                CostCenters = DataLayer.Instance.CostCenters.GetAll().ToSvenTechCollection();
                CostCenterCategories = DataLayer.Instance.CostCenterCategories.GetAll().ToSvenTechCollection();
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void NewCostCenter()
        {
            SelectedCostCenter = new CostCenter();
            CostCenters.Add(SelectedCostCenter);
        }

        private void DeleteCostCenter()
        {
            if (SelectedCostCenter == null)
            {
                return;
            }

            if (SelectedCostCenter.CostCenterId == 0)
            {
                CostCenters.Remove(SelectedCostCenter);
                SelectedCostCenter = null;
                return;
            }

            try
            {
                DataLayer.Instance.CostCenters.Delete(SelectedCostCenter.CostCenterId);
                CostCenters.Remove(SelectedCostCenter);
                SelectedCostCenter = null;
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void SaveCostCenter()
        {
            try
            {
                if (SelectedCostCenter.CostCenterId != 0)
                    DataLayer.Instance.CostCenters.Update(SelectedCostCenter);
                else
                    DataLayer.Instance.CostCenters.Insert(SelectedCostCenter);

            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private bool Validation()
        {
            if (SelectedCostCenter == null)
            {
                return false;
            }
            return !string.IsNullOrEmpty(SelectedCostCenter.Name);
        }

        private void ChangeSelectedCostCenterCategory(SelectedCostCenterCategory SelectedCostCenterCategory)
        {
            CostCenterCategories = DataLayer.Instance.CostCenterCategories.GetAll().ToSvenTechCollection();
            SelectedCostCenter.CostCenterCategory = SelectedCostCenterCategory.CostCenterCategory;
            SelectedCostCenter.RefCostCenterCategoryId = SelectedCostCenterCategory.CostCenterCategory.CostCenterCategoryId;
            RaisePropertyChanged("SelectedCostCenter");
        }

        #endregion Methods
    }
}