using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Text;

namespace gugi.LinqToFetchXml.QueryGeneration
{
    public class QueryPartsAggregator
    {
        private StringBuilder _fetchXml = new StringBuilder();

        public QueryPartsAggregator()
        {
            FromParts = new List<string>();
            WhereParts = new List<string>();
        }

        private string MainFromPart { get; set; }
        private List<string> FromParts { get; set; }
        private List<string> WhereParts { get; set; }
        private SortedList<int, OrderAttribute> orders = new SortedList<int, OrderAttribute>(); // the data is mostlz sorted so we go with SortedList
        public string SelectPart { get; set; }
        public string FilterType { get; set; }
        public int? Take { get; set; } = null;
        private SortedList<int, LinkEntity> joins = new SortedList<int, LinkEntity>();

        public void AddMainPart(string entityName)
        {
            MainFromPart = entityName;
        }

        public void AddFromPart(IQuerySource querySource)
        {
            FromParts.Add(querySource.ItemName);
        }

        public void AddWhereParts(string format, params object[] args)
        {
            WhereParts.Add(String.Format(format, args));
        }

        public void AddOrderByPart(string orderBy, bool isDescending, int index)
        {
            orders.Add(index, new OrderAttribute()
            {
                Attribute = orderBy,
                IsDescending = isDescending
            });
        }

        public void AddJoin(int index, string entityFrom, string attributeFrom, string entityTo, string attributeTo)
        {
            joins.Add(index, new LinkEntity()
            {
                EntityFrom = entityFrom,
                AttributeFrom = attributeFrom,
                EntityTo = entityTo,
                AttributeTo = attributeTo
            });
        }

        public string GetFetchXmlQuery()
        {
            if (Take.HasValue)
            {
                _fetchXml.AppendLine($"<fetch mapping='logical' count='{Take.Value}'>");
            }
            else
            {
                _fetchXml.AppendLine("<fetch mapping='logical'>");
            }
            
            _fetchXml.AppendLine($"<entity name='{MainFromPart}' >");
            _fetchXml.AppendLine("<all-attributes/>");

            foreach(var orderBy in orders)
            {
                var orderAttr = orderBy.Value;

                _fetchXml.AppendLine($"<order attribute='{orderAttr.Attribute}' descending='{orderAttr.Descending}' />");
            }

            if (WhereParts.Count > 0)
            {
                _fetchXml.AppendFormat("<filter type='{0}'>", FilterType);
                
                foreach (var where in WhereParts)
                {
                    _fetchXml.AppendLine(where);
                }
                _fetchXml.AppendLine("</filter>");
            }

            foreach (var join in joins)
            {
                var joinLink = join.Value;

                _fetchXml.AppendLine($"<link-entity name='{joinLink.EntityFrom}'  to='{joinLink.AttributeTo}' from='{joinLink.AttributeFrom}'>");
                _fetchXml.AppendLine($"</link-entity>");
            }

            _fetchXml.AppendLine("</entity>");
            _fetchXml.AppendLine("</fetch>");

            return _fetchXml.ToString();
        }

        private class OrderAttribute
        {
            public string Attribute { get; set; }
            public string Descending {
                get
                {
                    return IsDescending ? "true" : "false";
                }
            }


            public bool IsDescending { get; set; }
        }

        private class LinkEntity
        {
            public string EntityFrom { get; set; }
            public string AttributeFrom { get; set; }
            public string EntityTo { get; set; }
            public string AttributeTo { get; set; }
        }
    }
}