using gugi.LinqToFetchXml;
using LinqToFetchXml.Tests.Executor;
using LinqToFetchXml.Tests.Mappers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToFetchXml.Tests.Models
{
    class TestModelContext : FetchXmlContext
    {
        private IOrganizationService _organizationService;

        public TestModelContext(IOrganizationService organizationService) : base(new TestQueryExecutor(organizationService))
        {
            _organizationService = organizationService;

            Users = CreateQuery<User>();
            Teams = CreateQuery<Team>();
            BusinessUnits = CreateQuery<BusinessUnit>();
        }

        public FetchXmlSet<User> Users { get;  }
        public FetchXmlSet<Team> Teams { get;  }
        public FetchXmlSet<BusinessUnit> BusinessUnits { get; }

        
        public Guid Create(Entity record)
        {
            return _organizationService.Create(record);
        }

        public Guid Create<T>(T record)
        {
            Entity ent = CustomEntityModelMapper.ToEntity<T>(record);

            return _organizationService.Create(ent);
        }
    }
}
