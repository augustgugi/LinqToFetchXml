using gugi.LinqToFetchXml.Metadata;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing;
using System;
using System.Linq.Expressions;
using System.Text;

namespace gugi.LinqToFetchXml.QueryGeneration
{
    internal class FetchXmlExpressionTreeVisitor : ThrowingExpressionVisitor
    {
        private readonly StringBuilder _actualFetchXml = new StringBuilder();

        public static string GetFetchXml(Expression expression)
        {
            var visitor = new FetchXmlExpressionTreeVisitor();
            visitor.Visit(expression);
            return visitor.GetFetchXmlExpression();
        }

        public string GetFetchXmlExpression()
        {
            return _actualFetchXml.ToString();
        }

        public override Expression Visit(Expression expression)
        {
            return base.Visit(expression);
        }

        // Called when a LINQ expression type is not handled above.
        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            string itemText = FormatUnhandledItem(unhandledItem);
            var message = string.Format("The expression '{0}' (type: {1}) is not supported by this LINQ provider.", itemText, typeof(T));
            return new NotSupportedException(message);
        }

        protected override Expression VisitBinary(BinaryExpression expression)
        {


            switch (expression.NodeType)
            {
                case ExpressionType.Equal:
                    _actualFetchXml.Append("<condition attribute='");
                    base.Visit(expression.Left);
                    _actualFetchXml.Append("' operator='");
                    _actualFetchXml.Append("eq");
                    _actualFetchXml.Append("' value='");
                    base.Visit(expression.Right);
                    _actualFetchXml.Append("' />");
                    break;
                case ExpressionType.NotEqual:
                    _actualFetchXml.Append("<condition attribute='");
                    base.Visit(expression.Left);
                    _actualFetchXml.Append("' operator='");
                    _actualFetchXml.Append("ne");
                    _actualFetchXml.Append("' value='");
                    base.Visit(expression.Right);
                    _actualFetchXml.Append("' />");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    _actualFetchXml.Append("<condition attribute='");
                    base.Visit(expression.Left);
                    _actualFetchXml.Append("' operator='");
                    _actualFetchXml.Append("ge");
                    _actualFetchXml.Append("' value='");
                    base.Visit(expression.Right);
                    _actualFetchXml.Append("' />");
                    break;
                case ExpressionType.LessThanOrEqual:
                    _actualFetchXml.Append("<condition attribute='");
                    base.Visit(expression.Left);
                    _actualFetchXml.Append("' operator='");
                    _actualFetchXml.Append("le");
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

        protected override Expression VisitBlock(BlockExpression expression)
        {
            return base.VisitBlock(expression);
        }

        protected override CatchBlock VisitCatchBlock(CatchBlock expression)
        {
            return base.VisitCatchBlock(expression);
        }

        protected override Expression VisitConditional(ConditionalExpression expression)
        {
            return base.VisitConditional(expression);
        }

        protected override Expression VisitConstant(ConstantExpression expression)
        {
            _actualFetchXml.Append(expression.Value);
            return expression;
        }

        protected override Expression VisitDebugInfo(DebugInfoExpression expression)
        {
            return base.VisitDebugInfo(expression);
        }

        protected override Expression VisitDefault(DefaultExpression expression)
        {
            return base.VisitDefault(expression);
        }

        protected override Expression VisitDynamic(DynamicExpression node)
        {
            return base.VisitDynamic(node);
        }

        protected override ElementInit VisitElementInit(ElementInit elementInit)
        {
            return base.VisitElementInit(elementInit);
        }

        protected override Expression VisitExtension(Expression expression)
        {
            return expression;
        }

        protected override Expression VisitGoto(GotoExpression expression)
        {
            return base.VisitGoto(expression);
        }

        protected override Expression VisitIndex(IndexExpression expression)
        {
            return base.VisitIndex(expression);
        }

        protected override Expression VisitInvocation(InvocationExpression expression)
        {
            return base.VisitInvocation(expression);
        }

        protected override Expression VisitLabel(LabelExpression expression)
        {
            return base.VisitLabel(expression);
        }

        protected override LabelTarget VisitLabelTarget(LabelTarget expression)
        {
            return base.VisitLabelTarget(expression);
        }

        protected override Expression VisitLambda<T>(Expression<T> expression)
        {
            return base.VisitLambda(expression);
        }

        protected override Expression VisitListInit(ListInitExpression expression)
        {
            return base.VisitListInit(expression);
        }

        protected override Expression VisitLoop(LoopExpression expression)
        {
            return base.VisitLoop(expression);
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

        protected override MemberAssignment VisitMemberAssignment(MemberAssignment memberAssigment)
        {
            return base.VisitMemberAssignment(memberAssigment);
        }

        protected override MemberBinding VisitMemberBinding(MemberBinding expression)
        {
            return base.VisitMemberBinding(expression);
        }

        protected override Expression VisitMemberInit(MemberInitExpression expression)
        {
            return base.VisitMemberInit(expression);
        }

        protected override MemberListBinding VisitMemberListBinding(MemberListBinding listBinding)
        {
            return base.VisitMemberListBinding(listBinding);
        }

        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
        {
            return base.VisitMemberMemberBinding(binding);
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            return base.VisitMethodCall(expression);
        }

        protected override Expression VisitNew(NewExpression expression)
        {
            return base.VisitNew(expression);
        }

        protected override Expression VisitNewArray(NewArrayExpression expression)
        {
            return base.VisitNewArray(expression);
        }

        protected override Expression VisitParameter(ParameterExpression expression)
        {
            return base.VisitParameter(expression);
        }

        protected override Expression VisitQuerySourceReference(QuerySourceReferenceExpression expression)
        {
            _actualFetchXml.Append("<entity name='" + ((dynamic)Activator.CreateInstance(expression.ReferencedQuerySource.ItemType)).LogicalName + "' >");
            return expression;
        }
        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression expression)
        {
            return base.VisitRuntimeVariables(expression);
        }

        protected override Expression VisitSubQuery(SubQueryExpression expression)
        {
            return base.VisitSubQuery(expression);
        }

        protected override Expression VisitSwitch(SwitchExpression expression)
        {
            return base.VisitSwitch(expression);
        }

        protected override SwitchCase VisitSwitchCase(SwitchCase expression)
        {
            return base.VisitSwitchCase(expression);
        }

        protected override Expression VisitTry(TryExpression expression)
        {
            return base.VisitTry(expression);
        }

        protected override Expression VisitTypeBinary(TypeBinaryExpression expression)
        {
            return base.VisitTypeBinary(expression);
        }

        protected override Expression VisitUnary(UnaryExpression expression)
        {
            return base.Visit(expression.Operand);
        }

        protected override TResult VisitUnhandledItem<TItem, TResult>(TItem unhandledItem, string visitMethod, Func<TItem, TResult> baseBehavior)
        {
            return base.VisitUnhandledItem(unhandledItem, visitMethod, baseBehavior);
        }

        protected override Expression VisitUnknownStandardExpression(Expression expression, string visitMethod, Func<Expression, Expression> baseBehavior)
        {
            return base.VisitUnknownStandardExpression(expression, visitMethod, baseBehavior);
        }

        private string FormatUnhandledItem<T>(T unhandledItem)
        {
            var itemAsExpression = unhandledItem as Expression;
            return itemAsExpression != null ? itemAsExpression.NodeType.ToString() : unhandledItem.ToString();
        }
    }
}