using System;
using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.WarehouseManagement
{
    public class WarehouseBookingHistoryItem : BindableBase
    {
        public int WarehouseBookingHistoryItemId { get; set; }
        public int Quantity { get; set; }
        public int RefProductId { get; set; }
        public DateTime Date { get; set; }
        public int RefUserId { get; set; }
    }
}