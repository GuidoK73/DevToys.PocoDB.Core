using DevToys.PocoDB.Core.Config;
using DevToys.PocoDB.Core.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocoDBConsoleAppTest.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DevToys.PocoDB.Core.UnitTests
{
    [TestClass]
    public class TestCommandOperation
    {
        public TestCommandOperation()
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
        public void GetCompanyById()
        {
            // NOTE:
            // Each construction of DbCommandOperation means it does initializion on first call.
            // when using multiple times it's best to set the declaration on class level instead of method level.

            var operation = new DbCommandOperation<Company, GetCompanyById>("Local");
            Company _result = operation.ExecuteSingleReader(new GetCompanyById() { Id = 1 });

            var operation2 = new DbCommandOperation<Company, GetCompanyById>("Local");
            Company _result2 = operation2.ExecuteSingleReader(new GetCompanyById() { Id = 1 });


        }

        [TestMethod]
        public void GetCompanyById_Sql()
        {
            var operation = new DbCommandOperation<Company, GetCompanyById_Sql>("Local");
            Company _result = operation.ExecuteSingleReader(new GetCompanyById_Sql() { Id = 1 });
        }

        [TestMethod]
        public void InsertCompany()
        {
            var operation = new DbCommandOperation<InsertCompanyByProcedure>("Local");

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
            var operation = new DbCommandOperation<InsertCompanyBySqlStatement>("Local");

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
            var operation = new DbCommandOperation<Company, GetCompanyAll>("Local");
            IEnumerable<Company> _result = operation.ExecuteReader(new GetCompanyAll() { });
            var _resultMaterialized = _result.ToList();

            var operation2 = new DbCommandOperation<Company, GetCompanyAll>("Local");
            IEnumerable<Company> _result2 = operation2.ExecuteReader(new GetCompanyAll() { });
            var _resultMaterialized2 = _result2.ToList();
        }

        [TestMethod]
        public void TestDBStringArrayParameter()
        {
            var operation = new DbCommandOperation<Company, GetCompanies>("Local");
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
            var operationInsert = new DbCommandOperation<InsertCompanyBySqlStatement>("Local");
            operationInsert.ExecuteNonQuery(insert);


        }

        [TestMethod]
        public void TestDBStringArrayParameter2()
        {
            var operation = new DbCommandOperation<Company, GetCompanies2>("Local");
            var parameters = new GetCompanies2() { Id = new int[] { 1, 3, 6, 9 } };
            IEnumerable<Company> _result = operation.ExecuteReader(parameters);

            var _resultMaterialized = _result.ToList();
        }
    }
}