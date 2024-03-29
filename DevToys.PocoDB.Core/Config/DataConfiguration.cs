﻿using DevToys.PocoDB.Core.Factory;
using System.Collections.Generic;
using System.Data.Common;

namespace DevToys.PocoDB.Core.Config
{
    /// <summary>
    /// Singleton instance holding all ConnectionConfig settings.
    /// </summary>
    public class DataConfiguration
    {
        private static DataConfiguration _Instance;

        private readonly Dictionary<string, ConnectionConfig> _Connections = new Dictionary<string, ConnectionConfig>();

        private DataConfiguration()
        {
        }

        /// <summary>
        /// Returns static global instance of DataConfiguration
        /// </summary>
        public static DataConfiguration Instance => _Instance ?? (_Instance = new DataConfiguration());

        public void Add<T>(ConnectionConfig config) where T : DbConnection
        {
            if (_Connections.ContainsKey(config.Key.ToLower()))
                throw new DataException(string.Format("Could not add Data Connection named: '{0}'", config.Key));

            _Connections.Add(config.Key.ToLower(), config);

            config.ConnectionTypeName = typeof(T).Name;

            ConnectionFactory.Instance.AddType<T>();
        }

        public ConnectionConfig Get(string key)
        {
            if (!_Connections.ContainsKey(key.ToLower()))
                throw new DataException(string.Format("Could not find Data Connection named: '{0}'", key));

            return _Connections[key.ToLower()];
        }

        public void Remove(string key)
        {
            if (!_Connections.ContainsKey(key.ToLower()))
                throw new DataException(string.Format("Could not find Data Connection named: '{0}'", key));

            _Connections.Remove(key.ToLower());
        }
    }
}