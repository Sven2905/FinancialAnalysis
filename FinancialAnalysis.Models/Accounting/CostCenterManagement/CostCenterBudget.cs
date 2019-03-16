using DevExpress.Mvvm;
using System;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Kostenstellen Budget
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class CostCenterBudget : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int CostCenterBudgetId { get; set; }

        /// <summary>
        /// Jahr
        /// </summary>
        public int Year { get; set; } = DateTime.Now.Year;

        /// <summary>
        /// Referenz-Id Kostenstelle
        /// </summary>
        public int RefCostCenterId { get; set; }

        /// <summary>
        /// Geplante Kosten für Januar
        /// </summary>
        public decimal January { get; set; } = 0;

        /// <summary>
        /// Geplante Kosten für Februar
        /// </summary>
        public decimal February { get; set; } = 0;

        /// <summary>
        /// Geplante Kosten für März
        /// </summary>
        public decimal March { get; set; } = 0;

        /// <summary>
        /// Geplante Kosten für April
        /// </summary>
        public decimal April { get; set; } = 0;

        /// <summary>
        /// Geplante Kosten für Mai
        /// </summary>
        public decimal May { get; set; } = 0;

        /// <summary>
        /// Geplante Kosten für Juni
        /// </summary>
        public decimal June { get; set; } = 0;

        /// <summary>
        /// Geplante Kosten für Juli
        /// </summary>
        public decimal July { get; set; } = 0;

        /// <summary>
        /// Geplante Kosten für August
        /// </summary>
        public decimal August { get; set; } = 0;

        /// <summary>
        /// Geplante Kosten für September
        /// </summary>
        public decimal September { get; set; } = 0;

        /// <summary>
        /// Geplante Kosten für Oktober
        /// </summary>
        public decimal October { get; set; } = 0;

        /// <summary>
        /// Geplante Kosten für November
        /// </summary>
        public decimal November { get; set; } = 0;

        /// <summary>
        /// Geplante Kosten für Dezember
        /// </summary>
        public decimal December { get; set; } = 0;

        /// <summary>
        /// Aufsummierte Kosten des 1. Quartals
        /// </summary>
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

        /// <summary>
        /// Aufsummierte Kosten des 2. Quartals
        /// </summary>
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

        /// <summary>
        /// Aufsummierte Kosten des 3. Quartals
        /// </summary>
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

        /// <summary>
        /// Aufsummierte Kosten des 4. Quartals
        /// </summary>
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

        /// <summary>
        /// Aufsummierte Kosten des Jahres
        /// </summary>
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