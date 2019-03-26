using DevExpress.Mvvm;

using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.ClientManagement;
using Utilities;
using WebApiWrapper.Accounting;

namespace FinancialAnalysis.Logic.ViewModels.Accounting
{
    public class CompanyViewModel : ViewModelBase
    {
        public delegate void SelectedCustomerTypeChangedEvent(CustomerType customerType);

        private Client _Client;
        private CustomerType _SelectedCustomerType = CustomerType.Creditor;

        public CompanyViewModel()
        {
            if (IsInDesignMode) return;

            TaxTypeList = TaxTypes.GetAll().ToSvenTechCollection();
            SelectedTaxTypeId = 1;
        }

        public Client Client
        {
            get => _Client;
            set
            {
                _Client = value;
                if (Client != null)
                    if (Client.IsCompany)
                        SelectedClientType = ClientType.Business;
            }
        }

        public ClientType SelectedClientType { get; set; } = ClientType.Business;
        public bool ShowCustomerType { get; set; }
        public int SelectedTaxTypeId { get; set; }
        public SvenTechCollection<TaxType> TaxTypeList { get; set; } = new SvenTechCollection<TaxType>();
        public bool ShowTaxType { get; set; }
        public bool CompanyIsNotNull { get => Client != null; }

        public CustomerType SelectedCustomerType
        {
            get => _SelectedCustomerType;
            set
            {
                _SelectedCustomerType = value;
                SelectedCustomerTypeChanged.Invoke(value);
            }
        }

        public event SelectedCustomerTypeChangedEvent SelectedCustomerTypeChanged;

        internal void Clear()
        {
            Client = new Client();
        }
    }
}