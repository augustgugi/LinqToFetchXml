using gugi.LinqToFetchXml.Metadata;
using gugi.LinqToFetchXml.Query.CustomExpressionTreeVisitors;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.Query.CustomClauseVisitors
{
    internal class JoinClauseVisitor
    {
        public JoinClauseVisitor(JoinClause joinClause, int index)
        {
            Index = index;

            JoinExpressionTreeVisitor from = new JoinExpressionTreeVisitor(joinClause.InnerKeySelector);
            HandleFrom(from);
            JoinExpressionTreeVisitor to = new JoinExpressionTreeVisitor(joinClause.OuterKeySelector);
            HandleTo(to);

        }

        public int Index { get; set; }
        public string FromEntity { get; set; }
        public string FromAttribute { get; set; }
        public string ToEntity { get; set; }
        public string ToAttribute { get; set; }

        private void HandleFrom(JoinExpressionTreeVisitor from)
        {
            ModelMetadataRepository modelMetadataRepository = new ModelMetadataRepository();
            EntityModelType entityModelType = null;
            if (from.MemberContainingType != null)
            {
                entityModelType = modelMetadataRepository.GetModelMetadata(from.MemberContainingType);

            }
            
            if (entityModelType == null)
            {
                FromEntity = from.EntityLogicalName;
                FromAttribute = from.MemberName;
            }
            else
            {
                FromEntity = entityModelType.EntityLogicalName;
                FromAttribute = entityModelType.ParameterToAttributeLogicalName[from.MemberName];
            }
        }

        private void HandleTo(JoinExpressionTreeVisitor to)
        {
            ModelMetadataRepository modelMetadataRepository = new ModelMetadataRepository();
            EntityModelType entityModelType = null;
            if (to.MemberContainingType != null)
            {
                entityModelType = modelMetadataRepository.GetModelMetadata(to.MemberContainingType);

            }

            if (entityModelType == null)
            {
                ToEntity = to.EntityLogicalName;
                ToAttribute = to.MemberName;
            }
            else
            {
                ToEntity = entityModelType.EntityLogicalName;
                ToAttribute = entityModelType.ParameterToAttributeLogicalName[to.MemberName];
            }
        }
    }
}
