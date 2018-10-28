using gugi.LinqToFetchXml.Metadata;
using Microsoft.Xrm.Sdk;
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

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            if (expression.Method.Name != "GetAttributeValue")
            {
                throw new NotSupportedException($"{expression.Method.Name} not supported. Only GetAttributeValue method of {typeof(Entity)} class is allowed!");
            }
            return base.Visit(expression.Arguments.First());
        }

        protected override Expression VisitMember(MemberExpression expression)
        {
            MemberName = expression.Member.Name;
            MemberContainingType = expression.Member.DeclaringType;

            return expression;
        }

        protected override Expression VisitConstant(ConstantExpression expression)
        {
            EntityModelType entityModel = null;
            TypeEntityMapping.Instance.Value.TryGetValue(expression.Type, out entityModel);

            if (entityModel != null)
            {
                var crmAttributeLogicalName = entityModel.ParameterToAttributeLogicalName[(string)expression.Value];


                MemberName = crmAttributeLogicalName;
                MemberContainingType = expression.Type;
            }
            else
            {
                MemberName = (string)expression.Value;
                MemberContainingType = expression.Type;
            }

            return expression;
        }

        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            throw new NotImplementedException();
        }
    }
}
