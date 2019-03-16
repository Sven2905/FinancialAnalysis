using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.ProjectManagement
{
    /// <summary>
    /// Zuordnung Mitarbeiter-Projekt
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class ProjectEmployeeMapping : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ProjectEmployeeMappingId { get; set; }

        /// <summary>
        /// Referenz-Id Mitarbeiter
        /// </summary>
        public int RefEmployeeId { get; set; }

        /// <summary>
        /// Mitarbeiter
        /// </summary>
        public virtual Employee Employee { get; set; }

        /// <summary>
        /// Referenz-Id Projekt
        /// </summary>
        public int RefProjectId { get; set; }

        /// <summary>
        /// Projekt
        /// </summary>
        public virtual Project Project { get; set; }

        /// <summary>
        /// Referenz-Id Projektrolle
        /// </summary>
        public int RefProjectRoleId { get; set; }

        /// <summary>
        /// Projektrolle
        /// </summary>
        public virtual ProjectRole ProjectRole { get; set; }
    }
}