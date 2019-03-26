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

        public static IEnumerable<ScannedDocument> GetAll()
        {
            return WebApi.GetData<IEnumerable<ScannedDocument>>(controllerName);
        }

        public static ScannedDocument GetById(int id)
        {
            return WebApi.GetDataById<ScannedDocument>(controllerName, id);
        }

        public static int Insert(ScannedDocument ScannedDocument)
        {
            return WebApi.PostAsync(controllerName, ScannedDocument).Result;
        }

        public static int Insert(IEnumerable<ScannedDocument> ScannedDocuments)
        {
            return WebApi.PostAsync(controllerName, ScannedDocuments).Result;
        }

        public static bool Update(ScannedDocument ScannedDocument)
        {
            return WebApi.PutAsync(controllerName, ScannedDocument, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
