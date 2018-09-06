using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinancialAnalysis.Models.Models;

namespace FinancialAnalysis.Logic.Messages
{
    /// <summary>
    /// Klasse zum Öffnen des Hauptmenüs
    /// </summary>
    public class OpenKontenrahmenWindowMessage
    {
        public OpenKontenrahmenWindowMessage()
        {

        }

        public OpenKontenrahmenWindowMessage(AccountingType AccountingType)
        {
            this.AccountingType = AccountingType;
        }

        public AccountingType AccountingType { get; set; }
    }
}
