using System;
using System.Collections.Generic;
using System.Data.Common;


namespace DevToys.PocoDB.Core.Factory
{
    /// <summary>
    /// Default Connections: SqlClient
    /// </summary>
    public sealed class ConnectionFactory
    {
        private static ConnectionFactory _Instance;

        private Dictionary<string, Type> connectionTypes = new Dictionary<string, Type>();

        private ConnectionFactory() { }

        /// <summary>
        /// Returns singleton instance for ConnectionFactory.
        /// </summary>
        public static ConnectionFactory Instance => _Instance ?? (_Instance = new ConnectionFactory());

        /// <summary>
        /// Creates a DbConnection for connectionTypeName and sets the connectionString.
        /// </summary>
        /// <param name="connectionTypeName"></param>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        public DbConnection Create(string connectionTypeName, string connectionstring)
        {
            if (!connectionTypes.ContainsKey(connectionTypeName))
                throw new DataException($"The ClientType '{connectionTypeName}' does not exist. Please check Connection in app.config. Or add the desired DBConnection to the Connection Factory with the given name: '{connectionTypeName}'");

            return Activator.CreateInstance(connectionTypes[connectionTypeName], connectionstring) as DbConnection;
        }

        /// <summary>
        /// 
        /// </summary>
        public Type GetType(string connectionTypeName) => !connectionTypes.ContainsKey(connectionTypeName) ? null : connectionTypes[connectionTypeName];

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TCONNECTION">Type to add</typeparam>
        public void AddType<TCONNECTION>() where TCONNECTION : DbConnection
        {
            string _name = typeof(TCONNECTION).Name;

            if (connectionTypes.ContainsKey(_name))
                return;

            connectionTypes.Add(_name, typeof(TCONNECTION));
        }
    }
}