using DevToys.PocoDB.Core.Config;
using DevToys.PocoDB.Core.Enums;
using DevToys.PocoDB.Core.Factory;
using System;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace DevToys.PocoDB.Core.Operations
{
    public abstract class BaseDataOperation
    {
        /// <param name="configConnectionName">Points to ConnectionString Configuration in section DevToys.PocoDB in App.Config</param>
        protected BaseDataOperation(string configConnectionName) => Config = DataConfiguration.Instance.Get(configConnectionName);

        protected BaseDataOperation() => Config = null;

        protected BaseDataOperation(ConnectionConfig config) => Config = config;

        /// <param name="configConnectionName">Points to ConnectionString Configuration in section DevToys.PocoDB in App.Config</param>
        protected BaseDataOperation(DbConnectionStringBuilder connectionString, string configConnectionName) => Config = new ConnectionConfig() { ConnectionString = connectionString.ToString(), ConnectionTypeName = configConnectionName };

        /// <summary>Raised prior to command execution. Use it to set provider specific properties for command and connection if nececary. </summary>
        public event EventHandler<DataOperationPreExecute> PreExecute = delegate { };

        protected ConnectionConfig Config { get; private set; }

        public DbConnection CreateConnection() => ConnectionFactory.Instance.Create(Config.ConnectionTypeName, Config.ConnectionString);

        /// <summary>Call before invoking command.Execute etc. </summary>
        protected void RaisePreExecute(DbConnection connection, DbCommand command) => PreExecute?.Invoke(this, new DataOperationPreExecute() { Connection = connection, Command = command });
    }

    /// <typeparam name="TRESULTOBJECT">The Result Object Type either as single object.</typeparam>
    public abstract class BaseDataOperation<TRESULTOBJECT> : BaseDataOperation
        where TRESULTOBJECT : class, new()
    {
        private BaseDataOperationHelper<TRESULTOBJECT> _Helper = null;

        /// <param name="configConnectionName">Reference to connection in DevToys.PocoDB config section</param>
        protected BaseDataOperation(string configConnectionName) : base(configConnectionName) { }

        /// <param name="config"></param>
        protected BaseDataOperation(ConnectionConfig config) : base(config) { }

        protected BaseDataOperation() : base()
        {
        }

        /// <param name="configConnectionName">Points to ConnectionString Configuration in section DevToys.PocoDB in App.Config</param>
        protected BaseDataOperation(DbConnectionStringBuilder connectionString, string configConnectionName) : base(connectionString, configConnectionName) { }

        /// <summary>Reads a datarow and converts it to TObject</summary>
        protected TRESULTOBJECT ReadDataRow(IDataReader reader)
        {
            Init(reader);

            // Create new object
            var _result = new TRESULTOBJECT();
            // create a new base object so we can invoke base methods.
            for (int index = 0; index < reader.FieldCount; index++)
                SetPropertyValue(_Helper.Properties[index], _result, reader.GetValue(index), reader.GetFieldType(index), _Helper.Attributes[index].ReaderDefaultValue, _Helper.Attributes[index].StrictMapping);

            return _result;
        }

        private void Init(IDataReader reader)
        {
            if (_Helper != null)
                return;

            Type _type = typeof(BaseDataOperationHelper<TRESULTOBJECT>);
            _Helper = (BaseDataOperationHelper<TRESULTOBJECT>)CommandCache.Instance.Get(_type);

            if (_Helper == null)
            {
                _Helper = new BaseDataOperationHelper<TRESULTOBJECT>();
                _Helper.Initialize(reader);

                CommandCache.Instance.Register(_type, _Helper);
            }
        }

        private void SetPropertyValue(PropertyInfo propertyInfo, TRESULTOBJECT dataobject, object value, Type valueType, object defaultvalue, StrictMapping strictField)
        {
            if (value == DBNull.Value || value == null)
            {
                if (propertyInfo.PropertyType != typeof(string))
                {
                    if (Nullable.GetUnderlyingType(propertyInfo.PropertyType) == null)
                        throw new DataException("Property {0} cannot contain null value", propertyInfo.Name);
                }

                propertyInfo.SetValue(dataobject, defaultvalue, null);
                return;
            }

            if (propertyInfo.PropertyType.IsEnum)
            {
                propertyInfo.SetValue(dataobject, Enum.Parse(propertyInfo.PropertyType, System.Convert.ToString(value)), null);
                return;
            }

            if ((strictField == StrictMapping.ByConfigSetting && Config.StrictMapping) || strictField == StrictMapping.True)
            {
                if (propertyInfo.PropertyType == valueType)
                {
                    propertyInfo.SetMethod.Invoke(dataobject, new object[] { value });
                    return;
                }

                throw new DataException("Property '{0}': Type cannot be mapped from '{1}' to '{2}' ", propertyInfo.Name, valueType, propertyInfo.PropertyType);
            }

            if (propertyInfo.PropertyType == valueType)
                propertyInfo.SetMethod.Invoke(dataobject, new object[] { value });
            else
                propertyInfo.SetMethod.Invoke(dataobject, new object[] { Convert.ChangeType(value, propertyInfo.PropertyType, Config.CultureInfo) });
        }
    }
}