﻿using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.General;
using FinancialAnalysis.Models.ProductManagement;
using Newtonsoft.Json;
using System;
using System.Windows.Media;

namespace FinancialAnalysis.Models.WarehouseManagement
{
    /// <summary>
    /// Ein- und Auslagerungsverlauf
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class WarehouseStockingHistory : BaseClass
    {
        private readonly string takeOut = "M160 217.1c0-8.8 7.2-16 16-16h144v-93.9c0-7.1 8.6-10.7 13.6-5.7l141.6 143.1c6.3 6.3 6.3 16.4 0 22.7L333.6 410.4c-5 5-13.6 1.5-13.6-5.7v-93.9H176c-8.8 0-16-7.2-16-16v-77.7m-32 0v77.7c0 26.5 21.5 48 48 48h112v61.9c0 35.5 43 53.5 68.2 28.3l141.7-143c18.8-18.8 18.8-49.2 0-68L356.2 78.9c-25.1-25.1-68.2-7.3-68.2 28.3v61.9H176c-26.5 0-48 21.6-48 48zM0 112v288c0 26.5 21.5 48 48 48h132c6.6 0 12-5.4 12-12v-8c0-6.6-5.4-12-12-12H48c-8.8 0-16-7.2-16-16V112c0-8.8 7.2-16 16-16h132c6.6 0 12-5.4 12-12v-8c0-6.6-5.4-12-12-12H48C21.5 64 0 85.5 0 112z";
        private static readonly string store = "M32 217.1c0-8.8 7.2-16 16-16h144v-93.9c0-7.1 8.6-10.7 13.6-5.7l141.6 143.1c6.3 6.3 6.3 16.4 0 22.7L205.6 410.4c-5 5-13.6 1.5-13.6-5.7v-93.9H48c-8.8 0-16-7.2-16-16v-77.7m-32 0v77.7c0 26.5 21.5 48 48 48h112v61.9c0 35.5 43 53.5 68.2 28.3l141.7-143c18.8-18.8 18.8-49.2 0-68L228.2 78.9c-25.1-25.1-68.2-7.3-68.2 28.3v61.9H48c-26.5 0-48 21.6-48 48zM512 400V112c0-26.5-21.5-48-48-48H332c-6.6 0-12 5.4-12 12v8c0 6.6 5.4 12 12 12h132c8.8 0 16 7.2 16 16v288c0 8.8-7.2 16-16 16H332c-6.6 0-12 5.4-12 12v8c0 6.6 5.4 12 12 12h132c26.5 0 48-21.5 48-48z";

        public WarehouseStockingHistory()
        {
        }

        public WarehouseStockingHistory(Product product, Stockyard stockyard, int quantity, User user)
        {
            if (product != null)
            {
                RefProductId = product.ProductId;
                ProductName = product.Name;
            }
            if (stockyard != null)
            {
                RefStockyardId = stockyard.StockyardId;
                StockyardName = stockyard.Name;
            }
            Quantity = quantity;
            RefUserId = user.UserId;
            UserName = user.Name;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int WarehouseStockingHistoryId { get; set; }

        /// <summary>
        /// Menge
        /// </summary>
        public int Quantity { get; set; }

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
        /// Produktname
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Lagername
        /// </summary>
        public Stockyard Stockyard { get; set; }

        /// <summary>
        /// Lagerplatzname
        /// </summary>
        public string StockyardName { get; set; }

        /// <summary>
        /// Datum der Lagerung
        /// </summary>
        public DateTime Date { get; set; } = DateTime.Now;

        /// <summary>
        /// Referenz-Id des Benutzers
        /// </summary>
        public int RefUserId { get; set; }

        /// <summary>
        /// Benutzername
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Ausführender Benutzer
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Farbe des Icons für Ein- oder Auslagerung
        /// </summary>
        public Brush IconColor
        {
            get
            {
                if (Quantity >= 0)
                {
                    return SvenTechColors.BrushLightGreen;
                }
                else
                {
                    return SvenTechColors.BrushLightRed;
                }
            }
        }

        /// <summary>
        /// Icon für Ein- oder Auslagerung
        /// </summary>
        public Geometry IconData
        {
            get
            {
                if (Quantity >= 0)
                {
                    return Geometry.Parse(takeOut);
                }
                else
                {
                    return Geometry.Parse(store);
                }
            }
        }
    }
}