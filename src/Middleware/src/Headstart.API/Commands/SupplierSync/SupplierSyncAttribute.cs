﻿using System;

namespace Headstart.API.Commands
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SupplierSyncAttribute : Attribute
    {
        public SupplierSyncAttribute(string supplierID)
        {
            this.SupplierID = supplierID;
        }

        public string SupplierID { get; set; }
    }
}
