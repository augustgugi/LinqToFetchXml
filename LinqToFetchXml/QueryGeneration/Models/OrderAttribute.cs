using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.QueryGeneration.Models
{
    enum OrderDirection
    {
        Asc,
        Desc
    }

    class OrderAttribute
    {
        public string EntityLogicalName { get; set; }
        public string AttributeLogicalName { get; set; }
        public OrderDirection OrderDirection { get; set; }
    }
}
