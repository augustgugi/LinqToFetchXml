using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.Interfaces
{
    interface IFetchXmlSet
    {
        string EntityLogicalName { get; }
        Type EntityModelType { get; }
    }
}
