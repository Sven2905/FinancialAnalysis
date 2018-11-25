﻿using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models
{
    public class SelectedCostAccount
    {
        public CostAccount CostAccount { get; set; }
        public AccountingType AccountingType { get; set; }
    }
}