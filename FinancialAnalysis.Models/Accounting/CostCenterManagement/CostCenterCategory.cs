using DevExpress.Mvvm;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Kostenstellenkategorie
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class CostCenterCategory : BaseClass
    {
        /// <summary>
        /// Id
        /// </summary>
        public int CostCenterCategoryId { get; set; }

        /// <summary>
        /// Name der Kostenstellenkategorie
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Liste aller zugeordneten Kostenstellen
        /// </summary>
        public List<CostCenter> CostCenters { get; set; } = new List<CostCenter>();
    }
}