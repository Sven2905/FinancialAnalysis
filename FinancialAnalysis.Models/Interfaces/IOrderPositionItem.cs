﻿using FinancialAnalysis.Models.ProductManagement;

namespace FinancialAnalysis.Models
{
    public interface IOrderPositionItem
    {
        int RefProductId { get; set; }
        Product Product { get; set; }
        string Description { get; set; }
        decimal Quantity { get; set; }
        decimal Price { get; set; }
        decimal DiscountPercentage { get; set; }
        GrossNetType GrossNetType { get; set; }
        decimal DiscountAmount { get; }
        decimal SubtotalWithoutDiscount { get; }
        decimal Subtotal { get; } // w/o Tax
        decimal Total { get; }
        decimal TaxAmount { get; }
        bool IsCanceled { get; set; }
    }
}