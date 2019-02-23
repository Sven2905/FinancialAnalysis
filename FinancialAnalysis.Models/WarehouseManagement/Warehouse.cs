﻿using DevExpress.Mvvm;
using Utilities;

namespace FinancialAnalysis.Models.WarehouseManagement
{
    /// <summary>
    /// Lagerhaus
    /// </summary>
    public class Warehouse : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int WarehouseId { get; set; }

        /// <summary>
        /// Name des Lagerhauses
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Strasse
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// PLZ
        /// </summary>
        public int Postcode { get; set; }

        /// <summary>
        /// Stadt
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Lagerplätze
        /// </summary>
        public SvenTechCollection<Stockyard> Stockyards { get; set; } = new SvenTechCollection<Stockyard>();
    }
}