﻿using System;
using System.Data;

namespace DevToys.PocoDB.Core.Attributes
{
    /// <summary>
    /// Defines how TCOMMAND for DBCommandOperation is used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DBCommandAttribute : Attribute
    {
        /// <param name="commandText">sql statement / procedurename / table name depending on commandtype</param>
        public DBCommandAttribute(string commandText)
        {
            CommandText = commandText;
            CommandType = CommandType.Text;
        }

        /// <param name="commandText">sql statement / procedurename / table name depending on commandtype</param>
        public DBCommandAttribute(string commandText, CommandType commandtype)
        {
            CommandText = commandText;
            CommandType = commandtype;
        }

        /// <summary>
        /// Gets or sets the Transact-SQL statement, table name or stored procedure to execute at the data source.
        /// </summary>
        public string CommandText { get; private set; } = string.Empty;

        /// <summary>
        /// A Specific ConnectionType in configuration is required for this Procedure, throws error when not matched. Leave empty to ignore.
        /// </summary>
        public string RequiredConnectionType { get; set; }

        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
        /// </summary>
        public int CommandTimeout { get; set; } = 30;

        /// <summary>
        /// Gets or sets a value indicating how the System.Data.SqlClient.SqlCommand.CommandText property is to be interpreted.
        /// </summary>
        public CommandType CommandType { get; private set; } = CommandType.Text;

        public override string ToString() => CommandText;
    }
}