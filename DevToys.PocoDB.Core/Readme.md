
# DevToys.PocoDB.Core


The DbCommandOperation wraps around the System.Data.Common.DbCommand object and supports all it's features in a declaritive way.



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


### Define the Command

this object wraps to the DbCommand object.\
it consists of 2 parts:
- The DbCommand attribute defining the query and what kind of Command type it is.
- The DbParameter(s) attribute defining each parameter for the DbCommand. supports input and output.

The query language depends on the DbConnection type used in the configuration.

~~~cs

    [DBCommand(@"select id, [name], Adress, Country, ZipCode, HouseNumber, CompanyType, Text from dbo.Company where id = @Id", commandtype: CommandType.Text)]
    public class GetCompanyById
    {
        [DBParameter("Id")]
        public int Id { get; set; }
    }

~~~


### Define the Result Object

~~~cs

    public class Company
    {
        [DBField("Id")]
        public int Id { get; set; }

        [DBField("Name")]
        public string Name { get; set; }

        [DBField("Adress")]
        public string Adress { get; set; }

        [DBField("Country")]
        public string Country { get; set; }

        [DBField("ZipCode")]
        public string ZipCode { get; set; }

        [DBField("HouseNumber")]
        public string HouseNumber { get; set; }

        [DBField("CompanyType")]
        public CompanyType CompanyType { get; set; }

        [DBField("Text")]
        public string Text { get; set; }
    }

~~~


### Execute and consume the Query

The following query gives a single result object.

~~~cs

    var operation = new DbCommandOperation<Company, GetCompanyById>("MySqlConfig");
    Company _result = operation.ExecuteSingleReader(new GetCompanyById() { Id = 1 });

~~~

For resultsets use the ExecuteReader command.

~~~cs

    var operation = new DbCommandOperation<Company, GetCompanyAll>("MySqlConfig");
    IEnumerable<Company> _result = operation.ExecuteReader(new GetCompanyAll() { });
    var _resultMaterialized = _result.ToList();

~~~


## Inserting Random data

- the DBRandomParameter can be used to insert random data.
- in this example the Output parameter is used to retrieve the new id for the record.

~~~cs


   [DBCommand(@"insert into dbo.Company ([name], Adress, Country, ZipCode, HouseNumber, CompanyType, Text) 
                    values (@name, @Adress, @Country, @ZipCode, @HouseNumber, @CompanyType, @Text);
                 set @OutputId = @@IDENTITY", commandtype: CommandType.Text)]
    public class InsertCompanyRandom
    {
        [DBParameter("OutputId", Direction = ParameterDirection.Output)]
        public int Id { get; set; }

        [DBRandomParameter("name", RandomStringType = RandomStringType.FirstName )]
        public string Name { get; set; }

        [DBRandomParameter("Adress", RandomStringType = RandomStringType.Adress )]
        public string Adress { get; set; }

        [DBRandomParameter("Country", RandomStringType = RandomStringType.Country )]
        public string Country { get; set; }

        [DBRandomParameter("ZipCode", RandomStringType = RandomStringType.ZipCode )]
        public string ZipCode { get; set; }

        [DBRandomParameter("HouseNumber", RandomStringType = RandomStringType.Number)]
        public string HouseNumber { get; set; }

        [DBRandomParameter("CompanyType")]
        public CompanyType CompanyType { get; set; } = CompanyType.BV;

        [DBRandomParameter("Text", RandomStringType = RandomStringType.Text, Max = 20 )]
        public string Text { get; set; }
    }

~~~

### Inserting the random data

~~~cs

    var operation = new DbCommandOperation<InsertCompanyRandom>("MySqlConfig");

    InsertCompanyRandom parameters = new InsertCompanyRandom() { };

    for (int ii = 0; ii < 50; ii++)
    {
        operation.ExecuteNonQuery(parameters);
        int newId = parameters.Id;
    }

~~~


## Working with array parameters

Arrays can be used as parameters as well.

~~~cs

    [DBCommand(@"select * from dbo.Company where id in (select convert(int, [value]) from STRING_SPLIT (@ids, ','));", commandtype: CommandType.Text)]
    public class GetCompanies
    {
        [DBStringArrayParameter("Ids")]
        public int[] Id { get; set; }
    }

~~~

Executing a command with an array parameter.

~~~cs

    var operation = new DbCommandOperation<Company, GetCompanies>("MySqlConfig");
    var parameters = new GetCompanies() { Id = new int[] { 1, 3, 6, 9 } };
    IEnumerable<Company> _result = operation.ExecuteReader(parameters);

    var _resultMaterialized = _result.ToList();

~~~

Note: this sql example works only for Microsoft Sql Server.


## Inserting binary data

~~~cs

    [DBCommand("Insert into dbo.BinaryData (Name, Photo) values (@Name, @Photo);", commandtype: CommandType.Text)]
    public class InsertPhoto
    {
        [DBParameter("Name")]
        public string Name { get; set; }

        [DBParameter("Photo")]
        public byte[] Photo { get; set; }
    }

~~~

~~~cs

    var operation = new DbCommandOperation<InsertPhoto>("MySqlConfig");
    IEnumerable<InsertPhoto> commands = GetFiles().Select(p => new InsertPhoto() { Photo = File.ReadAllBytes(p.FullName), Name = p.Name });
    operation.ExecuteNonQuery(commands);

~~~
    
