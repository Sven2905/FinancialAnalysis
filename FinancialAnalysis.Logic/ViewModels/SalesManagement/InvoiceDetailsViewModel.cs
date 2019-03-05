using DevExpress.Mvvm;
using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class InvoiceDetailsViewModel : ViewModelBase
    {
        public Invoice Invoice { get; set; }
    }
}
