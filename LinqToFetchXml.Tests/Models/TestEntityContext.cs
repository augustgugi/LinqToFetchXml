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
    class TestEntityContext : FetchXmlContext
    {
        private IOrganizationService _organizationService;

        public TestEntityContext(IOrganizationService organizationService) : base(new TestQueryExecutor(organizationService))
        {
            _organizationService = organizationService;

            BusinessUnits = CreateQuery("bu");
        }

        public FetchXmlSet<Entity> BusinessUnits { get; }


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
