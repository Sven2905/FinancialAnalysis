﻿using DevExpress.Mvvm;
using FinancialAnalysis.Models.ProductManagement;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.WarehouseManagement
{
    /// <summary>
    /// Eingelagertes Produkt
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class StockedProduct : BaseClass
    {
        public StockedProduct()
        {
        }

        public StockedProduct(Product product, int refStockyardId, int quantity)
        {
            RefProductId = product.ProductId;
            Product = product;
            RefStockyardId = refStockyardId;
            Quantity = quantity;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int StockedProductId { get; set; }

        /// <summary>
        /// Referenz-Id Produkt
        /// </summary>
        public int RefProductId { get; set; }

        /// <summary>
        /// Produkt
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Referenz-Id Lagerplatz
        /// </summary>
        public int RefStockyardId { get; set; }

        /// <summary>
        /// Menge
        /// </summary>
        public int Quantity { get; set; }
    }
}