using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Datalayer
{
    public static class DatabaseNames
    {
        private static string financialAnalysisDB = "FinancialAnalysisDB";

        public static string FinancialAnalysisDB { get => financialAnalysisDB; }
    }
}
