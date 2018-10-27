using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.QueryGeneration.Models
{
    internal class LinkEntity
    {
        public LinkEntity(string entityFrom, string attributeFrom, string entityTo, string attributeTo)
        {
            EntityFrom = entityFrom;
            AttributeFrom = attributeFrom;
            EntityTo = entityTo;
            AttributeTo = attributeTo;
        }

        public string EntityFrom { get; }
        public string AttributeFrom { get; }
        public string EntityTo { get; }
        public string AttributeTo { get; }
        public string Alias { get
            {
                return $"{EntityFrom}_to_{EntityTo}";
            }
        }
    }
}
