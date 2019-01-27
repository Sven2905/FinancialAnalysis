using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.BaseClasses;
using FinancialAnalysis.Models.ProductManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.PurchaseManagement
{
    public class PurchaseOrderPosition : OrderPositionItem
    {
        public int PurchaseOrderPositionId { get; set; }
        public int RefPurchaseOrderId { get; set; }
        public bool IsDelivered { get; set; }
    }
}
