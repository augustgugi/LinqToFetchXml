using gugi.LinqToFetchXml;
using gugi.LinqToFetchXml.Interfaces;
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
    class SelectAttributesNodeProviders : NodeProviderBase
    {
        public Expression Expression { get; private set; }
        private IFetchXmlSet FetchXmlSet;

        public SelectAttributesNodeProviders(MethodCallExpressionParseInfo source, Expression expression) : base(source)
        {
            FetchXmlSet = (IFetchXmlSet)((ConstantExpression)source.ParsedExpression.Arguments.First()).Value;
            Expression = expression;
        }

        protected override void ApplyNodes(QueryModel queryModel, ClauseGenerationContext clauseGenerationContext)
        {
            queryModel.BodyClauses.Add(new SelectAttributesClause(Expression, FetchXmlSet));
        }

        public static IEnumerable<MethodInfo> SupportedMethods
        {
            get
            {
                string methodName = nameof(FetchXmlExtensions.SelectAttributes);
                MethodInfo method = typeof(FetchXmlExtensions).GetMethod(methodName);
                yield return method;
            }
        }
    }
}
