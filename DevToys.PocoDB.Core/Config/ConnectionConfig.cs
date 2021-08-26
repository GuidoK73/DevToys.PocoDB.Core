using System.Data.Common;
using System.Globalization;

namespace DevToys.PocoDB.Core.Config
{
    public class ConnectionConfig
    {
        /// <summary>
        /// Connection string to use, depending on ConnectionType used.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Connection type name as registered in ConnectionFactory (SqlClient is pre registered).
        /// </summary>
        public string ConnectionTypeName { get; set; } = "SqlClient";

        /// <summary>
        /// When strict mapping set to false, Conversion to string will be based on this culture, default: CultureInfo.CurrentCulture 
        /// </summary>
        public CultureInfo CultureInfo { get; set; } = CultureInfo.CurrentCulture;

        /// <summary>
        /// Key is required and must be unique, this is used for the engine to retrieve the connection string And create an instance of the DbConnection specified by ConnectionTypeName.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// When true, DB Values returned in the resultset must match the property type, when false, values will always be converted to the property type.
        /// Note: this only applies to DBField attribute, ParameterAttributes are always strict.
        /// </summary>
        public bool StrictMapping { get; set; } = true;
    }

}