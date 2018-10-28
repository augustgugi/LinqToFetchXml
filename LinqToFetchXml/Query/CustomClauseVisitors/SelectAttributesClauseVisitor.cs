using gugi.LinqToFetchXml.Query.Clauses;
using gugi.LinqToFetchXml.Query.CustomExpressionTreeVisitors;
using gugi.LinqToFetchXml.QueryGeneration.Models;
using Remotion.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.Query.CustomClauseVisitors
{
    internal class SelectAttributesClauseVisitor
    {
        public SelectAttributes SelectAttributes { get; }

        public SelectAttributesClauseVisitor(SelectAttributesClause selectAttributesClause, QueryModel queryModel)
        {
            SelectAttributesExpressionTreeVisitor selectAttributesExpressionTreeVisitor = new SelectAttributesExpressionTreeVisitor(selectAttributesClause.Expression);

            SelectAttributes = new SelectAttributes()
            {
                EntityLogicalName = selectAttributesClause.FetchXmlSet.EntityLogicalName,
                AllAttributes = selectAttributesExpressionTreeVisitor.AllAttributes,
                AttributesLogicalNames = selectAttributesExpressionTreeVisitor.Attributes
            };
        }
    }
}
