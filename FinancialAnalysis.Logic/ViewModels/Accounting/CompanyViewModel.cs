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
        }

        public Client Client { get; set; }
        public ClientType SelectedClientType { get; set; } = ClientType.Business;
        public TaxType SelectedTaxType { get; set; }
        public SvenTechCollection<TaxType> TaxTypes { get; set; } = new SvenTechCollection<TaxType>();
    }
}
