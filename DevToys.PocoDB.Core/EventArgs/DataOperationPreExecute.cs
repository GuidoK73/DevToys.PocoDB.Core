﻿using System;
using System.Data.Common;

namespace DevToys.PocoDB.Core
{
    public sealed class DataOperationPreExecute : EventArgs
    {
        public DbConnection Connection { get; set; }

        public DbCommand Command { get; set; }
    }
}