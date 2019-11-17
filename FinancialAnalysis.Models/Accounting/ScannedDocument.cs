using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Gespeichertes Dokument
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class ScannedDocument : BaseClass
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ScannedDocumentId { get; set; }

        /// <summary>
        /// Daten
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// Dateiname
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Pfad
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Datum
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Referenz-Id der Buchung
        /// </summary>
        public int RefBookingId { get; set; }

        /// <summary>
        /// Ausgabe für Tooltip: Dateiname und Datum
        /// </summary>
        public string ToolTip => $"Dateiname: {FileName}" + Environment.NewLine + $"Datum: {Date.ToShortDateString()}";
    }
}