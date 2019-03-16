using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.CarPoolManagement
{
    /// <summary>
    /// Bauart
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class CarBody : BindableBase
    {
        /// <summary>
        /// Id der Bauart
        /// </summary>
        public int CarBodyId { get; set; }

        /// <summary>
        /// Name der Bauart
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Referenz-Id auf das Automodell
        /// </summary>
        public int RefCarModelId { get; set; }
    }
}
