using gugi.LinqToFetchXml.Interfaces;
using Microsoft.Xrm.Sdk;
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
    internal class MainFromEntityExpressionTreeVisitor : ThrowingExpressionVisitor
    {
        
        public MainFromEntityExpressionTreeVisitor(Expression expression)
        {
            base.Visit(expression);
        }

        public string EntityLogicalName { get; private set; }
        public Type EntityType { get; private set; }

        protected override Expression VisitConstant(ConstantExpression expression)
        {
            var value = (IFetchXmlSet)expression.Value;

            EntityLogicalName = value.EntityLogicalName;
            EntityType = value.EntityModelType;

            return expression;
        }

        protected override Expression VisitSubQuery(SubQueryExpression expression)
        {
            throw new NotSupportedException();
        }

        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            throw new NotImplementedException();
        }
    }
}
