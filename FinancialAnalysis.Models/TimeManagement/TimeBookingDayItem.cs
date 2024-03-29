﻿using DevExpress.Mvvm;
using System;

namespace FinancialAnalysis.Models.TimeManagement
{
    public class TimeBookingDayItem : BaseClass
    {
        public DateTime BookingDate { get; set; }
        public DayOfWeek BookingDay => BookingDate.DayOfWeek;
        public double ObligatoryHours { get; set; }
        public TimeSpan WorkingHours { get; set; }
        public double Balance { get; set; }
        public TimeSpan BreaktimeHours { get; set; }
        public double DailySaldo => WorkingHours.Subtract(TimeSpan.FromHours(ObligatoryHours)).TotalHours;
        public string AbsentReason { get; set; }
    }
}