using gugi.LinqToFetchXml.Extensions;
using gugi.LinqToFetchXml.Query.Clauses;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.Query.NodeProviders
{
    internal class FilterTypeNodeProvider : NodeProviderBase
    {
        public Expression Expression { get; private set; }

        public FilterTypeNodeProvider(MethodCallExpressionParseInfo source, Expression expression) : base(source)
        {
            Expression = expression;
        }

        protected override void ApplyNodes(QueryModel queryModel, ClauseGenerationContext clauseGenerationContext)
        {
            queryModel.BodyClauses.Add(new FilterTypeClause(Expression));
        }

        public static IEnumerable<MethodInfo> SupportedMethods
        {
            get
            {
                string methodName = nameof(FetchXmlExtensions.SetFilterType);
                MethodInfo method = typeof(FetchXmlExtensions).GetMethod(methodName);
                yield return method;
            }
        }
    }
}
