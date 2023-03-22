
# DevToys.PocoDB.BulkInsert.Core

BulkInsert wraps arround BulkInsert features for Microsoft Sql-Server specific.


### Define Connectionstring configuration.

For type anything derived from DbConnection can be used. ( FbConnection, OleDbConnection, etc).

~~~cs

    DataConfiguration.Instance.Add<SqlConnection>(
        new ConnectionConfig()  
        {
            Key = "MySqlConfig", 
            ConnectionString = @"Server=LAPTOP-GUIDO\SQLEXPRESS;Database=Misc;Trusted_Connection=True;"  
        } 
    );
    
~~~

### BulkInsert data

~~~cs

    [DBBulkInsert("dbo.Company")]
    public class BulkCompany
    {
        [DBParameter("Id")]
        public int Id { get; set; } = 0;

        [DBParameter("Name")]
        public string Name { get; set; } = string.Empty;

        [DBParameter("Adress")]
        public string Adress { get; set; } = string.Empty;

        [DBParameter("Country")]
        public string Country { get; set; } = string.Empty;

        [DBParameter("ZipCode")]
        public string ZipCode { get; set; } = string.Empty;

        [DBParameter("HouseNumber")]
        public string HouseNumber { get; set; } = string.Empty;

        [DBParameter("CompanyType")]
        public CompanyType CompanyType { get; set; } = CompanyType.BV;
    }

~~~


~~~cs

    List<BulkCompany> _data = new List<BulkCompany>();

    for (int ii = 0; ii < 10000; ii++)
        _data.Add(new BulkCompany() { Name = "Guido", ZipCode = "4624JC", CompanyType = CompanyType.LLC });

    BulkInsertOperation<BulkCompany> operation = new BulkInsertOperation<BulkCompany>("Local", 2096);
    operation.Progress += Operation_Progress;

    operation.Insert(_data);
    
~~~

~~~cs

    private static void Operation_Progress(object sender, BulkInsertEventArgs e)
    {
        Console.WriteLine($"RowsProcessed: {e.RowsProcessed}");
    } 

~~~


### BulkInsert random data

~~~cs

    [DBBulkInsert("dbo.Company")]
    public class BulkCompanyRandom
    {
        [DBRandomParameter("Id")]
        public int Id { get; set; } = 0;

        [DBRandomParameter("Name", RandomStringType = RandomStringType.FullName)]
        public string Name { get; set; } = string.Empty;

        [DBRandomParameter("Adress", RandomStringType = RandomStringType.Adress)]
        public string Adress { get; set; } = string.Empty;

        [DBRandomParameter("Country", RandomStringType = RandomStringType.Country)]
        public string Country { get; set; } = string.Empty;

        [DBRandomParameter("ZipCode", RandomStringType = RandomStringType.ZipCode)]
        public string ZipCode { get; set; } = string.Empty;

        [DBRandomParameter("HouseNumber", RandomStringType = RandomStringType.Number, Min = 10, Max = 200)]
        public string HouseNumber { get; set; } = string.Empty;

        [DBRandomParameter("CompanyType")]
        public CompanyType CompanyType { get; set; } = CompanyType.BV;

        [DBParameter("Text")]
        public string Text { get; set; } = string.Empty;
    }
    
~~~

Execute to insert half a million random companies.

~~~cs

    BulkInsertOperation<BulkCompanyRandom> operation = new BulkInsertOperation<BulkCompanyRandom>("Local", 1024);
    operation.Progress += Operation_Progress;
    operation.Insert(500000);
    
~~~


