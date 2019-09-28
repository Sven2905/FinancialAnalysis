using DevExpress.Mvvm;
using FinancialAnalysis.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels.Administration
{
    public class HealthInsuranceViewModel : ViewModelBase
    {
        public HealthInsuranceViewModel()
        {

        }

        public SvenTechCollection<HealthInsurance> HealthInsuranceList { get; set; } = new SvenTechCollection<HealthInsurance>();
    }
}
