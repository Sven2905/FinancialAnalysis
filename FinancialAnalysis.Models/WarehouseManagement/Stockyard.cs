using DevExpress.Mvvm;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FinancialAnalysis.Models.WarehouseManagement
{
    /// <summary>
    /// Lagerplatz
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class Stockyard : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int StockyardId { get; set; }

        /// <summary>
        /// Bezeichnung Lagerplatz
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Referenz-Id Lagerhaus
        /// </summary>
        public int RefWarehouseId { get; set; }

        /// <summary>
        /// Lagerhaus
        /// </summary>
        public Warehouse Warehouse { get; set; }

        /// <summary>
        /// Ist leer
        /// </summary>
        public bool IsEmpty => StockedProducts.Count == 0;

        /// <summary>
        /// Eingelagerte Produkte
        /// </summary>
        public List<StockedProduct> StockedProducts { get; set; } = new List<StockedProduct>();
    }
}