using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.ShipmentManagement
{
    public class ShipmentType : BindableBase
    {
        public int ShipmentTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
