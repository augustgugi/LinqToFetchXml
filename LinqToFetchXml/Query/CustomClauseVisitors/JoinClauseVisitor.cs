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
        public JoinClauseVisitor(JoinClause joinClause, QueryModel queryModel, int index)
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
            EntityModelType entityModelType = null;
            TypeEntityMapping.Instance.Value.TryGetValue(from.MemberContainingType, out entityModelType);
            if (entityModelType == null)
            {
                throw new InvalidOperationException();
            }

            FromEntity = entityModelType.EntityLogicalName;
            FromAttribute = entityModelType.ParameterToAttributeLogicalName[from.MemberName];
        }

        private void HandleTo(JoinExpressionTreeVisitor to)
        {
            EntityModelType entityModelType = null;
            TypeEntityMapping.Instance.Value.TryGetValue(to.MemberContainingType, out entityModelType);
            if (entityModelType == null)
            {
                throw new InvalidOperationException();
            }

            ToEntity = entityModelType.EntityLogicalName;
            ToAttribute = entityModelType.ParameterToAttributeLogicalName[to.MemberName];
        }
    }
}
