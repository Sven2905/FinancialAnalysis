using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Enums;
using System;
using System.Linq;
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
                    _SelectedCostCenter.ScheduledBudget.ValueChangedEvent -= CostCenterBudget_ValueChangedEvent;
                }
                _SelectedCostCenter = value;
                if (_SelectedCostCenter != null)
                {
                    SetupLineStackedSeries2D();
                    _SelectedCostCenter.ScheduledBudget.ValueChangedEvent += CostCenterBudget_ValueChangedEvent;
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
        public SvenTechCollection<TextValuePoint> ScheduldedCostPoints { get; private set; }
        public SvenTechCollection<TextValuePoint> CurrentCostPoints { get; private set; }
        public CostCenterBudget CostCenterCurrentBudget { get; set; } = new CostCenterBudget();

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
            ScheduldedCostPoints = new SvenTechCollection<TextValuePoint>
            {
                new TextValuePoint("Januar", (double)SelectedCostCenter.ScheduledBudget.January),
                new TextValuePoint("Februar", (double)SelectedCostCenter.ScheduledBudget.February),
                new TextValuePoint("März", (double)SelectedCostCenter.ScheduledBudget.March),
                new TextValuePoint("April", (double)SelectedCostCenter.ScheduledBudget.April),
                new TextValuePoint("Mai", (double)SelectedCostCenter.ScheduledBudget.May),
                new TextValuePoint("Juni", (double)SelectedCostCenter.ScheduledBudget.June),
                new TextValuePoint("Juli", (double)SelectedCostCenter.ScheduledBudget.July),
                new TextValuePoint("August", (double)SelectedCostCenter.ScheduledBudget.August),
                new TextValuePoint("September", (double)SelectedCostCenter.ScheduledBudget.September),
                new TextValuePoint("Oktober", (double)SelectedCostCenter.ScheduledBudget.October),
                new TextValuePoint("November", (double)SelectedCostCenter.ScheduledBudget.November),
                new TextValuePoint("Dezember", (double)SelectedCostCenter.ScheduledBudget.December)
            };

            if (SelectedCostCenter == null || Equals(SelectedCostCenter.CostCenterId, 0))
                return;

            var currentCosts = DataContext.Instance.CostCenterBudgets.GetAnnuallyCosts(SelectedCostCenter.CostCenterId, DateTime.Now.Year).ToList();

            if (Equals(currentCosts.Count, 0))
                return;

            CostCenterCurrentBudget.Year = DateTime.Now.Year;

            foreach (var item in currentCosts)
            {
                switch (item.MonthIndex)
                {
                    case Months.January:
                        CostCenterCurrentBudget.January = item.Amount;
                        break;
                    case Months.February:
                        CostCenterCurrentBudget.February = item.Amount;
                        break;
                    case Months.March:
                        CostCenterCurrentBudget.March = item.Amount;
                        break;
                    case Months.April:
                        CostCenterCurrentBudget.April = item.Amount;
                        break;
                    case Months.May:
                        CostCenterCurrentBudget.May = item.Amount;
                        break;
                    case Months.June:
                        CostCenterCurrentBudget.June = item.Amount;
                        break;
                    case Months.July:
                        CostCenterCurrentBudget.July = item.Amount;
                        break;
                    case Months.August:
                        CostCenterCurrentBudget.August = item.Amount;
                        break;
                    case Months.September:
                        CostCenterCurrentBudget.September = item.Amount;
                        break;
                    case Months.October:
                        CostCenterCurrentBudget.October = item.Amount;
                        break;
                    case Months.November:
                        CostCenterCurrentBudget.November = item.Amount;
                        break;
                    case Months.December:
                        CostCenterCurrentBudget.December = item.Amount;
                        break;
                    default:
                        break;
                }

                CurrentCostPoints = new SvenTechCollection<TextValuePoint>
                {
                    new TextValuePoint("Januar", (double)CostCenterCurrentBudget.January),
                    new TextValuePoint("Februar", (double)CostCenterCurrentBudget.February),
                    new TextValuePoint("März", (double)CostCenterCurrentBudget.March),
                    new TextValuePoint("April", (double)CostCenterCurrentBudget.April),
                    new TextValuePoint("Mai", (double)CostCenterCurrentBudget.May),
                    new TextValuePoint("Juni", (double)CostCenterCurrentBudget.June),
                    new TextValuePoint("Juli", (double)CostCenterCurrentBudget.July),
                    new TextValuePoint("August", (double)CostCenterCurrentBudget.August),
                    new TextValuePoint("September", (double)CostCenterCurrentBudget.September),
                    new TextValuePoint("Oktober", (double)CostCenterCurrentBudget.October),
                    new TextValuePoint("November", (double)CostCenterCurrentBudget.November),
                    new TextValuePoint("Dezember", (double)CostCenterCurrentBudget.December)
                };
            }
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
                DataContext.Instance.CostCenterBudgets.Update(SelectedCostCenter.ScheduledBudget);
            }
            else
            {
                int id = DataContext.Instance.CostCenters.Insert(SelectedCostCenter);
                DataContext.Instance.CostCenterBudgets.Insert(SelectedCostCenter.ScheduledBudget);
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
            CostCenterCategories = DataContext.Instance.CostCenterCategories.GetAll().ToSvenTechCollection();
            SelectedCostCenter.CostCenterCategory = SelectedCostCenterCategory.CostCenterCategory;
            SelectedCostCenter.RefCostCenterCategoryId =
                SelectedCostCenterCategory.CostCenterCategory.CostCenterCategoryId;
            RaisePropertyChanged("SelectedCostCenter");
        }

        #endregion Methods
    }
}