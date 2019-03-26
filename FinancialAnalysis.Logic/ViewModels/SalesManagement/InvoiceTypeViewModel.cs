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
    public class InvoiceTypeViewModel : ViewModelBase
    {
        #region Constructor

        public InvoiceTypeViewModel()
        {
            if (IsInDesignMode) return;

            _InvoiceTypes = LoadAllInvoiceTypes();
            NewInvoiceTypeCommand = new DelegateCommand(NewInvoiceType);
            SaveInvoiceTypeCommand = new DelegateCommand(SaveInvoiceType, () => Validation());
            DeleteInvoiceTypeCommand = new DelegateCommand(DeleteInvoiceType, () => SelectedInvoiceType != null);
            SelectedCommand = new DelegateCommand(() =>
            {
                SendSelectedToParent();
                CloseAction();
            });
        }

        #endregion Constructor

        #region Fields

        private readonly InvoiceType _SelectedInvoiceType;
        private readonly SvenTechCollection<InvoiceType> _InvoiceTypes = new SvenTechCollection<InvoiceType>();
        private string _FilterText;

        #endregion Fields

        #region Methods

        private SvenTechCollection<InvoiceType> LoadAllInvoiceTypes()
        {
            var allInvoiceTypes = new SvenTechCollection<InvoiceType>();
            return InvoiceTypes.GetAll().ToSvenTechCollection();
        }

        private void NewInvoiceType()
        {
            SelectedInvoiceType = new InvoiceType();
            _InvoiceTypes.Add(SelectedInvoiceType);
        }

        private void DeleteInvoiceType()
        {
            if (SelectedInvoiceType == null) return;

            if (SelectedInvoiceType.InvoiceTypeId == 0)
            {
                _InvoiceTypes.Remove(SelectedInvoiceType);
                SelectedInvoiceType = null;
                return;
            }

            InvoiceTypes.Delete(SelectedInvoiceType.InvoiceTypeId);
            _InvoiceTypes.Remove(SelectedInvoiceType);
            SelectedInvoiceType = null;
        }

        private void SaveInvoiceType()
        {
            if (SelectedInvoiceType.InvoiceTypeId != 0)
                InvoiceTypes.Update(SelectedInvoiceType);
            else
                InvoiceTypes.Insert(SelectedInvoiceType);
        }

        private bool Validation()
        {
            if (SelectedInvoiceType == null) return false;
            if (string.IsNullOrEmpty(SelectedInvoiceType.Name)) return false;
            return true;
        }

        public void SendSelectedToParent()
        {
            if (SelectedInvoiceType == null) return;

            if (SelectedInvoiceType.InvoiceTypeId == 0) SaveInvoiceType();

            Messenger.Default.Send(new SelectedInvoiceType { InvoiceType = SelectedInvoiceType });
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<InvoiceType> FilteredInvoiceTypes { get; set; } =
            new SvenTechCollection<InvoiceType>();

        public DelegateCommand NewInvoiceTypeCommand { get; set; }
        public DelegateCommand SaveInvoiceTypeCommand { get; set; }
        public DelegateCommand DeleteInvoiceTypeCommand { get; set; }
        public DelegateCommand SelectedCommand { get; }
        public Action CloseAction { get; set; }

        public string FilterText
        {
            get => _FilterText;
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredInvoiceTypes = new SvenTechCollection<InvoiceType>();
                    foreach (var item in _InvoiceTypes)
                        if (item.Name.Contains(FilterText))
                            FilteredInvoiceTypes.Add(item);
                }
                else
                {
                    FilteredInvoiceTypes = _InvoiceTypes;
                }
            }
        }

        public InvoiceType SelectedInvoiceType { get; set; }

        public User ActualUser => Globals.ActiveUser;

        #endregion Properties
    }
}