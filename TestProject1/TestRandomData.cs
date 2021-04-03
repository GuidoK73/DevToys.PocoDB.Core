using DevToys.PocoDB.Core.Config;
using DevToys.PocoDB.Core.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocoDBConsoleAppTest.Data;

namespace DevToys.PocoDB.UnitTests
{
    [TestClass]
    public class TestRandomData
    {
        public TestRandomData()
        {
            DataConfiguration.Instance.Add(
                new ConnectionConfig()
                {
                    Name = "Local",
                    ConnectionTypeName = "SqlClient",
                    ConnectionString = @"Server=LAPTOP-GUIDO\SQLEXPRESS;Database=Misc;Trusted_Connection=True;"
                }
            );
        }

        [TestMethod]
        public void BulkInsertCompanyRandom()
        {
            BulkInsertOperation<BulkCompanyRandom> operation = new BulkInsertOperation<BulkCompanyRandom>("Local", 512);
            operation.Insert(1000);
        }

        [TestMethod()]
        public void TestInsertRandomData()
        {
            var operation = new DbCommandOperation<InsertCompanyRandom>("Local");

            InsertCompanyRandom parameters = new InsertCompanyRandom() { };

            for (int ii = 0; ii < 50; ii++)
            {
                operation.ExecuteNonQuery(parameters);
                int newId = parameters.Id;
            }
        }
    }
}