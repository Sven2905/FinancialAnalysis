﻿using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.SalesManagement
{
    /// <summary>
    /// Versandtyp
    /// </summary>
    public class ShipmentType : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ShipmentTypeId { get; set; }

        /// <summary>
        /// Name des Typs
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }
    }
}