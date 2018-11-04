using gugi.LinqToFetchXml.Interfaces;
using gugi.LinqToFetchXml.Query.Parsers;
using gugi.LinqToFetchXml.QueryGeneration;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace gugi.LinqToFetchXml
{
    public sealed class FetchXmlSet<T> : QueryableBase<T>, IFetchXmlSet
    {
        private Expression _expression;
        internal FetchXmlSet(string entityLogicalName, IQueryParser queryParser, IQueryExecutor executor) : base(queryParser, executor)
        {
            EntityLogicalName = entityLogicalName;
            EntityModelType = typeof(T);
        }

        // used by Linq
        public FetchXmlSet(IQueryProvider provider, Expression expression) : base(provider, expression)
        {
            _expression = expression;
        }

        public string EntityLogicalName { get; private set; }
        public Type EntityModelType { get; private set; }

        public override string ToString()
        {
            var queryParser = FetchXmlQueryParserLoader.CreateFetchXmlQueryParser();
            var queryModel = queryParser.GetParsedQuery(_expression);
            var queryMetadata = FetchXmlQueryModelVisitor.GetQueryMetadata(queryModel);
            var fetchXml = queryMetadata.GetFetchXmlQuery();
            return fetchXml;
        }
    }
}
