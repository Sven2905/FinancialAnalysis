using System;

namespace FinancialAnalysis.Models
{
    public class TableVersion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public DateTime LastModified { get; set; }
    }
}