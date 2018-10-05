using Remotion.Linq;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.QueryGeneration.NodeProviders
{
    internal abstract class NodeProviderBase : MethodCallExpressionNodeBase
    {
        public NodeProviderBase(MethodCallExpressionParseInfo source) : base(source)
        {

        }

        public override Expression Resolve(ParameterExpression inputParameter, Expression expressionToBeResolved, ClauseGenerationContext clauseGenerationContext)
        {
            return Source.Resolve(inputParameter, expressionToBeResolved, clauseGenerationContext);
        }

        protected override void ApplyNodeSpecificSemantics(QueryModel queryModel, ClauseGenerationContext clauseGenerationContext)
        {
            ApplyNodes(queryModel, clauseGenerationContext);
        }

        protected abstract void ApplyNodes(QueryModel queryModel, ClauseGenerationContext clauseGenerationContext);
    }
}
