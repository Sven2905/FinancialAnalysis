using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.Models.ProjectManagement
{
    /// <summary>
    /// Mapping between employee and project and role for employee
    /// </summary>
    public class ProjectEmployeeMapping
    {
        public int ProjectEmployeeMappingId { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public int ProjectRoleId { get; set; }
        public virtual ProjectRole ProjectRole { get; set; }
    }
}
