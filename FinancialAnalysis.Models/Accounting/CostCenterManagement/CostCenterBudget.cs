using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Kostenstellen Budget
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class CostCenterBudget : BindableBase
    {
        #region Fields

        private decimal _January = 0;
        private decimal _February = 0;
        private decimal _March = 0;
        private decimal _April = 0;
        private decimal _May = 0;
        private decimal _June = 0;
        private decimal _July = 0;
        private decimal _August = 0;
        private decimal _September = 0;
        private decimal _October = 0;
        private decimal _November = 0;
        private decimal _December = 0;

        #endregion Fields

        #region Properties

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
        public decimal January
        {
            get => _January;
            set { _January = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Geplante Kosten für Februar
        /// </summary>
        public decimal February
        {
            get => _February;
            set { _February = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Geplante Kosten für März
        /// </summary>
        public decimal March
        {
            get => _March;
            set { _March = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Geplante Kosten für April
        /// </summary>
        public decimal April
        {
            get => _April;
            set { _April = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Geplante Kosten für Mai
        /// </summary>
        public decimal May
        {
            get => _May;
            set { _May = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Geplante Kosten für Juni
        /// </summary>
        public decimal June
        {
            get => _June;
            set { _June = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Geplante Kosten für Juli
        /// </summary>
        public decimal July
        {
            get => _July;
            set { _July = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Geplante Kosten für August
        /// </summary>
        public decimal August
        {
            get => _August;
            set { _August = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Geplante Kosten für September
        /// </summary>
        public decimal September
        {
            get => _September;
            set { _September = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Geplante Kosten für Oktober
        /// </summary>
        public decimal October
        {
            get => _October;
            set { _October = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Geplante Kosten für November
        /// </summary>
        public decimal November
        {
            get => _November;
            set { _November = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Geplante Kosten für Dezember
        /// </summary>
        public decimal December
        {
            get => _December;
            set { _December = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Aufsummierte Kosten des 1. Quartals
        /// </summary>
        public decimal Quarter1
        {
            get => January + February + March;
            set
            {
                _January = value / 3;
                _February = value / 3;
                _March = value / 3;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Aufsummierte Kosten des 2. Quartals
        /// </summary>
        public decimal Quarter2
        {
            get => April + May + June;
            set
            {
                _April = value / 3;
                _May = value / 3;
                _June = value / 3;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Aufsummierte Kosten des 3. Quartals
        /// </summary>
        public decimal Quarter3
        {
            get => July + August + September;
            set
            {
                _July = value / 3;
                _August = value / 3;
                _September = value / 3;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Aufsummierte Kosten des 4. Quartals
        /// </summary>
        public decimal Quarter4
        {
            get => October + November + December;
            set
            {
                _October = value / 3;
                _November = value / 3;
                _December = value / 3;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Aufsummierte Kosten des Jahres
        /// </summary>
        public decimal Annually
        {
            get => Quarter1 + Quarter2 + Quarter3 + Quarter4;
            set
            {
                _January = value / 12;
                _February = value / 12;
                _March = value / 12;
                _April = value / 12;
                _May = value / 12;
                _June = value / 12;
                _July = value / 12;
                _August = value / 12;
                _September = value / 12;
                _October = value / 12;
                _November = value / 12;
                _December = value / 12;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}