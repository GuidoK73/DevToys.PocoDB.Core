﻿using DevToys.PocoDB.Core.Config;
using DevToys.PocoDB.Core.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocoDBConsoleAppTest.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DevToys.PocoDB.Core.UnitTests
{
    /// <summary>
    /// DynamicSqlOperation allows you to access data without the need of predefined classes
    /// you just use dynamic object for results and parameter data.
    ///
    /// requirement:
    /// 1.  Field names and Parameter names must match with property names.
    ///
    /// Limitations:
    /// 1.  FieldEncryption not available.
    /// 2.  Output Parameters cannot be retrieved from procedures.
    /// </summary>
    [TestClass]
    public class TestDynamicOperation
    {
        public TestDynamicOperation()
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
        public void InsertDynamic()
        {
            var operation = new DynamicSqlOperation("Local");

            dynamic parameters = new
            {
                Id = 0,
                Name = "A Company Name",
                Country = "",
                HouseNumber = "",
                ZipCode = "4624JC",
                Adress = "",
                CompanyType = CompanyType.GMBH,
                Text = "Guido Kleijer"
            };

            operation.ExecuteNonQuery("InsertCompany", parameters, CommandType.StoredProcedure);
        }

        [TestMethod]
        public void SelectAllDynamic()
        {
            var operation = new DynamicSqlOperation("Local");
            IEnumerable<dynamic> result = operation.ExecuteReader("Select * from Company", CommandType.Text);

            var _resultMaterialized = result.ToList();
        }
    }
}