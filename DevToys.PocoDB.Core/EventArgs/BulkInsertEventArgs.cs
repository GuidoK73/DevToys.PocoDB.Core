using System;

namespace DevToys.PocoDB.Core
{
    public sealed class BulkInsertEventArgs : EventArgs
    {
        public int RowsProcessed { get;  set; }
    }
}