using System.Globalization;
using System.Runtime.Serialization;

namespace DevToys.PocoDB.Core.Config
{
    [DataContract()]
    public class ConnectionConfig
    {
        public string ConnectionString { get; set; }

        /// <summary>
        /// Connection type name as registered in ConnectionFactory
        /// </summary>
        public string ConnectionTypeName { get; set; } = "SqlClient";

        public CultureInfo CultureInfo { get; set; } = CultureInfo.CurrentCulture;

        public string Key { get; set; }

        public bool StrictMapping { get; set; } = false;
    }
}