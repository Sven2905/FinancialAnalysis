using FinancialAnalysis.Models;

namespace FinancialAnalysis.Logic.Messages
{
    /// <summary>
    ///     Klasse zum Öffnen des Hauptmenüs
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