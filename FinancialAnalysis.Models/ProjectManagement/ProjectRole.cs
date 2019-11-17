using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.ProjectManagement
{
    /// <summary>
    /// Projektrolle
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class ProjectRole : BaseClass
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ProjectRoleId { get; set; }

        /// <summary>
        /// Name der Rolle
        /// </summary>
        public string Name { get; set; }
    }
}