using System;
using System.Windows;
using DevExpress.Mvvm;

using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.SalesManagement;
using Utilities;
using WebApiWrapper.SalesManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class SalesTypesViewModel : ViewModelBase
    {
        #region Constructor

        public SalesTypesViewModel()
        {
            if (IsInDesignMode) return;

            FilteredSalesTypes = _SalesTypes = LoadAllSalesTypes();
            NewSalesTypeCommand = new DelegateCommand(NewSalesType);
            SaveSalesTypeCommand = new DelegateCommand(SaveSalesType, () => Validation());
            DeleteSalesTypeCommand = new DelegateCommand(DeleteSalesType, () => SelectedSalesType != null);
            SelectedCommand = new DelegateCommand(() =>
            {
                SendSelectedToParent();
                CloseAction();
            });
        }

        #endregion Constructor

        #region Fields

        private readonly SalesType _SelectedSalesType;
        private readonly SvenTechCollection<SalesType> _SalesTypes = new SvenTechCollection<SalesType>();
        private string _FilterText = string.Empty;

        #endregion Fields

        #region Methods

        private SvenTechCollection<SalesType> LoadAllSalesTypes()
        {
            var allSalesTypes = new SvenTechCollection<SalesType>();
            allSalesTypes = SalesTypes.GetAll().ToSvenTechCollection();

            return allSalesTypes;
        }

        private void NewSalesType()
        {
            SelectedSalesType = new SalesType();
            _SalesTypes.Add(SelectedSalesType);
        }

        private void DeleteSalesType()
        {
            if (SelectedSalesType == null) return;

            if (SelectedSalesType.SalesTypeId == 0)
            {
                _SalesTypes.Remove(SelectedSalesType);
                SelectedSalesType = null;
                return;
            }

            SalesTypes.Delete(SelectedSalesType.SalesTypeId);
            _SalesTypes.Remove(SelectedSalesType);
            SelectedSalesType = null;
        }

        private void SaveSalesType()
        {
            if (SelectedSalesType.SalesTypeId != 0)
                SalesTypes.Update(SelectedSalesType);
            else
                SelectedSalesType.SalesTypeId = SalesTypes.Insert(SelectedSalesType);
        }

        private bool Validation()
        {
            if (SelectedSalesType == null) return false;
            if (string.IsNullOrEmpty(SelectedSalesType.Name)) return false;
            return true;
        }

        public void SendSelectedToParent()
        {
            if (SelectedSalesType == null) return;

            if (SelectedSalesType.SalesTypeId == 0) SaveSalesType();

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
            get => _FilterText;
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredSalesTypes = new SvenTechCollection<SalesType>();
                    foreach (var item in _SalesTypes)
                        if (item.Name.Contains(FilterText))
                            FilteredSalesTypes.Add(item);
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
        public User ActualUser => Globals.ActiveUser;

        #endregion Properties
    }
}