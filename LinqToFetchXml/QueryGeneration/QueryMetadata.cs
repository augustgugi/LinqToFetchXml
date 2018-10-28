using gugi.LinqToFetchXml.QueryGeneration.Models;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gugi.LinqToFetchXml.QueryGeneration
{
    internal class QueryMetadata
    {
        private StringBuilder _fetchXml = new StringBuilder();


        public QueryMetadata()
        {
            FromParts = new List<string>();
            WhereParts = new List<string>();
        }

        public Type ReturningType { get; set; }
        public string EntityName { get; set; }
        public Type EntityType { get; set; }
        public List<SelectAttributes> SelectAttributes = new List<SelectAttributes>();
        public List<LinkEntity> LinkEntities = new List<LinkEntity>();
        private SortedList<int, OrderAttribute> OrderAttributes = new SortedList<int, OrderAttribute>(); // the data is mostlz sorted so we go with SortedList

        public void AddSelectAttributes(SelectAttributes selectAttributes)
        {
            SelectAttributes.Add(selectAttributes);
        }
        
        private List<string> FromParts { get; set; }
        private List<string> WhereParts { get; set; }
        public string SelectPart { get; set; }
        public string FilterType { get; set; }
        public int? Take { get; set; } = null;
        public bool IsCount { get; set; } = false;

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
            OrderAttributes.Add(index, new OrderAttribute()
            {
                Attribute = orderBy,
                IsDescending = isDescending
            });
        }

        public void AddJoin(string entityFrom, string attributeFrom, string entityTo, string attributeTo)
        {
            LinkEntities.Add(new LinkEntity(entityFrom, attributeFrom, entityTo, attributeTo));
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

            _fetchXml.AppendLine($"<entity name='{EntityName}' >");

            var attributes = SelectAttributes.Where(sa => sa.EntityLogicalName == EntityName).FirstOrDefault();
            if (attributes != null)
            {
                if (attributes.AllAttributes)
                {
                    _fetchXml.AppendLine("<all-attributes/>");
                }
                else
                {
                    string attrFetch = String.Join("\n", attributes.AttributesLogicalNames.Select(a => $"<attribute name='{a}' />"));
                    _fetchXml.AppendLine(attrFetch);
                }
            }
            else
            {
                if (ReturningType == EntityType)
                {
                    _fetchXml.AppendLine("<all-attributes/>");
                }
            }



            foreach (var orderBy in OrderAttributes)
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

            AppendJoins(EntityName);

            _fetchXml.AppendLine("</entity>");
            _fetchXml.AppendLine("</fetch>");

            return _fetchXml.ToString();
        }

        private void AppendJoins(string toEntity)
        {
            var mainEntityJoins = LinkEntities.Where(le => le.EntityTo == toEntity).ToList();
            foreach (var join in mainEntityJoins)
            {
                _fetchXml.AppendLine($"<link-entity name='{join.EntityFrom}'  to='{join.AttributeTo}' from='{join.AttributeFrom}'>");
                AppendJoins(join.EntityFrom);
                _fetchXml.AppendLine($"</link-entity>");
            }
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
    }
}