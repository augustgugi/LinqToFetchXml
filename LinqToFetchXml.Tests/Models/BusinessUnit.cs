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
    public class BusinessUnit
    {

        public BusinessUnit()
        {

        }

        [FetchXmlAttributeLogicalName("buid")]
        public Guid Id { get; set; }

        public string _name { get; set; }
    }
}
