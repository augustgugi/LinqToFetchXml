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
    internal class SelectEntityClauseVisitor
    {
        public SelectEntityClauseVisitor(SelectClause selectClause, QueryModel queryModel)
        {
            SelectEntityExpressionTreeVisitor selectEntityExpressionTreeVisitor = new SelectEntityExpressionTreeVisitor(selectClause.Selector);
            EntityAttributes = selectEntityExpressionTreeVisitor.EntityAttributes;
        }

        public Dictionary<string, List<string>> EntityAttributes { get; }
    }
}
