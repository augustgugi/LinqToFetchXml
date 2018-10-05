using Remotion.Linq.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.Query.CustomExpressionTreeVisitors
{
    internal class JoinExpressionTreeVisitor : ThrowingExpressionVisitor
    {
        public Type MemberContainingType { get; private set; }

        public string MemberName { get; private set; }

        public JoinExpressionTreeVisitor(Expression expression)
        {
            base.Visit(expression);
        }

        protected override Expression VisitUnary(UnaryExpression expression)
        {
            return base.Visit(expression.Operand);
        }

        protected override Expression VisitMember(MemberExpression expression)
        {
            MemberName = expression.Member.Name;
            MemberContainingType = expression.Member.DeclaringType;

            return expression;
        }

        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            throw new NotImplementedException();
        }
    }
}
