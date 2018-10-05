using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using gugi.LinqToFetchXml.QueryGeneration.NodeProviders;
using Microsoft.Xrm.Sdk;
using Remotion.Linq;
using Remotion.Linq.Parsing.ExpressionVisitors.Transformation;
using Remotion.Linq.Parsing.Structure;
using Remotion.Linq.Parsing.Structure.NodeTypeProviders;

namespace gugi.LinqToFetchXml
{
    public sealed class FetchXmlSet<T> : QueryableBase<T>
    {
        internal FetchXmlSet(string entityLogicalName, IQueryExecutor executor) : base(CreateQueryParser(), executor)
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

        private static IQueryParser CreateQueryParser()
        {
            var customNodeTypeRegistry = new MethodInfoBasedNodeTypeRegistry();
            // Register custom node parsers here:
            customNodeTypeRegistry.Register(FilterTypeNodeProvider.SupportedMethods, typeof(FilterTypeNodeProvider));
            // Alternatively, use the CreateFromTypes factory method.
            // Use MethodNameBasedNodeTypeRegistry to register parsers by query operator name instead of MethodInfo.

            var nodeTypeProvider = ExpressionTreeParser.CreateDefaultNodeTypeProvider();
            nodeTypeProvider.InnerProviders.Add(customNodeTypeRegistry);

            var transformerRegistry = ExpressionTransformerRegistry.CreateDefault();
            // Register custom expression transformers executed _after_ partial evaluation here (this should be the default):
            // transformerRegistry.Register (new MyExpressionTransformer());

            var processor = ExpressionTreeParser.CreateDefaultProcessor(transformerRegistry);

            // To register custom expression transformers executed _before_ partial evaluation, use this code:
            // var earlyTransformerRegistry = new ExpressionTransformerRegistry();
            // earlyTransformerRegistry.Register (new MyEarlyExpressionTransformer());
            // processor.InnerProcessors.Insert (0, new TransformingExpressionTreeProcessor (tranformationProvider));

            // Add custom processors here (use Insert (0, ...) to add at the beginning):
            // processor.InnerProcessors.Add (new MyExpressionTreeProcessor());

            var expressionTreeParser = new ExpressionTreeParser(nodeTypeProvider, processor);
            var queryParser = new QueryParser(expressionTreeParser);

            return queryParser;
        }
    }
}
