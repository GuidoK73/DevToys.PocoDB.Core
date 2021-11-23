using DevToys.PocoDB.Core.Config;
using DevToys.PocoDB.Core.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocoDBConsoleAppTest.Data;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace DevToys.PocoDB.Core.UnitTests
{
    [TestClass]
    public class TestCommandOperationTransaction
    {
        public TestCommandOperationTransaction()
        {
            DataConfiguration.Instance.Add<SqlConnection>(
                new ConnectionConfig()
                {
                    Key = "Local",
                    ConnectionString = @"Server=LAPTOP-GUIDO\SQLEXPRESS;Database=Misc;Trusted_Connection=True;"
                }
            );
        }

        [TestMethod]
        public void InsertCompanyTransactional()
        {
            // Delete all companies outside Transaction.
            var deleteoperation = new DbCommandOperation<DeleteAllCompanies>("Local");
            deleteoperation.ExecuteNonQuery();

            var operation1 = new DbCommandOperation<InsertCompanyByProcedure>("Local");
            var operation2 = new DbCommandOperation<InsertCompanyBySqlStatement>("Local");

            using (DbConnection connection = operation1.CreateConnection())
            {
                // operation1.CreateConnection() : operation1 determines the connection to use.
                // the result is the same as  operation2.CreateConnection(), they both create a ConnectionObject from Config "Local"

                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        InsertCompanyByProcedure insert1 = new InsertCompanyByProcedure()
                        {
                            Adress = "",
                            Country = "NLD",
                            HouseNumber = "",
                            Name = "A Company Name",
                            ZipCode = "4555AA",
                            CompanyType = CompanyType.NV,
                            Text = "Guido Kleijer2"
                        };

                        operation1.ExecuteNonQuery(connection, transaction, insert1);

                        int newId = insert1.Id;

                        // Too large ZipCode
                        InsertCompanyBySqlStatement insert2 = new InsertCompanyBySqlStatement()
                        {
                            Adress = "",
                            Country = "NLD",
                            HouseNumber = "",
                            Name = "A Company Name ",
                            ZipCode = "4555AA TO LONG AAAAAAAAAAAbbbbbbbbbbbAAAAAAAAA TO LONG",
                            CompanyType = CompanyType.NV,
                            Text = "Guido Kleijer"
                        };

                        operation2.ExecuteNonQuery(connection, transaction, insert2);

                        int newId2 = insert2.Id;

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
                connection.Close();
            }
        }
    }
}