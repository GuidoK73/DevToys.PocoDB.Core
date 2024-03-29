﻿using DevToys.PocoDB.Core.Config;
using DevToys.PocoDB.Core.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocoDBConsoleAppTest.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DevToys.PocoDB.Core.UnitTests
{
    /// <summary>
    /// Wrapper arround System.Data.SqlClient.SqlBulkCopy
    /// </summary>
    [TestClass]
    public class TestBulkInsert
    {
        public TestBulkInsert()
        {
            DataConfiguration.Instance.Add<SqlConnection>( new ConnectionConfig() { Key = "Local", ConnectionString = @"Server=LAPTOP-GUIDO\SQLEXPRESS;Database=Misc;Trusted_Connection=True;" } );
        }

        [TestMethod]
        public void BulkInsertCompany()
        {
            List<BulkCompany> _data = new List<BulkCompany>();

            for (int ii = 0; ii < 10000; ii++)
                _data.Add(new BulkCompany() { Name = "Guido", ZipCode = "4624JC", CompanyType = CompanyType.LLC });

            BulkInsertOperation<BulkCompany> operation = new BulkInsertOperation<BulkCompany>("Local", 2096);
            operation.Progress += Operation_Progress;

            operation.Insert(_data);
        }

        [TestMethod]
        public void BulkInsertCompanyRandom()
        {
            BulkInsertOperation<BulkCompanyRandom> operation = new BulkInsertOperation<BulkCompanyRandom>("Local", 1024);
            operation.Progress += Operation_Progress;
            operation.Insert(500000);
        }

        private static void Operation_Progress(object sender, BulkInsertEventArgs e)
        {
            Console.WriteLine($"RowsProcessed: {e.RowsProcessed}");
        }
    }
}