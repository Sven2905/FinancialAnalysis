using DevExpress.Mvvm;
using System;

namespace FinancialAnalysis.Models.Accounting
{
    public class CostCenterBudget : ViewModelBase
    {
        public int CostCenterBudgetId { get; set; }
        public int Year { get; set; } = DateTime.Now.Year;
        public int RefCostCenterId { get; set; }

        public decimal January { get; set; } = 0;
        public decimal February { get; set; } = 0;
        public decimal March { get; set; } = 0;
        public decimal April { get; set; } = 0;
        public decimal May { get; set; } = 0;
        public decimal June { get; set; } = 0;
        public decimal July { get; set; } = 0;
        public decimal August { get; set; } = 0;
        public decimal September { get; set; } = 0;
        public decimal October { get; set; } = 0;
        public decimal November { get; set; } = 0;
        public decimal December { get; set; } = 0;

        public decimal Quarter1
        {
            get
            {
                return January + February + March;
            }
            set
            {
                January = value / 3;
                February = value / 3;
                March = value / 3;
            }
        }

        public decimal Quarter2
        {
            get
            {
                return April + May + June;
            }
            set
            {
                April = value / 3;
                May = value / 3;
                June = value / 3;
            }
        }

        public decimal Quarter3
        {
            get
            {
                return July + August + September;
            }
            set
            {
                July = value / 3;
                August = value / 3;
                September = value / 3;
            }
        }

        public decimal Quarter4
        {
            get
            {
                return October + November + December;
            }
            set
            {
                October = value / 3;
                November = value / 3;
                December = value / 3;
            }
        }

        public decimal Annually
        {
            get
            {
                return Quarter1 + Quarter2 + Quarter3 + Quarter4;
            }
            set
            {
                Quarter1 = value / 4;
                Quarter2 = value / 4;
                Quarter3 = value / 4;
                Quarter4 = value / 4;
            }
        }
    }
}