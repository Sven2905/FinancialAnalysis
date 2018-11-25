﻿using System.Collections.Generic;

namespace FinancialAnalysis.Models
{
    public class Tariff
    {
        public int TariffId { get; set; }
        public string Name { get; set; }

        public virtual List<Employee> Employees { get; set; }
    }
}