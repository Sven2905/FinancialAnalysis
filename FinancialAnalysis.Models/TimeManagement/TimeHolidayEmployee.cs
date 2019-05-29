using DevExpress.Mvvm;
using FinancialAnalysis.Models.General;
using FinancialAnalysis.Models.ProjectManagement;
using System;
using System.Windows.Media;

namespace FinancialAnalysis.Models.TimeManagement
{
    public class TimeHolidayEmployee : BindableBase
    {
        #region Fields

        private DateTime _FirstDay;
        private DateTime _LastDay;
        private bool _IsHalfFirstDay;
        private bool _IsHalfLastDay;
        private bool _OnlyWorkingDays;
        private bool _IsSpecialLeave;

        #endregion Fields

        #region Events

        public event EventHandler ValueChanged;

        #endregion Events

        #region Properties

        public bool OnlyWorkingDays
        {
            get => _OnlyWorkingDays;
            set { _OnlyWorkingDays = value; OnValueChanged(); }
        }

        public bool IsHalfLastDay
        {
            get => _IsHalfLastDay;
            set { _IsHalfLastDay = value; OnValueChanged(); }
        }

        public bool IsHalfFirstDay
        {
            get => _IsHalfFirstDay;
            set { _IsHalfFirstDay = value; OnValueChanged(); }
        }

        public DateTime LastDay
        {
            get => _LastDay;
            set { _LastDay = value; OnValueChanged(); }
        }

        public DateTime FirstDay
        {
            get => _FirstDay;
            set { _FirstDay = value; OnValueChanged(); }
        }

        public bool IsSpecialLeave
        {
            get => _IsSpecialLeave;
            set { _IsSpecialLeave = value; OnValueChanged(); }
        }

        public int TimeHolidayEmployeeId { get; set; }
        public int RefEmployeeId { get; set; }
        public string Reason { get; set; }
        public int RefTimeHolidayTypeId { get; set; }
        public DateTime TimeStamp { get; set; }
        public Employee ApprovalEmployee { get; set; }
        public int RefApprovalEmployeeId { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ApprovalDate { get; set; }

        public decimal Days
        {
            get
            {
                decimal days = (LastDay - FirstDay).Days + 1;
                if (IsHalfFirstDay)
                {
                    days -= 0.5m;
                }

                if (IsHalfLastDay)
                {
                    days -= 0.5m;
                }

                return days;
            }
        }

        public string IconData
        {
            get
            {
                if (IsApproved)
                {
                    return "M413.505 91.951L133.49 371.966l-98.995-98.995c-4.686-4.686-12.284-4.686-16.971 0L6.211 284.284c-4.686 4.686-4.686 12.284 0 16.971l118.794 118.794c4.686 4.686 12.284 4.686 16.971 0l299.813-299.813c4.686-4.686 4.686-12.284 0-16.971l-11.314-11.314c-4.686-4.686-12.284-4.686-16.97 0z";
                }
                else if (!IsApproved && RefApprovalEmployeeId != 0)
                {
                    return "M193.94 256L296.5 153.44l21.15-21.15c3.12-3.12 3.12-8.19 0-11.31l-22.63-22.63c-3.12-3.12-8.19-3.12-11.31 0L160 222.06 36.29 98.34c-3.12-3.12-8.19-3.12-11.31 0L2.34 120.97c-3.12 3.12-3.12 8.19 0 11.31L126.06 256 2.34 379.71c-3.12 3.12-3.12 8.19 0 11.31l22.63 22.63c3.12 3.12 8.19 3.12 11.31 0L160 289.94 262.56 392.5l21.15 21.15c3.12 3.12 8.19 3.12 11.31 0l22.63-22.63c3.12-3.12 3.12-8.19 0-11.31L193.94 256z";
                }
                else
                {
                    return "M368 32h4c6.627 0 12-5.373 12-12v-8c0-6.627-5.373-12-12-12H12C5.373 0 0 5.373 0 12v8c0 6.627 5.373 12 12 12h4c0 91.821 44.108 193.657 129.646 224C59.832 286.441 16 388.477 16 480h-4c-6.627 0-12 5.373-12 12v8c0 6.627 5.373 12 12 12h360c6.627 0 12-5.373 12-12v-8c0-6.627-5.373-12-12-12h-4c0-91.821-44.108-193.657-129.646-224C324.168 225.559 368 123.523 368 32zM48 32h288c0 110.457-64.471 200-144 200S48 142.457 48 32zm288 448H48c0-110.457 64.471-200 144-200s144 89.543 144 200zm-66.34-333.088a141.625 141.625 0 0 1-6.774 8.739c-2.301 2.738-5.671 4.348-9.248 4.348H130.362c-3.576 0-6.947-1.61-9.248-4.348a142.319 142.319 0 0 1-6.774-8.739c-5.657-7.91.088-18.912 9.813-18.912h135.694c9.725 0 15.469 11.003 9.813 18.912zM98.379 416h187.243a12.01 12.01 0 0 1 11.602 8.903 199.464 199.464 0 0 1 2.059 8.43c1.664 7.522-4 14.667-11.704 14.667H96.422c-7.704 0-13.368-7.145-11.704-14.667.62-2.804 1.307-5.616 2.059-8.43A12.01 12.01 0 0 1 98.379 416z";
                }
            }
        }

        public Brush IconColor
        {
            get
            {
                if (IsApproved)
                {
                    return SvenTechColors.BrushLightGreen;
                }
                else if (!IsApproved && RefApprovalEmployeeId != 0)
                {
                    return SvenTechColors.BrushLightRed;
                }
                else
                {
                    return Brushes.Black;
                }
            }
        }

        #endregion Properties

        #region Methods

        public void OnValueChanged()
        {
            ValueChanged?.Invoke(this, new EventArgs());
        }

        #endregion Methods
    }
}
