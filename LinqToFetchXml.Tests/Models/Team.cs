using gugi.LinqToFetchXml.Attributes;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToFetchXml.Tests.Models
{
    [FetchXmlEntityLogicalName("team")]
    public class Team : Entity
    {
        public static class Metadata
        {
            public static string LogicalName = "team";
        }

        public Team() : base(Metadata.LogicalName)
        {
        }

        public Guid teamid
        {
            get
            {
                return this.GetAttributeValue<Guid>(nameof(teamid));
            }
            set
            {
                this.Id = value;
                this.Attributes[nameof(teamid)] = value;
            }
        }

        public string _name
        {
            get
            {
                return this.GetAttributeValue<string>(nameof(_name));
            }
            set
            {
                this.Attributes[nameof(_name)] = value;
            }
        }

        public Guid? _bu {
            get
            {
                return this.GetAttributeValue<EntityReference>(nameof(_bu))?.Id;
            }
            set
            {
                this.Attributes[nameof(_bu)] = new EntityReference(BusinessUnit.Metadata.LogicalName, value.Value);
            }
        }
    }
}
