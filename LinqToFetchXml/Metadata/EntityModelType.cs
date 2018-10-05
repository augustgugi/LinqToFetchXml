using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.Metadata
{
    internal class EntityModelType
    {
        public EntityModelType(string entityLogicalName)
        {
            EntityLogicalName = entityLogicalName;
            ParameterToAttributeLogicalName = new Dictionary<string, string>();

        }

        public string EntityLogicalName { get; }
        public Dictionary<string, string> ParameterToAttributeLogicalName { get; }
    }
}
