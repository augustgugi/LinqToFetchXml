using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.QueryGeneration.Models
{
    class SelectAttributes
    {
        public SelectAttributes()
        {
            AttributesLogicalNames = new List<string>();
        }

        public string EntityLogicalName { get; set; }
        public bool AllAttributes { get; set; }
        public List<string> AttributesLogicalNames { get; set; }
    }
}
