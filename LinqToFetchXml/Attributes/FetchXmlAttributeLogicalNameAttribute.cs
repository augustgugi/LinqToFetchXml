﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class FetchXmlAttributeLogicalNameAttribute : Attribute
    {
        public FetchXmlAttributeLogicalNameAttribute(string logicalName)
        {
            LogicalName = logicalName;
        }
        public string LogicalName { get; }
    }
}
