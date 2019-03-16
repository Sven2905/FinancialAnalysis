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
    /// Generation
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class CarGeneration : BindableBase
    {
        /// <summary>
        /// Id der Generation
        /// </summary>
        public int CarGenerationId { get; set; }

        /// <summary>
        /// Name der Generation
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Referenz-Id auf die Bauart
        /// </summary>
        public int RefCarBodyId { get; set; }
    }
}
