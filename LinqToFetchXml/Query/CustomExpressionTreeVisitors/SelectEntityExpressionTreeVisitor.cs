using gugi.LinqToFetchXml.Metadata;
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
    class SelectEntityExpressionTreeVisitor : ThrowingExpressionVisitor
    {
        public SelectEntityExpressionTreeVisitor(Expression expression)
        {
            base.Visit(expression);
        }
        
        public Dictionary<string, List<string>> EntityAttributes = new Dictionary<string, List<string>>();
        Stack<string> processingEntity = new Stack<string>();

        protected override Expression VisitNew(NewExpression expression)
        {
            foreach(var argument in expression.Arguments)
            {
                base.Visit(argument);
            }
            return expression;
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            base.Visit(expression.Object);
            foreach (var argument in expression.Arguments)
            {
                base.Visit(argument);
            }
            return expression;
        }

        protected override Expression VisitQuerySourceReference(QuerySourceReferenceExpression expression)
        {
            MainFromClause mainFromClause = (MainFromClause)expression.ReferencedQuerySource;
            MainFromEntityExpressionTreeVisitor mainFromEntityExpressionTreeVisitor = new MainFromEntityExpressionTreeVisitor(mainFromClause.FromExpression);

            processingEntity.Push(mainFromEntityExpressionTreeVisitor.EntityLogicalName);

            return expression;
        }

        protected override Expression VisitConstant(ConstantExpression expression)
        {
            string attrLogicalName = (string)expression.Value;

            var entityLogicalName = processingEntity.Peek();
            if (!EntityAttributes.ContainsKey(entityLogicalName))
            {
                EntityAttributes.Add(entityLogicalName, new List<string>());
            }

            EntityAttributes[entityLogicalName].Add(attrLogicalName);

            return expression;
        }

        protected override Expression VisitMember(MemberExpression expression)
        {

            ModelMetadataRepository modelMetadataRepository = new ModelMetadataRepository();

            EntityModelType entityModel = modelMetadataRepository.GetModelMetadata(expression.Member.DeclaringType);
            if (entityModel != null)
            {
                var crmAttributeLogicalName = entityModel.ParameterToAttributeLogicalName[expression.Member.Name];

                var entityLogicalName = entityModel.EntityLogicalName;
                if (!EntityAttributes.ContainsKey(entityLogicalName))
                {
                    EntityAttributes.Add(entityLogicalName, new List<string>());
                }
                EntityAttributes[entityLogicalName].Add(crmAttributeLogicalName);
            }
            else
            {
                var entityLogicalName = processingEntity.Peek();
                if (!EntityAttributes.ContainsKey(entityLogicalName))
                {
                    EntityAttributes.Add(entityLogicalName, new List<string>());
                }
                EntityAttributes[entityLogicalName].Add(expression.Member.Name);
            }
            

            return expression;
        }

        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            throw new NotImplementedException();
        }
    }
}
