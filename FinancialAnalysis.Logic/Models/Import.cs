using Dapper;
using FinancialAnalysis.Logic.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.Models
{
    public static class Import
    {
        public static bool? ImportKontenrahmen()
        {
            bool? result = null;

            FinanceContext ctx = new FinanceContext();
            ctx.Configuration.AutoDetectChangesEnabled = false;
            ctx.Configuration.ValidateOnSaveEnabled = false;

            if (ctx.Kontenrahmen.Any())
            {
                List<Kontenrahmen> listKr = new List<Kontenrahmen>();

                string path = @"c:\skr.csv";

                try
                {
                    using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["FinanceContext"].ConnectionString))
                    {
                        using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                        {
                            while (sr.Peek() >= 0)
                            {
                                var line = sr.ReadLine();
                                var items = line.Split(';');

                                Kontenrahmen kr = new Kontenrahmen()
                                {
                                    Number = Convert.ToInt32(items[1]),
                                    Name = items[2],
                                    Type = Standardkontenrahmen.SKR03
                                };

                                listKr.Add(kr);

                                kr = new Kontenrahmen()
                                {
                                    Number = Convert.ToInt32(items[0]),
                                    Name = items[2],
                                    Type = Standardkontenrahmen.SKR04
                                };

                                listKr.Add(kr);
                            }
                        }


                        string sql = "SELECT * FROM CostAccounts JOIN CostAccountCategories ON CostAccounts.CostAccountCategoryId = CostAccountCategories.CostAccountCategoryId";
                        var test = db.Query(sql).ToList();
                        
                        foreach (var item in test)
                        {
                            var data = (IDictionary<string, object>)item;
                            object value = data["AccountNumber"];
                        }

                        string processQuery = "INSERT INTO Kontenrahmen (Name, Type, Number) VALUES (@Name, @Type, @Number)";
                        db.Execute(processQuery, listKr);


                        result = true;
                    }
                }
                catch (Exception e)
                {
                    result = false; ;
                }
            }
            return result;
        }
    }
}
