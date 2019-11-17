using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.ProductManagement
{
    /// <summary>
    /// Produktkategorie
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class ProductCategory : BaseClass
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ProductCategoryId { get; set; }

        /// <summary>
        /// Name des Produkts
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }
    }
}