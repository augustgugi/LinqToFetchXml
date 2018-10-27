using gugi.LinqToFetchXml.Attributes;
using Microsoft.Xrm.Sdk;
using System;

namespace LinqToFetchXml.Tests.Models
{
    [FetchXmlEntityLogicalName("systemuser")]
    public class User
    {
        public User()
        {
        }

        [FetchXmlAttributeLogicalName("systemuserid")]
        public Guid Id { get; set; }

        public string _name { get; set; }

        public EntityReference _teamid { get; set; }

        public EntityReference _bu { get; set; }
    }
}