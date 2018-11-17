using gugi.LinqToFetchXml.Interfaces;
using gugi.LinqToFetchXml.Metadata;
using gugi.LinqToFetchXml.Query.CustomClauseVisitors.Entity;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
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
        public string EntityLogicalName { get; private set; }

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
            MemberContainingType = MemberContainingType ?? expression.Member.DeclaringType;

            return expression;
        }

        protected override Expression VisitConstant(ConstantExpression expression)
        {
            if (expression.Value is IFetchXmlSet)
            {
                var set = (IFetchXmlSet)expression.Value;
                MemberContainingType = set.EntityModelType;
                EntityLogicalName = set.EntityLogicalName;
            }
            else
            {
                MemberName = (string)expression.Value;
            }
            

            return expression;
        }

        protected override Expression VisitQuerySourceReference(QuerySourceReferenceExpression expression)
        {
            var tt = expression.ReferencedQuerySource.GetType();
            if (expression.ReferencedQuerySource is JoinClause)
            {
                var clasue = (JoinClause)expression.ReferencedQuerySource;
                JoinExpressionTreeVisitor joinExpressionTreeVisitor = new JoinExpressionTreeVisitor(clasue.InnerSequence);
                MemberContainingType = joinExpressionTreeVisitor.MemberContainingType;
                EntityLogicalName = joinExpressionTreeVisitor.EntityLogicalName;
            }
            else if (expression.ReferencedQuerySource is MainFromClause)
            {
                var clasue = (MainFromClause)expression.ReferencedQuerySource;
                MainFromEntityClauseVisitor mainFromEntityClauseVisitor = new MainFromEntityClauseVisitor(clasue);
                MemberContainingType = mainFromEntityClauseVisitor.EntityType;
                EntityLogicalName = mainFromEntityClauseVisitor.EntityLogicalName;
            }

            return expression;
        }

        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            throw new NotImplementedException($"Method: {visitMethod}");
        }
    }
}
