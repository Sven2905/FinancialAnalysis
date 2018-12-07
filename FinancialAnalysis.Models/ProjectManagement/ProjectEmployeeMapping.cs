using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.ProjectManagement
{
    /// <summary>
    /// Mapping between employee, project and role
    /// </summary>
    public class ProjectEmployeeMapping
    {
        public int ProjectEmployeeMappingId { get; set; }
        public int RefEmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public int RefProjectId { get; set; }
        public virtual Project Project { get; set; }
        public int RefProjectRoleId { get; set; }
        public virtual ProjectRole ProjectRole { get; set; }
    }
}
