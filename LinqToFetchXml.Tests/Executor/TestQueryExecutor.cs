using gugi.LinqToFetchXml.Query.Executor;
using LinqToFetchXml.Tests.Mappers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToFetchXml.Tests.Executor
{
    class TestQueryExecutor : ICustomFetchXmlQueryExecutor
    {
        private IOrganizationService _organizationService;

        public TestQueryExecutor(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }
        public IEnumerable<T> ExecuteSync<T>(string fetchXml)
        {
            var query = new FetchExpression(fetchXml);
            EntityCollection entityCollection = _organizationService.RetrieveMultiple(query);

            if (typeof(T) == typeof(Entity))
            {
                return (IEnumerable<T>)entityCollection.Entities.AsEnumerable();
            }

            var result = CustomEntityModelMapper.ToModel<T>(entityCollection.Entities.ToList());

            return result;
        }
    }
}
