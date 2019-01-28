using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Datalayer.StoredProcedures;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.ClientManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.ClientManagement
{
    public class Clients : ITable
    {
        private readonly ClientsStoredProcedures sp = new ClientsStoredProcedures();

        public Clients()
        {
            TableName = "Clients";
            CheckAndCreateTable();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\Tables.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();
        }

        public string TableName { get; }

        public void CheckAndCreateStoredProcedures()
        {
            sp.CheckAndCreateProcedures();
        }

        public void CheckAndCreateTable()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"If not exists (select name from sysobjects where name = '{TableName}') CREATE TABLE {TableName}(" +
                    "ClientId int IDENTITY(1,1) PRIMARY KEY," +
                    "Name nvarchar(50) NOT NULL," +
                    "Street nvarchar(50) NOT NULL," +
                    "Postcode int NOT NULL," +
                    "City nvarchar(50) NOT NULL," +
                    "Phone nvarchar(50)," +
                    "Fax nvarchar(50)," +
                    "Mail nvarchar(50)," +
                    "IBAN nvarchar(50)," +
                    "BIC nvarchar(50)," +
                    "BankName nvarchar(50)," +
                    "FederalState int)";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns all Client records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Client> GetAll()
        {
            IEnumerable<Client> output = new List<Client>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<Client>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the Client item
        /// </summary>
        /// <param name="Client"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Client Client)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result =
                        con.Query<int>(
                            $"dbo.{TableName}_Insert @Name, @Street, @Postcode, @City, @Phone, @Fax, @Mail, @IBAN, @BIC, @BankName, @FederalState",
                            Client);
                    id = result.Single();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }

            return id;
        }

        /// <summary>
        ///     Inserts the list of Client items
        /// </summary>
        /// <param name="Client"></param>
        public void Insert(IEnumerable<Client> Clients)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var Client in Clients)
                        con.Query(
                            $"dbo.{TableName}_Insert @Name, @Street, @Postcode, @City, @Phone, @Fax, @Mail, @IBAN, @BIC, @BankName, @FederalState",
                            Client);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns Client by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Client GetById(int id)
        {
            var output = new Client();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<Client>($"dbo.{TableName}_GetById @ClientId",
                        new {ClientId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update Client, if not exist, insert it
        /// </summary>
        /// <param name="Client"></param>
        public void UpdateOrInsert(Client Client)
        {
            if (Client.ClientId == 0 || GetById(Client.ClientId) is null)
            {
                Insert(Client);
                return;
            }

            Update(Client);
        }

        /// <summary>
        ///     Update Clients, if not exist insert them
        /// </summary>
        /// <param name="Clients"></param>
        public void UpdateOrInsert(IEnumerable<Client> Clients)
        {
            foreach (var Client in Clients) UpdateOrInsert(Client);
        }

        /// <summary>
        ///     Update Client
        /// </summary>
        /// <param name="Client"></param>
        public void Update(Client Client)
        {
            if (Client.ClientId == 0 || GetById(Client.ClientId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute(
                        $"dbo.{TableName}_Update @ClientId, @Name, @Street, @Postcode, @City, @Phone, @Fax, @Mail, @IBAN, @BIC, @BankName, @FederalState",
                        Client);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete Client by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @ClientId", new {ClientId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Checks if Client has Creditor or Debitor
        /// </summary>
        /// <param name="id"></param>
        public bool IsClientInUse(int id)
        {
            var IsInUse = true;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    IsInUse = con.ExecuteScalar<bool>($"dbo.{TableName}_IsClientInUse @ClientId",
                        new {ClientId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'IsClientInUse' from table '{TableName}'", e);
            }

            return IsInUse;
        }
    }
}