using FinancialAnalysis.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.CarPoolManagement
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        public VehicleType VehicleType { get; set; }
        public FuelType FuelType { get; set; }
        public string LicenseNumber { get; set; }
        public string VehicleNumber { get; set; }
        public string Color { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public DateTime ConstructionDate { get; set; }
        public DateTime FirstRegistrationDate { get; set; }
        public int RefMarqueId { get; set; }
        public int RefVehicleModelId { get; set; }
        public decimal MilageOnAcquisition { get; set; }
        public decimal CurrentMilage { get; set; }
    }
}
