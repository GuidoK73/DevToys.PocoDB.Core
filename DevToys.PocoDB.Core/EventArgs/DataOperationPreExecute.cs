using System;
using System.Data.Common;

namespace DevToys.PocoDB.Core
{
    /// <summary>
    /// PreExecute can be used to alter connection or command properties prior to execution.
    /// </summary>
    public sealed class DataOperationPreExecute : EventArgs
    {
        /// <summary>
        /// Connection object used for execution.
        /// </summary>
        public DbConnection Connection { get; set; }

        /// <summary>
        /// Command used for execution
        /// </summary>
        public DbCommand Command { get; set; }
    }
}