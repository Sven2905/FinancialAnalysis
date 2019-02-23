using System;

namespace FinancialAnalysis.Models.ProjectManagement
{
    /// <summary>
    /// Arbeitszeit am Projekt
    /// </summary>
    public class ProjectWorkingTime
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ProjectWorkingTimeId { get; set; }

        /// <summary>
        /// Referenz-Id Mitarbeiter
        /// </summary>
        public int RefEmployeeId { get; set; }

        /// <summary>
        /// Referenz-Id Projekt
        /// </summary>
        public int RefProjectId { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Startzeit
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Endzeit
        /// </summary>
        public DateTime EndTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Pause
        /// </summary>
        public int Breaktime { get; set; }
    }
}