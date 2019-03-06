﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.CarPoolManagement
{
    /// <summary>
    /// Automodell
    /// </summary>
    public class CarModel
    {
        /// <summary>
        /// Id des Automodells
        /// </summary>
        public int CarModelId { get; set; }

        /// <summary>
        /// Name des Automodells
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Referenz-Id auf Automarke
        /// </summary>
        public int RefCarMakeId { get; set; }
    }
}