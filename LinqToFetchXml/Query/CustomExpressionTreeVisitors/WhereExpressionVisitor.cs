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
    internal class WhereExpressionVisitor : ThrowingExpressionVisitor
    {

        private readonly StringBuilder _actualFetchXml = new StringBuilder();

        private string entityLogicalName = "";

        public string Filters { get
            {
                return _actualFetchXml.ToString();
            }
        }

        public WhereExpressionVisitor(Expression expression)
        {
            base.Visit(expression);
        }

        protected override Expression VisitUnary(UnaryExpression expression)
        {
            return base.Visit(expression.Operand);
        }

        protected override Expression VisitBinary(BinaryExpression expression)
        {

            switch (expression.NodeType)
            {
                case ExpressionType.Equal:
                    _actualFetchXml.Append("<condition attribute='");
                    base.Visit(expression.Left);
                    _actualFetchXml.Append("' entityname='" + entityLogicalName + "' operator='");
                    _actualFetchXml.Append("eq");
                    _actualFetchXml.Append("' value='");
                    base.Visit(expression.Right);
                    _actualFetchXml.Append("' />");
                    break;
                case ExpressionType.NotEqual:
                    _actualFetchXml.Append("<condition attribute='");
                    base.Visit(expression.Left);
                    _actualFetchXml.Append("' entityname='" + entityLogicalName + "' operator='");
                    _actualFetchXml.Append("ne");
                    _actualFetchXml.Append("' value='");
                    base.Visit(expression.Right);
                    _actualFetchXml.Append("' />");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    _actualFetchXml.Append("<condition attribute='");
                    base.Visit(expression.Left);
                    _actualFetchXml.Append("' entityname='" + entityLogicalName + "' operator='");
                    _actualFetchXml.Append("ge");
                    _actualFetchXml.Append("' value='");
                    base.Visit(expression.Right);
                    _actualFetchXml.Append("' />");
                    break;
                case ExpressionType.LessThanOrEqual:
                    _actualFetchXml.Append("<condition attribute='");
                    base.Visit(expression.Left);
                    _actualFetchXml.Append("' entityname='" + entityLogicalName + "' operator='");
                    _actualFetchXml.Append("le");
                    _actualFetchXml.Append("' value='");
                    base.Visit(expression.Right);
                    _actualFetchXml.Append("' />");
                    break;
                case ExpressionType.LessThan:
                    _actualFetchXml.Append("<condition attribute='");
                    base.Visit(expression.Left);
                    _actualFetchXml.Append("' entityname='" + entityLogicalName + "' operator='");
                    _actualFetchXml.Append("lt");
                    _actualFetchXml.Append("' value='");
                    base.Visit(expression.Right);
                    _actualFetchXml.Append("' />");
                    break;
                case ExpressionType.GreaterThan:
                    _actualFetchXml.Append("<condition attribute='");
                    base.Visit(expression.Left);
                    _actualFetchXml.Append("' entityname='" + entityLogicalName + "' operator='");
                    _actualFetchXml.Append("gt");
                    _actualFetchXml.Append("' value='");
                    base.Visit(expression.Right);
                    _actualFetchXml.Append("' />");
                    break;
                case ExpressionType.AndAlso:
                    _actualFetchXml.Append("<filter type='and'>");
                    base.Visit(expression.Left);
                    base.Visit(expression.Right);
                    _actualFetchXml.Append("</filter>");
                    break;
                case ExpressionType.OrElse:
                    _actualFetchXml.Append("<filter type='or'>");
                    base.Visit(expression.Left);
                    base.Visit(expression.Right);
                    _actualFetchXml.Append("</filter>");
                    break;

                default:
                    base.VisitBinary(expression);
                    break;
            }



            return expression;
        }

        protected override Expression VisitMember(MemberExpression expression)
        {
            ModelMetadataRepository modelMetadataRepository = new ModelMetadataRepository();

            EntityModelType entityModel = modelMetadataRepository.GetModelMetadata(expression.Member.DeclaringType);
            if (entityModel != null)
            {
                var crmAttributeLogicalName = entityModel.ParameterToAttributeLogicalName[expression.Member.Name];


                _actualFetchXml.Append(crmAttributeLogicalName);
            }
            else
            {
                _actualFetchXml.Append(expression.Member.Name);
            }

            return expression;
        }

        protected override Expression VisitConstant(ConstantExpression expression)
        {
            ModelMetadataRepository modelMetadataRepository = new ModelMetadataRepository();

            EntityModelType entityModel = modelMetadataRepository.GetModelMetadata(expression.Type);

            if (entityModel != null)
            {
                var crmAttributeLogicalName = entityModel.ParameterToAttributeLogicalName[(string)expression.Value];


                _actualFetchXml.Append(crmAttributeLogicalName);
            }
            else
            {
                _actualFetchXml.Append(expression.Value);
            }
            
            return expression;
        }

        protected override Expression VisitQuerySourceReference(QuerySourceReferenceExpression expression)
        {
            if (expression.ReferencedQuerySource is JoinClause)
            {
                var clause = (JoinClause)expression.ReferencedQuerySource;
                JoinExpressionTreeVisitor joinExpressionTreeVisitor = new JoinExpressionTreeVisitor(clause.InnerSequence);
                entityLogicalName = joinExpressionTreeVisitor.EntityLogicalName;
            }
            else if (expression.ReferencedQuerySource is MainFromClause)
            {
                var clasue = (MainFromClause)expression.ReferencedQuerySource;
                MainFromEntityClauseVisitor mainFromEntityClauseVisitor = new MainFromEntityClauseVisitor(clasue);
                entityLogicalName = mainFromEntityClauseVisitor.EntityLogicalName;
            }

            return expression;
        }

        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            throw new NotImplementedException();
        }
    }
}
