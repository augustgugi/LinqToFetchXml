using Remotion.Linq;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.Query.Clauses
{
    internal class FilterTypeClause : IBodyClause
    {
        public Expression Expression { get; private set; }

        public FilterTypeClause(Expression expression)
        {
            Expression = expression;
        }

        public void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
        {
            
        }

        public IBodyClause Clone(CloneContext cloneContext)
        {
            return new FilterTypeClause(Expression);
        }

        public void TransformExpressions(Func<Expression, Expression> transformation)
        {
            Expression = transformation(Expression);
        }
    }
}
