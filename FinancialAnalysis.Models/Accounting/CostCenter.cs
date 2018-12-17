using System.Collections.Generic;
using DevExpress.Mvvm;
using FinancialAnalysis.Models.ProjectManagement;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    ///     Class that represents a cost center for matching costs and bills
    /// </summary>
    public class CostCenter : BindableBase
    {
        public int CostCenterId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Project> Projects { get; set; }
    }
}