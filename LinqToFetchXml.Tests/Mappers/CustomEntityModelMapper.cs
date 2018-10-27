using gugi.LinqToFetchXml.Attributes;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToFetchXml.Tests.Mappers
{
    class CustomEntityModelMapper
    {
        public static IEnumerable<T> ToModel<T>(List<Entity> entities)
        {
            var properties = typeof(T).GetProperties();

            foreach (var entity in entities)
            {
                T instance = (T)Activator.CreateInstance(typeof(T));

                foreach(var property in properties)
                {
                    if (entity.Attributes.ContainsKey(property.Name))
                    {
                        property.SetValue(instance, entity.Attributes[property.Name]);
                    }

                    if (property.Name == "Id")
                    {
                        property.SetValue(instance, entity.Id);
                    }
                }

                yield return instance;
            }
        }

        public static Entity ToEntity<T>(T record)
        {
            FetchXmlEntityLogicalNameAttribute logicalNameAttribute = (FetchXmlEntityLogicalNameAttribute)typeof(T).GetCustomAttributes(typeof(FetchXmlEntityLogicalNameAttribute), true).FirstOrDefault();

            dynamic recordD = record;

            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id");

            Entity instance = new Entity(logicalNameAttribute.LogicalName, recordD.Id);

            foreach (var property in properties)
            {
                instance.Attributes[property.Name] = property.GetValue(record);
            }

            return instance;
        }
    }
}
