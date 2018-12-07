using System;
using System.Collections.Generic;
using FinancialAnalysis.Models.Accounting;

namespace FinancialAnalysis.Models.ProjectManagement
{
    /// <summary>
    ///     Project which can be worked on
    /// </summary>
    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public DateTime TotalEndDate { get; set; }
        public bool IsEnded { get; set; }
        public decimal Costs { get; set; }

        public int RefCostCenterId { get; set; }
        public CostCenter CostCenter { get; set; }

        public int? RefCustomerId { get; set; }
        public Customer Customer { get; set; } // Leader

        public virtual List<ProjectEmployeeMapping> ProjectEmployeeMappings { get; set; }
        public virtual List<ProjectWorkingTime> ProjectWorkingTimes { get; set; }
    }
}