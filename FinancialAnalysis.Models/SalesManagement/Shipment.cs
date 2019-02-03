﻿using System;
using DevExpress.Mvvm;
using Utilities;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class Shipment : BindableBase
    {
        public Shipment()
        {
            ShippedProducts = new SvenTechCollection<ShippedProduct>();
        }

        public int ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime ShipmentDate { get; set; }

        public SvenTechCollection<ShippedProduct> ShippedProducts { get; set; }
    }
}