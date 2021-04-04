using DevToys.PocoDB.Core.Attributes;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DevToys.PocoDB.Core.Operations
{
    internal class BaseDataOperationHelper<TRESULTOBJECT>
    {
        private bool _Initialized = false;

        public BaseDataOperationHelper()
        {
        }

        public DBFieldAttribute[] Attributes { get; private set; }
        public PropertyInfo[] Properties { get; private set; }

        public void Initialize(IDataReader reader)
        {
            if (_Initialized)
                return;

            // Get Reader Column names ordered by ordinal
            string[] _readerFieldNames = DataUtils.GetReaderColumns(reader);

            // Create property and Attribute dictionaries
            var _attributes = new Dictionary<string, DBFieldAttribute>();
            var _properties = new Dictionary<string, PropertyInfo>();

            foreach (PropertyInfo property in typeof(TRESULTOBJECT).GetProperties())
            {
                DBFieldAttribute _attribute = property.GetCustomAttribute<DBFieldAttribute>(false);
                if (_attribute != null)
                {
                    string _key = _attribute.Field.ToLower();
                    _attributes.Add(_key, _attribute);
                    _properties.Add(_key, property);
                }
            }

            // Validate fieldsnames
            foreach (string column in _readerFieldNames)
            {
                if (!_properties.ContainsKey(column))
                {
                    string message = string.Format("Column '{0}' does not exist in the result DataSet.", column);
                    throw new DataException(message);
                }
            }

            // Convert Property / Attribute Dictionaries to Ordinal Array.
            Attributes = new DBFieldAttribute[_readerFieldNames.Length];
            Properties = new PropertyInfo[_readerFieldNames.Length];

            for (int index = 0; index < _readerFieldNames.Length; index++)
            {
                string _name = _readerFieldNames[index];
                Attributes[index] = _attributes[_name];
                Properties[index] = _properties[_name];
            }

            _Initialized = true;
        }
    }
}