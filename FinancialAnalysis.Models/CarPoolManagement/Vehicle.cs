using FinancialAnalysis.Models.Enums;
using System;

namespace FinancialAnalysis.Models.CarPoolManagement
{
    /// <summary>
    /// Fahrzeug
    /// </summary>
    public class Vehicle
    {
        /// <summary>
        /// Id
        /// </summary>
        public int VehicleId { get; set; }

        /// <summary>
        /// Fahrzeugtyp
        /// </summary>
        public VehicleType VehicleType { get; set; }

        /// <summary>
        /// Kraftstoffart
        /// </summary>
        public FuelType FuelType { get; set; }

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