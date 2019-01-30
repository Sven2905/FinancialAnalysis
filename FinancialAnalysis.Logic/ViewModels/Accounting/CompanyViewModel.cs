using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.ClientManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels.Accounting
{
    public class CompanyViewModel : ViewModelBase
    {
        public CompanyViewModel()
        {
            if (IsInDesignMode)
                return;

            TaxTypes = DataLayer.Instance.TaxTypes.GetAll().ToSvenTechCollection();
            SelectedTaxTypeId = 1;
        }

        private Client _Client;
        private CustomerType _SelectedCustomerType = CustomerType.Creditor;

        public Client Client
        {
            get { return _Client; }
            set
            {
                _Client = value;
                if (Client != null)
                    if (Client.IsCompany)
                        SelectedClientType = ClientType.Business;
            }
        }

        public event SelectedCustomerTypeChangedEvent SelectedCustomerTypeChanged;
        public delegate void SelectedCustomerTypeChangedEvent(CustomerType customerType);
        public ClientType SelectedClientType { get; set; } = ClientType.Business;
        public bool ShowCustomerType { get; set; }
        public int SelectedTaxTypeId { get; set; }
        public SvenTechCollection<TaxType> TaxTypes { get; set; } = new SvenTechCollection<TaxType>();
        public bool ShowTaxType { get; set; }
        public bool CompanyIsNotNull { get; set; } = false;

        public CustomerType SelectedCustomerType
        {
            get { return _SelectedCustomerType; }
            set { _SelectedCustomerType = value; SelectedCustomerTypeChanged.Invoke(value); }
        }

        internal void Clear()
        {
            Client = new Client();
            CompanyIsNotNull = true;
        }
    }
}
