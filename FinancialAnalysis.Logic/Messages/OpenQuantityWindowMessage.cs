using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.Messages
{
    public class OpenQuantityWindowMessage
    {
        public OpenQuantityWindowMessage(int MaxQuantity)
        {
            this.MaxQuantity = MaxQuantity;
        }

        public int MaxQuantity { get; set; }
    }
}
