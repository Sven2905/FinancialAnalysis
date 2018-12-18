using System;

namespace FinancialAnalysis.Models.ProjectManagement
{
    public class ProjectWorkingTime
    {
        public int ProjectWorkingTimeId { get; set; }
        public int RefEmployeeId { get; set; }
        public int RefProjectId { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now;
        public int Breaktime { get; set; }
    }
}