using DevExpress.Mvvm;
using FinancialAnalysis.Models.Enums;
using Newtonsoft.Json;
using System;

namespace FinancialAnalysis.Models.CarPoolManagement
{
    /// <summary>
    /// Fahrzeug
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class Vehicle : BindableBase
    {
        // Make - Model - Year - Body - Generation - Trim

        /// <summary>
        /// Id des Fahrzeugs
        /// </summary>
        public int VehicleId { get; set; }

        /// <summary>
        /// Automarke
        /// </summary>
        public CarMake CarMake { get; set; }

        /// <summary>
        /// Automodell
        /// </summary>
        public CarModel CarModel { get; set; }

        /// <summary>
        /// Bauart
        /// </summary>
        public CarBody CarBody { get; set; }

        /// <summary>
        /// Generation
        /// </summary>
        public CarGeneration CarGeneration { get; set; }

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
        /// Kilometerstand bei Anschaffung
        /// </summary>
        public decimal MilageOnAcquisition { get; set; }

        /// <summary>
        /// Aktueller Kilometerstand
        /// </summary>
        public decimal CurrentMilage { get; set; }
    }
}