using Remotion.Linq.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.Query.CustomExpressionTreeVisitors
{
    internal class SelectAttributesExpressionTreeVisitor : ThrowingExpressionVisitor
    {
        public List<string> Attributes = new List<string>();
        public bool AllAttributes = true;

        public SelectAttributesExpressionTreeVisitor(Expression expression)
        {
            base.Visit(expression);
        }

        protected override Expression VisitLambda<T>(Expression<T> expression)
        {
            return base.Visit(expression.Body);
        }

        protected override Expression VisitNew(NewExpression expression)
        {
            return base.Visit(expression.Arguments.First());
        }

        protected override Expression VisitUnary(UnaryExpression expression)
        {
            return base.Visit(expression.Operand);
        }

        protected override Expression VisitNewArray(NewArrayExpression expression)
        {
            foreach(Expression currentArgument in expression.Expressions)
            {
                base.Visit(currentArgument);
            }

            return expression;
        }

        protected override Expression VisitConstant(ConstantExpression expression)
        {
            if (expression.Value is FetchXmlAttributes)
            {
                var fetchXmlAttributes = (FetchXmlAttributes)expression.Value;
                AllAttributes = fetchXmlAttributes.AllAttributes;
                Attributes.AddRange(fetchXmlAttributes.Attributes?.Cast<string>());
            }
            else
            {
                AllAttributes = false;
                Attributes.Add((string)expression.Value);
            }

            return expression;
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            foreach(var methodArg in expression.Arguments)
            {
                base.Visit(methodArg);
            }

            return expression;
        }
               
        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            throw new NotImplementedException();
        }
    }
}
