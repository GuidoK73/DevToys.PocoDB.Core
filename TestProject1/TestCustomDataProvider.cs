using DevToys.PocoDB.Core;
using DevToys.PocoDB.Core.Config;
using DevToys.PocoDB.Core.Factory;
using DevToys.PocoDB.Core.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocoDBConsoleAppTest.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DevToys.PocoDB.Core.UnitTests
{
    /// <summary>
    /// Use ConnectionFactory.Instance to add a Custom Connection Object.
    ///
    /// In the config you can use the ClientName ( 'MyCustomClient' ) for the ConnectionType property.
    /// in this example we use SqlConnection as a custom DataProvider.
    /// </summary>
    [TestClass]
    public class TestCustomDataProvider
    {
        public TestCustomDataProvider()
        {
            ConnectionFactory.Instance.AddType<SqlConnection>("MyCustomClient");

            DataConfiguration.Instance.Add(
                new ConnectionConfig()
                {
                    Key = "Local",
                    ConnectionTypeName = "MyCustomClient",
                    ConnectionString = @"Server=LAPTOP-GUIDO\SQLEXPRESS;Database=Misc;Trusted_Connection=True;"
                }
            );
        }

        [TestMethod()]
        public void TestCreateNewDataProvider()
        {
            var operation = new DbCommandOperation<Company, GetCompanyAll>("Local");
            operation.PreExecute += Operation_PreExecute;
            IEnumerable<Company> _result = operation.ExecuteReader(new GetCompanyAll() { });

            var _resultMaterialized = _result.ToList();
        }

        /// PreExecute event can be used to alter Command and Connection properties prior to execution, this may be needed when using custom data providers.
        private void Operation_PreExecute(object sender, DataOperationPreExecute e)
        {
            SqlCommand command = e.Command as SqlCommand;
            SqlConnection connection = e.Connection as SqlConnection;
        }
    }
}