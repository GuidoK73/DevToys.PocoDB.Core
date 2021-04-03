using DevToys.PocoDB.Core;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Runtime.Serialization;

namespace DevToys.PocoDB.Core.Config
{
    public class DataConfiguration
    {
        private static DataConfiguration _Instance;

        private readonly Dictionary<string, ConnectionConfig> _Connections = new Dictionary<string, ConnectionConfig>();

        private DataConfiguration()
        {
        }

        /// <summary>
        /// Retuerns static global instance of DataConfiguration 
        /// </summary>
        public static DataConfiguration Instance => _Instance ?? (_Instance = new DataConfiguration());

        public ConnectionConfig Get(string name)
        {
            if (!_Connections.ContainsKey(name.ToLower()))
                throw new DataException(string.Format("Could not find Data Connection named: '{0}'", name));

            return _Connections[name.ToLower()];
        }

        public void Add(ConnectionConfig config)
        {
            if (_Connections.ContainsKey(config.Name.ToLower()))
                throw new DataException(string.Format("Could not add Data Connection named: '{0}'", config.Name));

            _Connections.Add(config.Name.ToLower(), config);
        }

        public void Remove(string name)
        {
            if (!_Connections.ContainsKey(name.ToLower()))
                throw new DataException(string.Format("Could not find Data Connection named: '{0}'", name));

            _Connections.Remove(name.ToLower());
        }

    }
}