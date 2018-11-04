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
    public class Team
    {

        public Team()
        {
        }

        [FetchXmlAttributeLogicalName("teamid")]
        public Guid Id { get; set; }

        public EntityReference teamid  { get; set; }

        public string _name { get; set; }

        public EntityReference _bu { get; set; }
    }
}
