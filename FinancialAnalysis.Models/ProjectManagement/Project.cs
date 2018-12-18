using System;
using System.Collections.Generic;
using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;

namespace FinancialAnalysis.Models.ProjectManagement
{
    /// <summary>
    ///     Project which can be worked on
    /// </summary>
    public class Project : BindableBase
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime ExpectedEndDate { get; set; } = DateTime.Now;
        public DateTime TotalEndDate { get; set; } = DateTime.Now;
        public bool IsEnded { get; set; }
        public decimal Costs { get; set; }
        public string Identifier { get; set; }

        public int RefCostCenterId { get; set; }
        public virtual CostCenter CostCenter { get; set; }

        public int RefEmployeeId { get; set; }
        public virtual Employee Employee { get; set; } // Leader

        public virtual List<ProjectEmployeeMapping> ProjectEmployeeMappings { get; set; }
        public virtual List<ProjectWorkingTime> ProjectWorkingTimes { get; set; }
    }
}