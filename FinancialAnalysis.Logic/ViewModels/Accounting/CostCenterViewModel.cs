using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Accounting.CostCenterManagement;
using FinancialAnalysis.Models.Enums;
using System;
using System.Linq;
using Utilities;
using WebApiWrapper.Accounting;

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

        public SvenTechCollection<CostCenter> CostCenterList { get; set; }
        public SvenTechCollection<CostCenterCategory> CostCenterCategoryList { get; set; }
        private CostCenter _SelectedCostCenter;

        public CostCenter SelectedCostCenter
        {
            get => _SelectedCostCenter;
            set
            {
                if (_SelectedCostCenter != null)
                {
                    _SelectedCostCenter.ScheduledBudget.PropertyChanged -= ScheduledBudget_PropertyChanged;
                }
                _SelectedCostCenter = value;
                if (_SelectedCostCenter != null)
                {
                    SetupLineSeries2D();
                    _SelectedCostCenter.ScheduledBudget.PropertyChanged += ScheduledBudget_PropertyChanged; ;
                }
            }
        }

        private void ScheduledBudget_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetupLineSeries2D();
        }

        public CostCenterFlatStructure SelectedCostCenterFlatStructure
        {
            set
            {
                if (value?.CostCenter != null)
                {
                    SelectedCostCenter = CostCenterList.Single(x => x.CostCenterId == value.CostCenter.CostCenterId);
                }
                else
                {
                    SelectedCostCenter = null;
                }
            }
        }


        public DelegateCommand NewCostCenterCommand { get; set; }
        public DelegateCommand SaveCostCenterCommand { get; set; }
        public DelegateCommand DeleteCostCenterCommand { get; set; }
        public DelegateCommand OpenCostCenterCategoriesWindowCommand { get; set; }
        public SvenTechCollection<TextValuePoint> ScheduldedCostPoints { get; private set; }
        public SvenTechCollection<TextValuePoint> CurrentCostPoints { get; private set; }
        public SvenTechCollection<CostCenterFlatStructure> CostCenterFlatStructures { get; private set; }
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

        private void SetupLineSeries2D()
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
            {
                return;
            }

            GetCurrentCosts();

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

        private void SetupFlatStructure()
        {
            int key = 0;
            CostCenterFlatStructures = new SvenTechCollection<CostCenterFlatStructure>();
            foreach (CostCenterCategory item in CostCenterCategoryList)
            {
                CostCenterFlatStructures.Add(new CostCenterFlatStructure()
                {
                    CostCenterCategory = item,
                    Key = key = item.CostCenterCategoryId,
                    ParentKey = 0
                });
                key++;
            }

            foreach (CostCenter item in CostCenterList)
            {
                CostCenterFlatStructures.Add(new CostCenterFlatStructure()
                {
                    CostCenter = item,
                    Key = key,
                    ParentKey = item.RefCostCenterCategoryId
                });
                key++;
            }
        }

        private void GetCurrentCosts()
        {
            System.Collections.Generic.List<CostCenterCurrentCosts> currentCosts = CostCenterBudgets.GetAnnuallyCosts(SelectedCostCenter.CostCenterId, DateTime.Now.Year).ToList();

            CostCenterCurrentBudget.Year = DateTime.Now.Year;
            if (Equals(currentCosts.Count, 0))
            {
                CostCenterCurrentBudget.Annually = 0;
            }
            else
            {

                foreach (CostCenterCurrentCosts item in currentCosts)
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
                }
            }
        }

        private void OpenCostCenterCategoriesWindow()
        {
            Messenger.Default.Send(new OpenCostCenterCategoriesWindowMessage());
        }

        private void LoadCostCenters()
        {
            CostCenterList = CostCenters.GetAll().ToSvenTechCollection();
            CostCenterCategoryList = CostCenterCategories.GetAll().ToSvenTechCollection();
            SetupFlatStructure();
        }

        private void NewCostCenter()
        {
            SelectedCostCenter = new CostCenter();
            CostCenterList.Add(SelectedCostCenter);
        }

        private void DeleteCostCenter()
        {
            if (SelectedCostCenter == null)
            {
                return;
            }

            if (SelectedCostCenter.CostCenterId == 0)
            {
                CostCenterList.Remove(SelectedCostCenter);
                SelectedCostCenter = null;
                return;
            }

            CostCenters.Delete(SelectedCostCenter.CostCenterId);
            CostCenterList.Remove(SelectedCostCenter);
            SelectedCostCenter = null;
        }

        private void SaveCostCenter()
        {
            if (SelectedCostCenter.CostCenterId != 0)
            {
                CostCenters.Update(SelectedCostCenter);
                CostCenterBudgets.Update(SelectedCostCenter.ScheduledBudget);
            }
            else
            {
                int id = CostCenters.Insert(SelectedCostCenter);
                CostCenterBudgets.Insert(SelectedCostCenter.ScheduledBudget);
            }
            SetupFlatStructure();
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
            CostCenterCategoryList = CostCenterCategories.GetAll().ToSvenTechCollection();
            SelectedCostCenter.CostCenterCategory = SelectedCostCenterCategory.CostCenterCategory;
            SelectedCostCenter.RefCostCenterCategoryId =
                SelectedCostCenterCategory.CostCenterCategory.CostCenterCategoryId;
            RaisePropertyChanged("SelectedCostCenter");
        }

        #endregion Methods
    }
}