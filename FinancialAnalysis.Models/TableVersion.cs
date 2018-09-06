using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Models
{
    public class TableVersion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public DateTime LastModified { get; set; }
    }
}
