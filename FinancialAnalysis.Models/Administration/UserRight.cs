using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.Administration
{
    /// <summary>
    /// Benutzerrecht
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class UserRight : BindableBase
    {
        public UserRight()
        {
        }

        public UserRight(Permission Permission, string Name, int ParentCategory = 0, string Description = "")
        {
            this.Permission = Permission;
            this.Name = Name;
            this.Description = Description;
            this.ParentCategory = ParentCategory;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int UserRightId { get; set; }

        /// <summary>
        /// Name des Rechts
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Entsprechender Enumerationswert
        /// </summary>
        public Permission Permission { get; set; }

        /// <summary>
        /// Übergeordnetes Recht, Default 0
        /// </summary>
        public int ParentCategory { get; set; }
    }
}