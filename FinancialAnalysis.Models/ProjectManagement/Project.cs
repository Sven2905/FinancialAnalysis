using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;

namespace FinancialAnalysis.Models.ProjectManagement
{
    /// <summary>
    /// Projekt
    /// </summary>
    public class Project : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Name des Projekts
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Budget
        /// </summary>
        public decimal Budget { get; set; }

        /// <summary>
        /// Startdatum
        /// </summary>
        public DateTime StartDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Voraussichtliches Enddatum
        /// </summary>
        public DateTime ExpectedEndDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Tatsächliches Enddatum
        /// </summary>
        public DateTime TotalEndDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Abgeschlossen
        /// </summary>
        public bool IsEnded { get; set; }

        /// <summary>
        /// Tatsächlichen Kosten
        /// </summary>
        public decimal Costs { get; set; }

        /// <summary>
        /// Eindeutige Kennzeichnung, vom Benutzer frei wählbar
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Referenz-Id der Kostenstelle
        /// </summary>
        public int RefCostCenterId { get; set; }

        /// <summary>
        /// Kostenstelle
        /// </summary>
        public virtual CostCenter CostCenter { get; set; }

        /// <summary>
        /// Referenz-Id des Projektverantworlichen
        /// </summary>
        public int RefEmployeeId { get; set; }

        /// <summary>
        /// Projektverantworlicher
        /// </summary>
        public virtual Employee Employee { get; set; } // Leader

        /// <summary>
        /// Dem Projekt zugeordnete Mitarbeiter
        /// </summary>
        public virtual List<ProjectEmployeeMapping> ProjectEmployeeMappings { get; set; }

        /// <summary>
        /// Gebuchte Arbeitszeiten auf dem Projekt
        /// </summary>
        public virtual List<ProjectWorkingTime> ProjectWorkingTimes { get; set; }
    }
}