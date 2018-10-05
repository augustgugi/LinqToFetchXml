using gugi.LinqToFetchXml.Query.Executor;
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
            EntityCollection entityCollection = _organizationService.RetrieveMultiple(new FetchExpression(fetchXml));

            foreach (var ent in entityCollection.Entities)
            {
                dynamic a = Activator.CreateInstance(typeof(T));
                a.Id = ent.Id;
                a.Attributes = new AttributeCollection();
                foreach (var attr in ent.Attributes)
                {
                    a.Attributes[attr.Key] = attr.Value;
                };

                yield return a;
            }
        }
    }
}
