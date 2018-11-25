﻿using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    public class TaxType : BindableBase
    {
        public int TaxTypeId { get; set; }
        public string Description { get; set; }
        public string DescriptionShort { get; set; }
        public decimal AmountOfTax { get; set; }
        public TaxCategory TaxCategory { get; set; } // Steuerart
        public int RefAccountNumber { get; set; } // Das Umsatzsteuer- oder Vorsteuerkonto, auf das der Steuerbetrag zum Steuersatz gebucht wird
        public int RefAccountNotPayable { get; set; } // Haben Sie die Art ‚i.g.E‘ oder ’13b‘ gewählt, ist ein Vorsteuerkontos in der Spalte USt-Konto EU Bau einzutragen
        public int RefCostAccount { get; set; }
        public CostAccount CostAccount { get; set; }
    }
}