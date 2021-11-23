using System;

namespace DevToys.PocoDB.Core.Attributes
{
    /// <summary>
    /// For bulk insert operations Schema name is case sensitive!.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DBBulkInsertAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        public DBBulkInsertAttribute(string tablename)
        {
            TableName = tablename;
        }

        /// <summary>
        /// Tablename to perform bulk insert operation on.
        /// </summary>
        public string TableName { get; private set; }

        public override string ToString() => TableName;
    }
}