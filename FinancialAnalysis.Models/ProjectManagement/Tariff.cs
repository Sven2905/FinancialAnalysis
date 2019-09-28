using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FinancialAnalysis.Models.ProjectManagement
{
    /// <summary>
    /// Tarif
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class Tariff : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int TariffId { get; set; }

        /// <summary>
        /// Name des Tarifs
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mitarbeiter in diesem Tarif
        /// </summary>
        public virtual List<User> Users { get; set; }
    }
}