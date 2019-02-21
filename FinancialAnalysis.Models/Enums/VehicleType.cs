using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Enums
{
    public enum VehicleType
    {
        [Display(Name = "PKW")] Automobile,
        [Display(Name = "LKW")] Truck
    }
}
