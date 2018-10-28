using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml
{
    public sealed class FetchXmlAttributes
    {
        public FetchXmlAttributes(bool allAttributes)
        {
            AllAttributes = allAttributes;
        }

        public FetchXmlAttributes(params object[] attributues)
        {
            AllAttributes = false;
            if (attributues != null)
            {
                Attributes = attributues?.ToList();
            }
        }

        public bool AllAttributes { get; }
        public List<object> Attributes { get; }
    }
}
