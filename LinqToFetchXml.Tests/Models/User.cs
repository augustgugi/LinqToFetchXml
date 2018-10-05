using gugi.LinqToFetchXml.Attributes;
using Microsoft.Xrm.Sdk;
using System;

namespace LinqToFetchXml.Tests.Models
{
    [FetchXmlEntityLogicalName("systemuser")]
    public class User : Entity
    {
        public static class Metadata
        {
            public static string LogicalName = "systemuser";
        }

        public User() : base(Metadata.LogicalName)
        {
        }

        public Guid systemuserid
        {
            get
            {
                return this.GetAttributeValue<Guid>(nameof(systemuserid));
            }
            set
            {
                this.Id = value;
                this.Attributes[nameof(systemuserid)] = value;
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

        public Guid? _teamid
        {
            get
            {
                return this.GetAttributeValue<EntityReference>(nameof(_teamid))?.Id;
            }
            set
            {
                this.Attributes[nameof(_teamid)] = new EntityReference(Team.Metadata.LogicalName, value.Value);
            }
        }

        public Guid? _bu
        {
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