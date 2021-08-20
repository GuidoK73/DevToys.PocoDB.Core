using DevToys.PocoDB.Core.Config;
using DevToys.PocoDB.Core.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocoDBConsoleAppTest.Data;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;

namespace DevToys.PocoDB.Core.UnitTests
{
    [TestClass]
    public class TestCommandOperation2
    {
        public TestCommandOperation2()
        {
            DataConfiguration.Instance.Add<OleDbConnection>(new ConnectionConfig(){ Key = "LocalOleDb", ConnectionString = @"Provider=MSOLEDBSQL;Server=LAPTOP-GUIDO\SQLEXPRESS;Database=Misc;Integrated Security=SSPI;" } );          
            DataConfiguration.Instance.Add<OleDbConnection>(new ConnectionConfig(){ Key = "LocalOleDb2", ConnectionString = @"Provider=MSOLEDBSQL;Data Source=LAPTOP-GUIDO\SQLEXPRESS;Database=Misc;Integrated Security=SSPI;" });
        }

        [TestMethod]
        public void GetCompanyById()
        {
            // NOTE:
            // Each construction of DbCommandOperation means it does initializion on first call.
            // when using multiple times it's best to set the declaration on class level instead of method level.

            var operation = new DbCommandOperation<Company, GetCompanyById>("LocalOleDb");
            Company _result = operation.ExecuteSingleReader(new GetCompanyById() { Id = 1 });

            var operation2 = new DbCommandOperation<Company, GetCompanyById>("LocalOleDb");
            Company _result2 = operation2.ExecuteSingleReader(new GetCompanyById() { Id = 1 });


        }

        [TestMethod]
        public void GetCompanyById_Sql()
        {
            var operation = new DbCommandOperation<Company, GetCompanyById_Sql>("LocalOleDb");
            Company _result = operation.ExecuteSingleReader(new GetCompanyById_Sql() { Id = 1 });
        }

        [TestMethod]
        public void InsertCompany()
        {
            var operation = new DbCommandOperation<InsertCompanyByProcedure>("LocalOleDb");

            InsertCompanyByProcedure parameters = new InsertCompanyByProcedure()
            {
                Adress = "",
                Country = "NLD",
                HouseNumber = "181",
                Name = "A Company Name",
                ZipCode = "4624JC",
                CompanyType = CompanyType.NV,
                Text = "My Name 2"
            };

            operation.ExecuteNonQuery(parameters);

            int newId = parameters.Id;
        }

        [TestMethod]
        public void InsertCompany2()
        {
            var operation = new DbCommandOperation<InsertCompanyBySqlStatement>("LocalOleDb");

            InsertCompanyBySqlStatement parameters = new InsertCompanyBySqlStatement()
            {
                Adress = "",
                Country = "NLD",
                HouseNumber = "180",
                Name = "A Company Name",
                ZipCode = "4624JC",
                CompanyType = CompanyType.NV,
                Text = "My Name"
            };

            operation.ExecuteNonQuery(parameters);

            int newId = parameters.Id;
        }

        [TestMethod]
        public void SelectAll()
        {
            var operation = new DbCommandOperation<Company, GetCompanyAll>("LocalOleDb");
            IEnumerable<Company> _result = operation.ExecuteReader(new GetCompanyAll() { });
            var _resultMaterialized = _result.ToList();

            var operation2 = new DbCommandOperation<Company, GetCompanyAll>("LocalOleDb2");
            IEnumerable<Company> _result2 = operation2.ExecuteReader(new GetCompanyAll() { });
            var _resultMaterialized2 = _result2.ToList();

            Console.WriteLine("X");

        }

        [TestMethod]
        public void TestDBStringArrayParameter()
        {
            var operation = new DbCommandOperation<Company, GetCompanies>("LocalOleDb");
            var parameters = new GetCompanies() { Id = new List<int> { 1, 3, 6, 9 } };
            IEnumerable<Company> _result = operation.ExecuteReader(parameters);

            var _resultMaterialized = _result.ToList();
        }


        [TestMethod]
        public void InsertNull()
        {
            var insert = new InsertCompanyBySqlStatement()
            {
                Name = "First Company",
                Adress = "New Adres",
                Country = "NL",
                HouseNumber = "12",
                CompanyType = CompanyType.BV,
                Text = null,
                ZipCode = "ABCD12"
            };
            var operationInsert = new DbCommandOperation<InsertCompanyBySqlStatement>("LocalOleDb");
            operationInsert.ExecuteNonQuery(insert);

        }

        [TestMethod]
        public void TestDBStringArrayParameter2()
        {
            var operation = new DbCommandOperation<Company, GetCompanies2>("LocalOleDb");
            var parameters = new GetCompanies2() { Id = new int[] { 1, 3, 6, 9 } };
            IEnumerable<Company> _result = operation.ExecuteReader(parameters);

            var _resultMaterialized = _result.ToList();
        }
    }
}