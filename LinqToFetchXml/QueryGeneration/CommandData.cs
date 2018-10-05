namespace gugi.LinqToFetchXml.QueryGeneration
{
    internal class CommandData
    {
        public CommandData(QueryPartsAggregator queryParts)
        {
            QueryParts = queryParts;
        }

        public QueryPartsAggregator QueryParts { get; set; }

        public string Build()
        {
            return QueryParts.GetFetchXmlQuery();
        }
    }
}