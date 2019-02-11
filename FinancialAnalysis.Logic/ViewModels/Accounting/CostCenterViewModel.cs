using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models;
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
            {
                return;
            }

            Messenger.Default.Register<SelectedCostCenterCategory>(this, ChangeSelectedCostCenterCategory);

            SetCommands();
            LoadCostCenters();
        }

        #endregion Constructor

        #region Properties

        public SvenTechCollection<CostCenter> CostCenters { get; set; }
        public SvenTechCollection<CostCenterCategory> CostCenterCategories { get; set; }
        private CostCenter _SelectedCostCenter;

        public CostCenter SelectedCostCenter
        {
            get { return _SelectedCostCenter; }
            set
            {
                if (_SelectedCostCenter != null)
                {
                    _SelectedCostCenter.CostCenterBudget.ValueChangedEvent -= CostCenterBudget_ValueChangedEvent;
                }
                _SelectedCostCenter = value;
                if (_SelectedCostCenter != null)
                {
                    SetupLineStackedSeries2D();
                    _SelectedCostCenter.CostCenterBudget.ValueChangedEvent += CostCenterBudget_ValueChangedEvent;
                }
            }
        }

        private void CostCenterBudget_ValueChangedEvent()
        {
            SetupLineStackedSeries2D();
        }

        public DelegateCommand NewCostCenterCommand { get; set; }
        public DelegateCommand SaveCostCenterCommand { get; set; }
        public DelegateCommand DeleteCostCenterCommand { get; set; }
        public DelegateCommand OpenCostCenterCategoriesWindowCommand { get; set; }
        public SvenTechCollection<TextValuePoint> PointsCollection { get; private set; }

        #endregion Properties

        #region Methods

        private void SetCommands()
        {
            NewCostCenterCommand = new DelegateCommand(NewCostCenter);
            SaveCostCenterCommand = new DelegateCommand(SaveCostCenter, () => Validation());
            DeleteCostCenterCommand = new DelegateCommand(DeleteCostCenter, () => SelectedCostCenter != null);
            OpenCostCenterCategoriesWindowCommand = new DelegateCommand(OpenCostCenterCategoriesWindow);
        }

        private void SetupLineStackedSeries2D()
        {
            PointsCollection = new SvenTechCollection<TextValuePoint>
            {
                new TextValuePoint("Januar", (double)SelectedCostCenter.CostCenterBudget.January),
                new TextValuePoint("Februar", (double)SelectedCostCenter.CostCenterBudget.February),
                new TextValuePoint("März", (double)SelectedCostCenter.CostCenterBudget.March),
                new TextValuePoint("April", (double)SelectedCostCenter.CostCenterBudget.April),
                new TextValuePoint("Mai", (double)SelectedCostCenter.CostCenterBudget.May),
                new TextValuePoint("Juni", (double)SelectedCostCenter.CostCenterBudget.June),
                new TextValuePoint("Juli", (double)SelectedCostCenter.CostCenterBudget.July),
                new TextValuePoint("August", (double)SelectedCostCenter.CostCenterBudget.August),
                new TextValuePoint("September", (double)SelectedCostCenter.CostCenterBudget.September),
                new TextValuePoint("Oktober", (double)SelectedCostCenter.CostCenterBudget.October),
                new TextValuePoint("November", (double)SelectedCostCenter.CostCenterBudget.November),
                new TextValuePoint("Dezember", (double)SelectedCostCenter.CostCenterBudget.December)
            };
        }

        private void OpenCostCenterCategoriesWindow()
        {
            Messenger.Default.Send(new OpenCostCenterCategoriesWindowMessage());
        }

        private void LoadCostCenters()
        {
            CostCenters = DataContext.Instance.CostCenters.GetAll().ToSvenTechCollection();
            CostCenterCategories = DataContext.Instance.CostCenterCategories.GetAll().ToSvenTechCollection();
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

            DataContext.Instance.CostCenters.Delete(SelectedCostCenter.CostCenterId);
            CostCenters.Remove(SelectedCostCenter);
            SelectedCostCenter = null;
        }

        private void SaveCostCenter()
        {
            if (SelectedCostCenter.CostCenterId != 0)
            {
                DataContext.Instance.CostCenters.Update(SelectedCostCenter);
                DataContext.Instance.CostCenterBudgets.Update(SelectedCostCenter.CostCenterBudget);
            }
            else
            {
                int id = DataContext.Instance.CostCenters.Insert(SelectedCostCenter);
                SelectedCostCenter.CostCenterBudget.RefCostCenterId = id;
                DataContext.Instance.CostCenterBudgets.Insert(SelectedCostCenter.CostCenterBudget);
            }
        }

        private void UpdateCostCenterBudget()
        {
            DataContext.Instance.CostCenterBudgets.Update(SelectedCostCenter.CostCenterBudget);
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
            CostCenterCategories = DataContext.Instance.CostCenterCategories.GetAll().ToSvenTechCollection();
            SelectedCostCenter.CostCenterCategory = SelectedCostCenterCategory.CostCenterCategory;
            SelectedCostCenter.RefCostCenterCategoryId =
                SelectedCostCenterCategory.CostCenterCategory.CostCenterCategoryId;
            RaisePropertyChanged("SelectedCostCenter");
        }

        #endregion Methods
    }
}