using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.PurchaseManagement
{
    public class PurchaseOrderPosition : BindableBase
    {
        public int PurchaseOrderPositionId { get; set; }
        public int RefPurchaseOrderId { get; set; }
        public int RefProductId { get; set; }
        public double RefTaxTypeId { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double DiscountPercentage { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsCanceled { get; set; }
    }
}
