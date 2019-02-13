using DevExpress.Mvvm;
using FinancialAnalysis.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    ///     Class that represents a cost center for matching costs and bills
    /// </summary>
    public class CostCenter : BindableBase
    {
        public string Name { get; set; }
        public int CostCenterId { get; set; }
        public string Description { get; set; }
        public string Identifier { get; set; }
        public int RefCostCenterCategoryId { get; set; }
        public CostCenterType CostCenterType { get; set; } = CostCenterType.Main;
        public CostCenterCategory CostCenterCategory { get; set; }
        public int CostCenterBudgetId { get; set; }
        public CostCenterBudget ScheduledBudget { get; set; } = new CostCenterBudget();

        public List<Project> Projects { get; set; }
    }
}