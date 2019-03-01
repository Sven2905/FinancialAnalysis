using FinancialAnalysis.Models.Enums;
using System;

namespace FinancialAnalysis.Models.CarPoolManagement
{
    /// <summary>
    /// Fahrzeug
    /// </summary>
    public class Car
    {
        // Make - Model - Year - Body - Generation - Trim

        /// <summary>
        /// Referenz-Id der Automarke
        /// </summary>
        public int RefCarMakeId { get; set; }
        
        /// <summary>
        /// Automarke
        /// </summary>
        public CarMake CarMake { get; set; }

        /// <summary>
        /// Referenz-Id des Automodells
        /// </summary>
        public int RefCarModelId { get; set; }

        /// <summary>
        /// Automodell
        /// </summary>
        public CarModel CarModel { get; set; }

        /// <summary>
        /// Referenz-Id der Bauart
        /// </summary>
        public int RefCarBodyId { get; set; }

        /// <summary>
        /// Bauart
        /// </summary>
        public CarBody CarBody { get; set; }

        /// <summary>
        /// Referenz-Id der Generation
        /// </summary>
        public int RefCarGenerationId { get; set; }

        /// <summary>
        /// Generation
        /// </summary>
        public CarGeneration CarGeneration { get; set; }

        /// <summary>
        /// Referenz-Id der Motorisierung
        /// </summary>
        public int RefCarTrimId { get; set; }

        /// <summary>
        /// Motorisierung
        /// </summary>
        public CarTrim CarTrim { get; set; }

        /// <summary>
        /// Referenz-Id des Motors
        /// </summary>
        public int RefCarEngineId { get; set; }

        /// <summary>
        /// Motor
        /// </summary>
        public CarEngine CarEngine { get; set; }

        /// <summary>
        /// Kennzeichen
        /// </summary>
        public string LicenseNumber { get; set; }

        /// <summary>
        /// Fahrzeugnummer
        /// </summary>
        public string VehicleNumber { get; set; }

        /// <summary>
        /// Farbe
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Anschaffungsdatum
        /// </summary>
        public DateTime AcquisitionDate { get; set; }

        /// <summary>
        /// Baujahr
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Datum der Erstzulassung
        /// </summary>
        public DateTime FirstRegistrationDate { get; set; }

        /// <summary>
        /// Referenz-Id der Automarke
        /// </summary>
        public int RefMarqueId { get; set; }

        /// <summary>
        /// Referenz-Id des Models
        /// </summary>
        public int RefVehicleModelId { get; set; }

        /// <summary>
        /// Kilometerstand bei Anschaffung
        /// </summary>
        public decimal MilageOnAcquisition { get; set; }

        /// <summary>
        /// Aktueller Kilometerstand
        /// </summary>
        public decimal CurrentMilage { get; set; }
    }
}