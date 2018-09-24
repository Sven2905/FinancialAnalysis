using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class CompanyViewModel : ViewModelBase
    {
        public SvenTechCollection<Company> Companies { get; set; }

        public CompanyViewModel()
        {
            DataLayer db = new DataLayer();
            Companies = db.Companies.GetAll().ToSvenTechCollection();
        }
    }
}
