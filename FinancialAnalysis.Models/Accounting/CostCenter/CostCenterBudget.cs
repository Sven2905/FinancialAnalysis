using DevExpress.Mvvm;
using System;

namespace FinancialAnalysis.Models.Accounting
{
    public class CostCenterBudget : ViewModelBase
    {
        public delegate void ValueChanged();

        public event ValueChanged ValueChangedEvent;

        private decimal _january = 0;
        private decimal _february = 0;
        private decimal _march = 0;
        private decimal _april = 0;
        private decimal _may = 0;
        private decimal _june = 0;
        private decimal _july = 0;
        private decimal _august = 0;
        private decimal _september = 0;
        private decimal _october = 0;
        private decimal _november = 0;
        private decimal _december = 0;

        public int CostCenterBudgetId { get; set; }
        public int Year { get; set; } = DateTime.Now.Year;
        public int RefCostCenterId { get; set; }
        public decimal January { get => _january; set { _january = value; ValueChangedEvent?.Invoke(); } }
        public decimal February { get => _february; set { _february = value; ValueChangedEvent?.Invoke(); } }
        public decimal March { get => _march; set { _march = value; ValueChangedEvent?.Invoke(); } }
        public decimal April { get => _april; set { _april = value; ValueChangedEvent?.Invoke(); } }
        public decimal May { get => _may; set { _may = value; ValueChangedEvent?.Invoke(); } }
        public decimal June { get => _june; set { _june = value; ValueChangedEvent?.Invoke(); } }
        public decimal July { get => _july; set { _july = value; ValueChangedEvent?.Invoke(); } }
        public decimal August { get => _august; set { _august = value; ValueChangedEvent?.Invoke(); } }
        public decimal September { get => _september; set { _september = value; ValueChangedEvent?.Invoke(); } }
        public decimal October { get => _october; set { _october = value; ValueChangedEvent?.Invoke(); } }
        public decimal November { get => _november; set { _november = value; ValueChangedEvent?.Invoke(); } }
        public decimal December { get => _december; set { _december = value; ValueChangedEvent?.Invoke(); } }

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
                ValueChangedEvent?.Invoke();
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
                ValueChangedEvent?.Invoke();
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
                ValueChangedEvent?.Invoke();
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
                ValueChangedEvent?.Invoke();
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
                ValueChangedEvent?.Invoke();
            }
        }
    }
}