using gugi.LinqToFetchXml.Attributes;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToFetchXml.Tests.Models
{
    [FetchXmlEntityLogicalName("businessunit")]
    public class BusinessUnit : Entity
    {
        public static class Metadata
        {
            public static string LogicalName = "businessunit";
        }

        public BusinessUnit() : base(Metadata.LogicalName)
        {

        }

        public Guid buid {
            get
            {
                return this.GetAttributeValue<Guid>(nameof(buid));
            }
            set
            {
                this.Id = value;
                this.Attributes[nameof(buid)] = value;
            }
        }

        public string _name {
            get
            {
                return this.GetAttributeValue<string>(nameof(_name));
            }
            set {
                this.Attributes[nameof(_name)] = value;
            }
        }
    }
}
