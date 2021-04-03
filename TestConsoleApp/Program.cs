using DevToys.PocoDB.Core.COnfig;
using DevToys.PocoDB.Core.Attributes;
using DevToys.PocoDB.Core.Operations;
using System.Data;

namespace TestConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Global initialization

            DataConfiguration.Instance.Add(
                new ConnectionConfig()
                {
                    Name = "Connection1",
                    ConnectionType = "SqlClient",
                    StrictMapping = false,
                    ConnectionString = ""
                }
            );

            var operation = new DbCommandOperation<Company, GetCompanyById>("Connection1");
            Company _result = operation.ExecuteSingleReader(new GetCompanyById() { Id = 1 });
        }

        public class Company
        {
            [DBField("Id")]
            public int Id { get; set; }

            [DBField("Name")]
            public string Name { get; set; }
        }

        [DBCommand(@"Select * from Companies where Id = @id", commandtype: CommandType.Text)]
        public class GetCompanyById
        {
            [DBParameter("Id")]
            public int Id { get; set; }
        }
    }
}