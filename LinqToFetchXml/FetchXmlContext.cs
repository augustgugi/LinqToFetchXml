using gugi.LinqToFetchXml.Attributes;
using gugi.LinqToFetchXml.Metadata;
using gugi.LinqToFetchXml.Query.Executor;
using gugi.LinqToFetchXml.Query.Parsers;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml
{
    public abstract class FetchXmlContext
    {
        private FetchXmlQueryExecutor _fetchXmlQueryExecutor;
        private ModelMetadataRepository _modelMetadataRepository;

        public FetchXmlContext(ICustomFetchXmlQueryExecutor userFetchXmlQueryExecutor)
        {

            _fetchXmlQueryExecutor = new FetchXmlQueryExecutor(userFetchXmlQueryExecutor);
            _modelMetadataRepository = new ModelMetadataRepository();
        }

        protected FetchXmlSet<Entity> CreateQuery(string entityLogicalName)
        {
            FetchXmlSet<Entity> set = new FetchXmlSet<Entity>(entityLogicalName, FetchXmlQueryParserLoader.CreateFetchXmlQueryParser(), _fetchXmlQueryExecutor);

            return set;
        }

        protected FetchXmlSet<T> CreateQuery<T>()
        {
            EntityModelType entityModelType = _modelMetadataRepository.AddModelMetadata<T>();

            FetchXmlSet<T> set = new FetchXmlSet<T>(entityModelType.EntityLogicalName, FetchXmlQueryParserLoader.CreateFetchXmlQueryParser(), _fetchXmlQueryExecutor);

            return set;
        }
    }
}
