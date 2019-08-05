using System;

namespace FinancialAnalysis.Models.Administration
{
    [Serializable]
    public class DatabaseConfiguration
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
}