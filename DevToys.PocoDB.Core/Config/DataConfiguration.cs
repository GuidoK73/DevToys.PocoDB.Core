using System.Collections.Generic;

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

        public void Add(ConnectionConfig config)
        {
            if (_Connections.ContainsKey(config.Key.ToLower()))
                throw new DataException(string.Format("Could not add Data Connection named: '{0}'", config.Key));

            _Connections.Add(config.Key.ToLower(), config);
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