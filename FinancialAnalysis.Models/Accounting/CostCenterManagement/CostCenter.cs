using DevExpress.Mvvm;
using FinancialAnalysis.Models.ProjectManagement;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    ///     Kostenstelle
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class CostCenter : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int CostCenterId { get; set; }

        /// <summary>
        /// Name der Kostenstelle
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Eindeutiger Schlüssel, vom User frei wählbar
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Referenz-Id Kostenstellenkategorie
        /// </summary>
        public int RefCostCenterCategoryId { get; set; }

        /// <summary>
        /// Kostenstellentyp
        /// </summary>
        public CostCenterType CostCenterType { get; set; } = CostCenterType.Main;

        /// <summary>
        /// Kostenstellenkategorie
        /// </summary>
        [JsonIgnore]
        public CostCenterCategory CostCenterCategory { get; set; }

        /// <summary>
        /// Referenz-Id Kostenstellenbudget
        /// </summary>
        public int RefCostCenterBudgetId { get; set; }

        /// <summary>
        /// Geplantes Kostenstellenbudget
        /// </summary>
        public CostCenterBudget ScheduledBudget { get; set; } = new CostCenterBudget();

        /// <summary>
        /// Liste aller Projekte denen die Kostenstelle zugeordnet ist
        /// </summary>
        public List<Project> Projects { get; set; }

        /// <summary>
        /// Liste der zugeordneten Beträge
        /// </summary>
        public List<BookingCostCenterMapping> BookingCostCenterMappingList { get; set; } = new List<BookingCostCenterMapping>();
    }
}