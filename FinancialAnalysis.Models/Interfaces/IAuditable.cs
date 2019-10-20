using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Interfaces
{
    public interface IAuditable
    {
        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity update
        /// </summary>
        DateTime UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets the userId of entity creation
        /// </summary>
        int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the userId of entity update
        /// </summary>
        int UpdatedBy { get; set; }
    }
}
