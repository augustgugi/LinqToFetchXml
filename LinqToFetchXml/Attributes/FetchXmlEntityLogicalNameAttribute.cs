using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class FetchXmlEntityLogicalNameAttribute : Attribute
    {
        public FetchXmlEntityLogicalNameAttribute(string logicalName)
        {
            LogicalName = logicalName;
        }
        public string LogicalName { get; }
    }
}
