using gugi.LinqToFetchXml.Query.CustomExpressionTreeVisitors;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using System;

namespace gugi.LinqToFetchXml.Query.CustomClauseVisitors
{
    internal class WhereClauseEntityVisitor
    {
        public WhereClauseEntityVisitor(WhereClause whereClause, QueryModel queryModel)
        {
            WhereExpressionVisitor whereExpressionVisitor = new WhereExpressionVisitor(whereClause.Predicate);
            Filters = whereExpressionVisitor.Filters;
        }

        public string Filters
        {
            get;
        }

    }
}
