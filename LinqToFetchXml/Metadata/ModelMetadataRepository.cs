using gugi.LinqToFetchXml.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.Metadata
{
    internal class ModelMetadataRepository
    {
        private static Lazy<ConcurrentDictionary<Type, EntityModelType>> _metadata = new Lazy<ConcurrentDictionary<Type, EntityModelType>>();

        private ConcurrentDictionary<Type, EntityModelType> Metadata
        {
            get
            {
                return _metadata.Value;
            }
        }

        public EntityModelType AddModelMetadata<T>()
        {
            string entityLogicalName = GetLogicalName<T>();
            return AddModelMetadata(entityLogicalName, typeof(T));
        }

        public EntityModelType AddModelMetadata(string entityLogicalName, Type modelType)
        {
            EntityModelType entityModelType = new EntityModelType(entityLogicalName);

            IEnumerable<PropertyInfo> properties = modelType
                                                            .GetProperties()
                                                            .Where(pi => pi.CanRead);
            foreach (var currentPropertyInfo in properties)
            {
                string attributeLogicalName = GetCRMAttributeLogicalName(currentPropertyInfo);

                entityModelType.ParameterToAttributeLogicalName.Add(currentPropertyInfo.Name, attributeLogicalName);
            }

            Metadata.TryAdd(modelType, entityModelType);

            return entityModelType;
        }

        public EntityModelType GetModelMetadata(Type modelType)
        {
            EntityModelType entityModelType = null;

            Metadata.TryGetValue(modelType, out entityModelType);

            return entityModelType;
        }

        private string GetLogicalName<T>()
        {
            Type currentType = typeof(T);
            object entityLogicalNameAttribute = currentType
                .GetCustomAttributes(typeof(FetchXmlEntityLogicalNameAttribute), true)
                .FirstOrDefault();

            if (entityLogicalNameAttribute == null)
            {
                throw new InvalidOperationException($"Attribute of type {typeof(FetchXmlEntityLogicalNameAttribute)} is not applied on type {typeof(T)}");
            }

            FetchXmlEntityLogicalNameAttribute fetchXmlEntityLogicalNameAttribute = (FetchXmlEntityLogicalNameAttribute)entityLogicalNameAttribute;
            string entityLogicalName = fetchXmlEntityLogicalNameAttribute.LogicalName;

            return entityLogicalName;
        }

        private string GetCRMAttributeLogicalName(PropertyInfo currentPropertyInfo)
        {
            string attributeLogicalName = currentPropertyInfo.Name.ToLowerInvariant();
            var attributeLogicalNameAttribute = currentPropertyInfo.GetCustomAttribute<FetchXmlAttributeLogicalNameAttribute>(true);
            if (attributeLogicalNameAttribute != null)
            {
                attributeLogicalName = attributeLogicalNameAttribute.LogicalName;
            }

            return attributeLogicalName;
        }
    }
}
