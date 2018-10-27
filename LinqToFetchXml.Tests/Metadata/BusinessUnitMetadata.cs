using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToFetchXml.Tests.Metadata
{
    class BusinessUnitMetadata
    {

        public static EntityMetadata GetMetadata()
        {
            EntityMetadata entityMetadata = new EntityMetadata()
            {
                LogicalName = "bu"
            };
            //entityMetadata.Attributes.ToList().AddRange(GetAttributesMetadata());


            return entityMetadata;
        }

        private static List<AttributeMetadata> GetAttributesMetadata()
        {
            List<AttributeMetadata> attributeMetadatas = new List<AttributeMetadata>()
            {
                new IntegerAttributeMetadata("users_filter")
                {
                    LogicalName = "users_filter"
                }
            };

            return attributeMetadatas;
        }
    }
}
