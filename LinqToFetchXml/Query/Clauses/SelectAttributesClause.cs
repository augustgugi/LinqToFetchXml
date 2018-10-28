using gugi.LinqToFetchXml.Interfaces;
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
    class SelectAttributesClause : IBodyClause
    {
        public Expression Expression { get; private set; }
        public IFetchXmlSet FetchXmlSet { get; private set; }

        public SelectAttributesClause(Expression expression, IFetchXmlSet fetchXmlSet)
        {
            Expression = expression;
            FetchXmlSet = fetchXmlSet;
        }

        public void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
        {

        }

        public IBodyClause Clone(CloneContext cloneContext)
        {
            return new SelectAttributesClause(Expression, FetchXmlSet);
        }

        public void TransformExpressions(Func<Expression, Expression> transformation)
        {
            Expression = transformation(Expression);
        }
    }
}
