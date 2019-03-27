using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class ScannedDocuments
    {
        private const string controllerName = "ScannedDocuments";

        public static List<ScannedDocument> GetAll()
        {
            return WebApi<List<ScannedDocument>>.GetData(controllerName);
        }

        public static ScannedDocument GetById(int id)
        {
            return WebApi<ScannedDocument>.GetDataById(controllerName, id);
        }

        public static int Insert(ScannedDocument ScannedDocument)
        {
            return WebApi<int>.PostAsync(controllerName, ScannedDocument).Result;
        }

        public static int Insert(IEnumerable<ScannedDocument> ScannedDocuments)
        {
            return WebApi<int>.PostAsync(controllerName, ScannedDocuments).Result;
        }

        public static bool Update(ScannedDocument ScannedDocument)
        {
            return WebApi<bool>.PutAsync(controllerName, ScannedDocument, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id).Result;
        }
    }
}
