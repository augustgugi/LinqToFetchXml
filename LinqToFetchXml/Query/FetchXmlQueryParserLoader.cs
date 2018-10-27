using gugi.LinqToFetchXml.Query.NodeProviders;
using Remotion.Linq.Parsing.ExpressionVisitors.Transformation;
using Remotion.Linq.Parsing.Structure;
using Remotion.Linq.Parsing.Structure.NodeTypeProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.Query.Parsers
{
    internal class FetchXmlQueryParserLoader
    {
        public static IQueryParser CreateFetchXmlQueryParser()
        {
            var customNodeTypeRegistry = new MethodInfoBasedNodeTypeRegistry();
            RegisterCustomNodes(customNodeTypeRegistry);

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

        private static void RegisterCustomNodes(MethodInfoBasedNodeTypeRegistry customNodeTypeRegistry)
        {
            // Register custom node parsers here:
            customNodeTypeRegistry.Register(FilterTypeNodeProvider.SupportedMethods, typeof(FilterTypeNodeProvider));
            customNodeTypeRegistry.Register(SelectAttributesNodeProviders.SupportedMethods, typeof(SelectAttributesNodeProviders));
            // Alternatively, use the CreateFromTypes factory method.
            // Use MethodNameBasedNodeTypeRegistry to register parsers by query operator name instead of MethodInfo.
        }
    }
}
