﻿using System;

namespace OrderCloud.Integrations.Library.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SearchableAttribute : Attribute
    {
        public SearchableAttribute(int priority)
        {
            Priority = priority;
        }

        public int Priority { get; set; }
    }
}
