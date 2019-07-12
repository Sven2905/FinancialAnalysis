using FinancialAnalysis.Models;

namespace FinancialAnalysis.Logic.Messages
{
    public class OpenDebitSplitWindowMessage
    {
        public OpenDebitSplitWindowMessage(BookingType BookingType, decimal TotalAmount)
        {
            this.BookingType = BookingType;
            this.TotalAmount = TotalAmount;
        }

        public BookingType BookingType { get; set; }
        public decimal TotalAmount { get; set; }
    }
}