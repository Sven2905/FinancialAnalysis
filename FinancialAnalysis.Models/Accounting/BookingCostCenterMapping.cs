using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    public class BookingCostCenterMapping
    {
        public BookingCostCenterMapping()
        {

        }

        public BookingCostCenterMapping(int RefBookingId, int RefCostCenterId, decimal Amount)
        {
            this.RefBookingId = RefBookingId;
            this.RefCostCenterId = RefCostCenterId;
            this.Amount = Amount;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int BookingCostCenterMappingId { get; set; }

        /// <summary>
        /// Referenz-Id der zugeordneten Buchung
        /// </summary>
        public int RefBookingId { get; set; }

        /// <summary>
        /// Referenz-Id der zugeordneten Kostenstelle
        /// </summary>
        public int RefCostCenterId { get; set; }

        /// <summary>
        /// Betrag
        /// </summary>
        public decimal Amount { get; set; }
    }
}
