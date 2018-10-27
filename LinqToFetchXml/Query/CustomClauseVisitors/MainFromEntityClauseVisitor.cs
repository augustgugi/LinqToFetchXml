using gugi.LinqToFetchXml.Query.CustomExpressionTreeVisitors;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.Query.CustomClauseVisitors.Entity
{
    class MainFromEntityClauseVisitor
    {
        
        public MainFromEntityClauseVisitor(MainFromClause fromClause, QueryModel queryModel)
        {
            MainFromEntityExpressionTreeVisitor mainFromEntityExpressionTreeVisitor = new MainFromEntityExpressionTreeVisitor(fromClause.FromExpression);
            EntityLogicalName = mainFromEntityExpressionTreeVisitor.EntityLogicalName;
            EntityType = mainFromEntityExpressionTreeVisitor.EntityType;
        }

        public string EntityLogicalName { get; }
        public Type EntityType { get; }
    }
}
