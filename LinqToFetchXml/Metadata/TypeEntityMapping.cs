using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.Metadata
{
    internal static class TypeEntityMapping
    {
        public static Lazy<ConcurrentDictionary<Type, EntityModelType>> Instance = new Lazy<ConcurrentDictionary<Type, EntityModelType>>();
    }
}
