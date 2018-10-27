using gugi.LinqToFetchXml.Interfaces;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace gugi.LinqToFetchXml
{
    public sealed class FetchXmlSet<T> : QueryableBase<T>, IFetchXmlSet
    {
        internal FetchXmlSet(string entityLogicalName, IQueryParser queryParser, IQueryExecutor executor) : base(queryParser, executor)
        {
            EntityLogicalName = entityLogicalName;
            EntityModelType = typeof(T);
        }

        //public FetchXmlSet(IQueryProvider provider) : base(provider)
        //{
        //}

        // used by Linq
        public FetchXmlSet(IQueryProvider provider, Expression expression) : base(provider, expression)
        {
        }

        public string EntityLogicalName { get; private set; }
        public Type EntityModelType { get; private set; }
    }
}
