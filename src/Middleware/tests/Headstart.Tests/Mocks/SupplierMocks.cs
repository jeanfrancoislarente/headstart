﻿using System.Collections.Generic;
using Headstart.Models.Headstart;
using OrderCloud.SDK;

namespace Headstart.Tests.Mocks
{
    public static class SupplierMocks
    {
        public static List<HSSupplier> Suppliers(params HSSupplier[] suppliers)
        {
            return new List<HSSupplier>(suppliers);
        }

        public static ListPage<HSSupplier> EmptySuppliersList()
        {
            var items = new List<HSSupplier>();
            return new ListPage<HSSupplier>
            {
                Items = items,
            };
        }

        public static ListPage<HSSupplier> SupplierList(params HSSupplier[] suppliers)
        {
            return new ListPage<HSSupplier>
            {
                Items = new List<HSSupplier>(suppliers),
            };
        }
    }
}
