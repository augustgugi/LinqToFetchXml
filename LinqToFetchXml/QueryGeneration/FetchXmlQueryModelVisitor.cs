using gugi.LinqToFetchXml.Metadata;
using gugi.LinqToFetchXml.Query.CustomClauseVisitors;
using gugi.LinqToFetchXml.Query.Clauses;
using gugi.LinqToFetchXml.Query.NodeProviders;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using gugi.LinqToFetchXml.Query.CustomClauseVisitors.Entity;

namespace gugi.LinqToFetchXml.QueryGeneration
{
    internal class FetchXmlQueryModelVisitor : QueryModelVisitorBase
    {
        private readonly QueryMetadata _queryMetadata = new QueryMetadata();

        internal static QueryMetadata GetQueryMetadata(QueryModel queryModel)
        {
            var visitor = new FetchXmlQueryModelVisitor();
            visitor.VisitQueryModel(queryModel);
            return visitor.GetQueryMetadata();
        }

        internal QueryMetadata GetQueryMetadata()
        {
            return _queryMetadata;
        }

        public override void VisitQueryModel(QueryModel queryModel)
        {
            VisitMainFromClause(queryModel.MainFromClause, queryModel);
            VisitBodyClauses(queryModel.BodyClauses, queryModel);
            VisitSelectClause(queryModel.SelectClause, queryModel);
            VisitResultOperators(queryModel.ResultOperators, queryModel);
        }

        public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
        {

            MainFromEntityClauseVisitor mainFromEntityClauseVisitor = new MainFromEntityClauseVisitor(fromClause, queryModel);
            _queryMetadata.EntityName = mainFromEntityClauseVisitor.EntityLogicalName;
            _queryMetadata.EntityType = mainFromEntityClauseVisitor.EntityType;
        }

        protected override void VisitBodyClauses(ObservableCollection<IBodyClause> bodyClauses, QueryModel queryModel)
        {
            foreach (IBodyClause bodyClause in bodyClauses)
            {
                if (bodyClause is FilterTypeClause)
                {
                    var filterTypeClause = bodyClause as FilterTypeClause;
                    _queryMetadata.FilterType = GetFetchXmlExpression(filterTypeClause.Expression);
                }
                else if(bodyClause is SelectAttributesClause)
                {
                    var actualClause = bodyClause as SelectAttributesClause;
                    SelectAttributesClauseVisitor selectAttributesClauseVisitor = new SelectAttributesClauseVisitor(actualClause, queryModel);
                    _queryMetadata.AddSelectAttributes(selectAttributesClauseVisitor.SelectAttributes);
                }
            }
            base.VisitBodyClauses(bodyClauses, queryModel);
        }

        public override void VisitSelectClause(SelectClause selectClause, QueryModel queryModel)
        {
            //SelectEntityClauseVisitor selectEntityClauseVisitor = new SelectEntityClauseVisitor(selectClause, queryModel);
            //foreach (var entity in selectEntityClauseVisitor.EntityAttributes.Keys)
            //{
            //    _queryMetadata.AddSelectAttributes(entity, selectEntityClauseVisitor.EntityAttributes[entity].ToArray());
            //}

            var selectT = selectClause.GetOutputDataInfo();
            _queryMetadata.ReturningType = selectT.ResultItemType;
            base.VisitSelectClause(selectClause, queryModel);
        }

        protected override void VisitResultOperators(ObservableCollection<ResultOperatorBase> resultOperators, QueryModel queryModel)
        {
            base.VisitResultOperators(resultOperators, queryModel);
        }

        public override void VisitAdditionalFromClause(AdditionalFromClause fromClause, QueryModel queryModel, int index)
        {
            _queryMetadata.AddFromPart(fromClause);

            base.VisitAdditionalFromClause(fromClause, queryModel, index);
        }

        public override void VisitGroupJoinClause(GroupJoinClause groupJoinClause, QueryModel queryModel, int index)
        {
            throw new NotSupportedException("Adding a join ... into ... implementation to the query provider is left to the reader for extra points.");
        }

        public override void VisitJoinClause(JoinClause joinClause, QueryModel queryModel, int index)
        {
            // HQL joins work differently, need to simulate using a cross join with a where condition

            JoinClauseVisitor joinClauseVisitor = new JoinClauseVisitor(joinClause, queryModel, index);

            _queryMetadata.AddJoin(joinClauseVisitor.FromEntity, joinClauseVisitor.FromAttribute, joinClauseVisitor.ToEntity, joinClauseVisitor.ToAttribute);

            base.VisitJoinClause(joinClause, queryModel, index);
        }

        public override void VisitJoinClause(JoinClause joinClause, QueryModel queryModel, GroupJoinClause groupJoinClause)
        {
            base.VisitJoinClause(joinClause, queryModel, groupJoinClause);
        }

        public override void VisitOrderByClause(OrderByClause orderByClause, QueryModel queryModel, int index)
        {
            base.VisitOrderByClause(orderByClause, queryModel, index);
        }

        public override void VisitOrdering(Ordering ordering, QueryModel queryModel, OrderByClause orderByClause, int index)
        {
            _queryMetadata.AddOrderByPart(GetFetchXmlExpression(ordering.Expression), ordering.OrderingDirection == OrderingDirection.Desc, index);

            base.VisitOrdering(ordering, queryModel, orderByClause, index);
        }

        public override void VisitResultOperator(ResultOperatorBase resultOperator, QueryModel queryModel, int index)
        {
            if (resultOperator is TakeResultOperator)
            {
                var takeResul = resultOperator as TakeResultOperator;
                _queryMetadata.Take = int.Parse(GetFetchXmlExpression(takeResul.Count));
            } else if (resultOperator is CountResultOperator)
            {
                _queryMetadata.IsCount = true;
            }
            base.VisitResultOperator(resultOperator, queryModel, index);
        }

        public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
        {
            WhereClauseEntityVisitor whereClauseEntityVisitor = new WhereClauseEntityVisitor(whereClause, queryModel);

            _queryMetadata.AddWhereParts(whereClauseEntityVisitor.Filters);
        }

        protected override void VisitOrderings(ObservableCollection<Ordering> orderings, QueryModel queryModel, OrderByClause orderByClause)
        {
            base.VisitOrderings(orderings, queryModel, orderByClause);
        }

        private string GetFetchXmlExpression(Expression expression)
        {
            return FetchXmlExpressionTreeVisitor.GetFetchXml(expression);
        }
    }
}