using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models
{
    public class PurchaseOrderDetail : BindableBase
    {
        public int PurchaseOrderDetailsId { get; set; }
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double DiscountPercentage { get; set; }
        public double TaxPercentage { get; set; }
        public double Total { get; set; }
    }
}
