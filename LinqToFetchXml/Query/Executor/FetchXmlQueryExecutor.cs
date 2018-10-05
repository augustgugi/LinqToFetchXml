using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gugi.LinqToFetchXml.QueryGeneration;
using Remotion.Linq;

namespace gugi.LinqToFetchXml.Query.Executor
{
    internal sealed class FetchXmlQueryExecutor : IQueryExecutor
    {
        internal FetchXmlQueryExecutor(ICustomFetchXmlQueryExecutor userFetchXmlQueryExecutor)
        {
            UserFetchXmlQueryExecutor = userFetchXmlQueryExecutor;
        }

        public ICustomFetchXmlQueryExecutor UserFetchXmlQueryExecutor { get; }

        // Executes a query with a scalar result, i.e. a query that ends with a result operator such as Count, Sum, or Average.
        public T ExecuteScalar<T>(QueryModel queryModel)
        {
            return ExecuteCollection<T>(queryModel).FirstOrDefault();
        }

        // Executes a query with a single result object, i.e. a query that ends with a result operator such as First, Last, Single, Min, or Max.
        public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
        {
            return returnDefaultWhenEmpty ? ExecuteCollection<T>(queryModel).SingleOrDefault() : ExecuteCollection<T>(queryModel).Single();
        }

        // Executes a query with a collection result.
        public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
        {
            var commandData = FetchXmlQueryModelVisitor.GetCommand(queryModel);
            var query = commandData.Build();

            var result = UserFetchXmlQueryExecutor.ExecuteSync<T>(query);

            return result;
        }
    }
}
