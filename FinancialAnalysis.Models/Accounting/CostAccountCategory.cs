using DevExpress.Mvvm;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Kontenrahmenkategorie
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class CostAccountCategory : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int CostAccountCategoryId { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Referenz-Id der übergeordneten Kategorie
        /// </summary>
        public int ParentCategoryId { get; set; }

        /// <summary>
        /// Liste aller untergeordneten Kategorien
        /// </summary>
        public IEnumerable<CostAccountCategory> SubCategories { get; set; }
    }
}