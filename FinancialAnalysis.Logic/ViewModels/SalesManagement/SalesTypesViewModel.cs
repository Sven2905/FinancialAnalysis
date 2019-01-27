using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Windows.Media.Imaging;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class SalesTypesViewModel : ViewModelBase
    {
        #region Fields

        private readonly SalesType _SelectedSalesType;
        private SvenTechCollection<SalesType> _SalesTypes = new SvenTechCollection<SalesType>();
        private string _FilterText = string.Empty;

        #endregion Fields

        #region Constructor

        public SalesTypesViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            FilteredSalesTypes = _SalesTypes = LoadAllSalesTypes();
            NewSalesTypeCommand = new DelegateCommand(NewSalesType);
            SaveSalesTypeCommand = new DelegateCommand(SaveSalesType, () => Validation());
            DeleteSalesTypeCommand = new DelegateCommand(DeleteSalesType, () => (SelectedSalesType != null));
            SelectedCommand = new DelegateCommand(() =>
            {
                SendSelectedToParent();
                CloseAction();
            });
        }

        #endregion Constructor

        #region Methods

        private SvenTechCollection<SalesType> LoadAllSalesTypes()
        {
            SvenTechCollection<SalesType> allSalesTypes = new SvenTechCollection<SalesType>();
            try
            {
                allSalesTypes = DataLayer.Instance.SalesTypes.GetAll().ToSvenTechCollection();
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            return allSalesTypes;
        }

        private void NewSalesType()
        {
            SelectedSalesType = new SalesType();
            _SalesTypes.Add(SelectedSalesType);
        }

        private void DeleteSalesType()
        {
            if (SelectedSalesType == null)
            {
                return;
            }

            if (SelectedSalesType.SalesTypeId == 0)
            {
                _SalesTypes.Remove(SelectedSalesType);
                SelectedSalesType = null;
                return;
            }

            try
            {
                DataLayer.Instance.SalesTypes.Delete(SelectedSalesType.SalesTypeId);
                _SalesTypes.Remove(SelectedSalesType);
                SelectedSalesType = null;
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void SaveSalesType()
        {
            try
            {
                if (SelectedSalesType.SalesTypeId != 0)
                {
                    DataLayer.Instance.SalesTypes.Update(SelectedSalesType);
                }
                else
                {
                    SelectedSalesType.SalesTypeId = DataLayer.Instance.SalesTypes.Insert(SelectedSalesType);
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private bool Validation()
        {
            if (SelectedSalesType == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(SelectedSalesType.Name))
            {
                return false;
            }
            return true;
        }

        public void SendSelectedToParent()
        {
            if (SelectedSalesType == null)
            {
                return;
            }

            if (SelectedSalesType.SalesTypeId == 0)
            {
                SaveSalesType();
            }

            Messenger.Default.Send(new SelectedSalesType { SalesType = SelectedSalesType });
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<SalesType> FilteredSalesTypes { get; set; } = new SvenTechCollection<SalesType>();
        public DelegateCommand NewSalesTypeCommand { get; set; }
        public DelegateCommand SaveSalesTypeCommand { get; set; }
        public DelegateCommand DeleteSalesTypeCommand { get; set; }
        public DelegateCommand SelectedCommand { get; }

        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredSalesTypes = new SvenTechCollection<SalesType>();
                    foreach (var item in _SalesTypes)
                    {
                        if (item.Name.Contains(FilterText))
                        {
                            FilteredSalesTypes.Add(item);
                        }
                    }
                }
                else
                {
                    FilteredSalesTypes = _SalesTypes;
                    RaisePropertiesChanged("FilteredSalesTypes");
                }
            }
        }
        public SalesType SelectedSalesType { get; set; }
        public Action CloseAction { get; set; }
        public User ActualUser { get { return Globals.ActualUser; } }

        #endregion Properties
    }
}
