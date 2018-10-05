using gugi.LinqToFetchXml.Metadata;
using gugi.LinqToFetchXml.Query.CustomClauseVisitors;
using gugi.LinqToFetchXml.QueryGeneration.Clauses;
using gugi.LinqToFetchXml.QueryGeneration.NodeProviders;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace gugi.LinqToFetchXml.QueryGeneration
{
    internal class FetchXmlQueryModelVisitor : QueryModelVisitorBase
    {
        private readonly QueryPartsAggregator _queryParts = new QueryPartsAggregator();

        internal static CommandData GetCommand(QueryModel queryModel)
        {
            var visitor = new FetchXmlQueryModelVisitor();
            visitor.VisitQueryModel(queryModel);
            return visitor.GetFetchXmlCommand();
        }

        internal CommandData GetFetchXmlCommand()
        {
            return new CommandData(_queryParts);
        }

        public override void VisitAdditionalFromClause(AdditionalFromClause fromClause, QueryModel queryModel, int index)
        {
            _queryParts.AddFromPart(fromClause);

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

            _queryParts.AddJoin(joinClauseVisitor.Index, joinClauseVisitor.FromEntity, joinClauseVisitor.FromAttribute, joinClauseVisitor.ToEntity, joinClauseVisitor.ToAttribute);

            //_queryParts.AddFromPart(joinClause);
            //_queryParts.AddWherePart(
            //    "({0} = {1})",
            //    GetFetchXmlExpression(joinClause.OuterKeySelector),
            //    GetFetchXmlExpression(joinClause.InnerKeySelector));

            base.VisitJoinClause(joinClause, queryModel, index);
        }

        public override void VisitJoinClause(JoinClause joinClause, QueryModel queryModel, GroupJoinClause groupJoinClause)
        {
            base.VisitJoinClause(joinClause, queryModel, groupJoinClause);
        }

        public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
        {
            EntityModelType entityName = null;
                
            TypeEntityMapping.Instance.Value.TryGetValue(fromClause.ItemType, out entityName);


            _queryParts.AddMainPart(entityName.EntityLogicalName);

            base.VisitMainFromClause(fromClause, queryModel);
        }

        public override void VisitOrderByClause(OrderByClause orderByClause, QueryModel queryModel, int index)
        {
            //_queryParts.AddOrderByPart(orderByClause.Orderings.Select(o => GetFetchXmlExpression(o.Expression)));

            base.VisitOrderByClause(orderByClause, queryModel, index);
        }

        public override void VisitOrdering(Ordering ordering, QueryModel queryModel, OrderByClause orderByClause, int index)
        {

            _queryParts.AddOrderByPart(GetFetchXmlExpression(ordering.Expression), ordering.OrderingDirection == OrderingDirection.Desc, index);

            base.VisitOrdering(ordering, queryModel, orderByClause, index);
        }

        public override void VisitQueryModel(QueryModel queryModel)
        {
            queryModel.SelectClause.Accept(this, queryModel);
            queryModel.MainFromClause.Accept(this, queryModel);
            VisitBodyClauses(queryModel.BodyClauses, queryModel);
            VisitResultOperators(queryModel.ResultOperators, queryModel);
        }

        public override void VisitResultOperator(ResultOperatorBase resultOperator, QueryModel queryModel, int index)
        {
            if (resultOperator is TakeResultOperator)
            {
                var takeResul = resultOperator as TakeResultOperator;
                _queryParts.Take = int.Parse(GetFetchXmlExpression(takeResul.Count));
            }
            base.VisitResultOperator(resultOperator, queryModel, index);
        }
        public override void VisitSelectClause(SelectClause selectClause, QueryModel queryModel)
        {
            //_queryParts.SelectPart = GetFetchXmlExpression(selectClause.Selector);

            base.VisitSelectClause(selectClause, queryModel);
        }

        public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
        {
            _queryParts.AddWhereParts(GetFetchXmlExpression(whereClause.Predicate));

            base.VisitWhereClause(whereClause, queryModel, index);
        }
        protected override void VisitBodyClauses(ObservableCollection<IBodyClause> bodyClauses, QueryModel queryModel)
        {
            foreach (IBodyClause bodyClause in bodyClauses)
            {
                if (bodyClause is FilterTypeClause)
                {
                    var filterTypeClause = bodyClause as FilterTypeClause;
                    _queryParts.FilterType = GetFetchXmlExpression(filterTypeClause.Expression);
                }
            }
            base.VisitBodyClauses(bodyClauses, queryModel);
        }

        protected override void VisitOrderings(ObservableCollection<Ordering> orderings, QueryModel queryModel, OrderByClause orderByClause)
        {
            base.VisitOrderings(orderings, queryModel, orderByClause);
        }

        protected override void VisitResultOperators(ObservableCollection<ResultOperatorBase> resultOperators, QueryModel queryModel)
        {
            base.VisitResultOperators(resultOperators, queryModel);
        }

        private string GetFetchXmlExpression(Expression expression)
        {
            return FetchXmlExpressionTreeVisitor.GetFetchXml(expression);
        }
    }
}