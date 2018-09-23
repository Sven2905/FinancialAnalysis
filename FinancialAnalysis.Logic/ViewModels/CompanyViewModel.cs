using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.Accounting;
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
        public SvenTechCollection<CostAccountCategory> CostAccountCategories { get; set; }

        public CompanyViewModel()
        {
            DataLayer db = new DataLayer();
            var tempCat = db.CostAccountCategories.GetAll().ToSvenTechCollection();
            CostAccountCategories = tempCat.ToHierachicalCollection<CostAccountCategory>().ToSvenTechCollection();
        }
    }
}
