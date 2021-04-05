using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DevToys.PocoDB.Core.Factory
{
    internal class ConnectionFactoryInitializer
    {
        public ConnectionFactoryInitializer() { }
        public Dictionary<string, Type> Init()
        {
            var _types = new Dictionary<string, Type>
            {
                { ConnectionTypeNameConstants.SqlClient, typeof(SqlConnection) },
            };

            // Add new types for other DB Drivers here!
            return _types;
        }
    }
}
