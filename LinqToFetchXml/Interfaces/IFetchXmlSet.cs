using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.Interfaces
{
    internal interface IFetchXmlSet
    {
        string EntityLogicalName { get; }
        Type EntityModelType { get; }
    }
}
