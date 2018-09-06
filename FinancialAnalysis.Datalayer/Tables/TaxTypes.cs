using Dapper;
using FinancialAnalysis.Datalayer.StoredProcedures;
using FinancialAnalysis.Models.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Datalayer.Tables
{
    public class TaxTypes : ITable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private TaxTypesStoredProcedures sp = new TaxTypesStoredProcedures();

        public TaxTypes()
        {
            TableName = "TaxTypes";
            CheckAndCreateTable();
        }

        public void CheckAndCreateStoredProcedures()
        {
            sp.CheckAndCreateProcedures();
        }

        /// <summary>
        /// Checks if table exists otherwise creates it
        /// </summary>
        public void CheckAndCreateTable()
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr = $"If not exists (select name from sysobjects where name = '{TableName}') CREATE TABLE {TableName}(Id int IDENTITY(1,1) PRIMARY KEY,Name char(50) NOT NULL,AmountOfTax decimal NOT NULL)";

                using (SqlCommand command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }

                log.Info($"Table '{TableName}' created successfully...");
            }
            catch (Exception e)
            {
                log.Error($"Exception occured while creating table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Seeds the table with initial data
        /// </summary>
        public void Seed()
        {
            List<TaxType> taxTypes = new List<TaxType>()
            {
                new TaxType() { Name = "Keine", AmountOfTax = 0 },
                new TaxType() { Name = "VSt. 7%", AmountOfTax = 7 },
                new TaxType() { Name = "VSt. normal", AmountOfTax = 19 },
                new TaxType() { Name = "Kfz. VSt. 50%", AmountOfTax = 50 },
                new TaxType() { Name = "USt. normal", AmountOfTax = 19 },
                new TaxType() { Name = "USt. 7%", AmountOfTax = 7 },
                new TaxType() { Name = "USt. 16% alt", AmountOfTax = 16 },
                new TaxType() { Name = "VSt. 16% alt", AmountOfTax = 16 },
                new TaxType() { Name = "igE 7%", AmountOfTax = 7 },
                new TaxType() { Name = "igE 16%", AmountOfTax = 19 },
                new TaxType() { Name = "USt./VSt. 19% §13b", AmountOfTax = 19 },
                new TaxType() { Name = "USt./VSt. 7% §13b", AmountOfTax = 7 },
                new TaxType() { Name = "USt. 19% §13b Inland (alt)", AmountOfTax = 19 },
            };

            using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                con.Execute("dbo.TaxTypes_Insert @Name, @AmountOfTax", taxTypes);
            }
        }

        /// <summary>
        /// Returns all TaxType records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TaxType> GetAllTaxTypes()
        {
            using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var output = con.Query<TaxType>("dbo.TaxTypes_GetAll");
                return output;
            }
        }

        /// <summary>
        /// Inserts the TaxType item
        /// </summary>
        /// <param name="taxType"></param>
        /// <returns>Id of inserted item</returns>
        public async Task<int> Insert(TaxType taxType)
        {
            using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var result = await con.QueryAsync("dbo.TaxTypes_Insert @Name, @AmountOfTax", taxType);
                return result.Single();
            }
        }

        /// <summary>
        /// Inserts the list of TaxType items
        /// </summary>
        /// <param name="taxTypes"></param>
        public void Insert(IEnumerable<TaxType> taxTypes)
        {
            using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                con.QueryAsync("dbo.TaxTypes_Insert @Name, @AmountOfTax", taxTypes);
            }
        }

        /// <summary>
        /// Returns TaxType by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TaxType GetById(int id)
        {
            using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                var output = con.QuerySingleOrDefault<TaxType>("dbo.TaxTypes_GetById @Id", new {Id = id });
                return output;
            }
        }

        /// <summary>
        /// Update TaxType, if not exist, insert it
        /// </summary>
        /// <param name="taxType"></param>
        public void UpdateOrInsert(TaxType taxType)
        {
            if (taxType.Id == 0 || GetById(taxType.Id) is null)
            {
                Insert(taxType);
                return;
            }

            using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                con.Execute("dbo.TaxTypes_Update @Id, @Name, @AmountOfTax", taxType);
            }
        }

        /// <summary>
        /// Update TaxTypes, if not exist insert them
        /// </summary>
        /// <param name="taxTypes"></param>
        public void UpdateOrInsert(IEnumerable<TaxType> taxTypes)
        {
            foreach (var taxType in taxTypes)
            {
                UpdateOrInsert(taxType);
            }
        }

        /// <summary>
        /// Update TaxType
        /// </summary>
        /// <param name="taxType"></param>
        public void Update(TaxType taxType)
        {
            if (taxType.Id == 0 || GetById(taxType.Id) is null)
            {
                return;
            }

            using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                con.Execute("dbo.TaxTypes_Update @Id, @Name, @AmountOfTax", taxType);
            }
        }

        /// <summary>
        /// Delete TaxType by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                con.Execute("dbo.TaxTypes_Delete @Id", new {Id = id});
            }
        }

        public string TableName { get; }
    }
}
