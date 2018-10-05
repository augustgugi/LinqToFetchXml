using gugi.LinqToFetchXml.Attributes;
using gugi.LinqToFetchXml.Metadata;
using gugi.LinqToFetchXml.Query.Executor;
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

        public FetchXmlContext(ICustomFetchXmlQueryExecutor userFetchXmlQueryExecutor)
        {
            UserFetchXmlQueryExecutor = userFetchXmlQueryExecutor;

            _fetchXmlQueryExecutor = new FetchXmlQueryExecutor(userFetchXmlQueryExecutor);
        }

        public ICustomFetchXmlQueryExecutor UserFetchXmlQueryExecutor { get; }

        protected FetchXmlSet<Entity> CreateQuery(string entityLogicalName)
        {
            FetchXmlSet<Entity> set = new FetchXmlSet<Entity>(entityLogicalName, _fetchXmlQueryExecutor);

            return set;
        }

        protected FetchXmlSet<T> CreateQuery<T>()
        {
            string entityLogicalName = GetLogicalName<T>();

            FetchXmlSet<T> set = new FetchXmlSet<T>(entityLogicalName, _fetchXmlQueryExecutor);

            return set;
        }

        private string GetLogicalName<T>()
        {
            Type currentType = typeof(T);
            if (currentType == typeof(Entity))
            {
                throw new NotSupportedException("Please use CreateQuery(string entityLogicalName) with Microsoft.Xrm.Sdk.Entity class!");
            }
            object entityLogicalNameAttribute = currentType
                .GetCustomAttributes(typeof(FetchXmlEntityLogicalNameAttribute), true)
                .FirstOrDefault();

            if (entityLogicalNameAttribute == null)
            {
                throw new InvalidOperationException($"Attribute of type {typeof(FetchXmlEntityLogicalNameAttribute)} is not applied on type {typeof(T)}");
            }

            FetchXmlEntityLogicalNameAttribute fetchXmlEntityLogicalNameAttribute = (FetchXmlEntityLogicalNameAttribute)entityLogicalNameAttribute;
            string entityLogicalName = fetchXmlEntityLogicalNameAttribute.LogicalName;
            EntityModelType entityModelType = new EntityModelType(entityLogicalName);

            IEnumerable<PropertyInfo> properties = currentType
                                                            .GetProperties()
                                                            .Where(pi => pi.CanRead);
            foreach(var currentPropertyInfo in properties)
            {

                string attributeLogicalName = currentPropertyInfo.Name.ToLowerInvariant();
                var attributeLogicalNameAttribute = currentPropertyInfo.GetCustomAttribute<FetchXmlAttributeLogicalNameAttribute>(true);
                if (attributeLogicalNameAttribute != null)
                {
                    attributeLogicalName = attributeLogicalNameAttribute.LogicalName;
                }

                entityModelType.ParameterToAttributeLogicalName.Add(currentPropertyInfo.Name, attributeLogicalName);
            }

            TypeEntityMapping.Instance.Value.TryAdd(currentType, entityModelType);

            return entityLogicalName;
        }
    }
}
