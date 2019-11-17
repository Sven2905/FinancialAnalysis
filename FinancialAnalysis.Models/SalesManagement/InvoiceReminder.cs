using DevExpress.Mvvm;
using FinancialAnalysis.Models.Enums;
using Newtonsoft.Json;
using System;

namespace FinancialAnalysis.Models.SalesManagement
{
    /// <summary>
    /// Mahnung
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class InvoiceReminder : BaseClass
    {
        /// <summary>
        /// Id
        /// </summary>
        public int InvoiceReminderId { get; set; }

        /// <summary>
        /// Referenz-Id der Rechnung
        /// </summary>
        public int RefInvoiceId { get; set; }

        /// <summary>
        /// Erstellungsdatum
        /// </summary>
        public DateTime Date { get; set; } = DateTime.Now;

        /// <summary>
        /// Name des Benutzers, der die Mahnung erstellt hat
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Postalisch, Mail, Beides
        /// </summary>
        public ReminderType ReminderType { get; set; }

        /// <summary>
        /// Ist letzte Mahnung
        /// </summary>
        public bool IsLastReminder { get; set; } = false;

        /// <summary>
        /// Ist die Mahnung bezahlt
        /// </summary>
        public bool IsClosed { get; set; }
    }
}