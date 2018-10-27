using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gugi.LinqToFetchXml.QueryGeneration;
using Microsoft.Xrm.Sdk;
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
            var queryMetadata = FetchXmlQueryModelVisitor.GetQueryMetadata(queryModel);

            var result = ExecuteQuery<T>(queryModel, queryMetadata);
            var asEnumerable = (System.Collections.IEnumerable)(result);

            if (queryMetadata.IsCount)
            {
                int tot = 0;
                foreach (var res in asEnumerable)
                {
                    tot++;
                }
                return (T)(object)tot;
            }

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
            var queryMetadata = FetchXmlQueryModelVisitor.GetQueryMetadata(queryModel);

            var result = ExecuteQuery<T>(queryModel, queryMetadata);
            var asEnumerable = (System.Collections.IEnumerable)(result);
            var typeofT = typeof(T);
            if (typeof(T) != queryMetadata.EntityType)
            {

                foreach (Entity rec in asEnumerable)
                {
                    dynamic instance = Activator.CreateInstance(queryMetadata.ReturningType, rec["name"]);

                    var asObj = (object)instance;
                    var asT = (T)instance;

                    yield return asT;
                }
            }
            else
            {
                foreach (T rec in asEnumerable)
                {
                    yield return rec;
                };
            }            
        }

        private object ExecuteQuery<T>(QueryModel queryModel, QueryMetadata queryMetadata)
        {
            string query = queryMetadata.GetFetchXmlQuery();
            object result = null;
            if (typeof(T) == queryMetadata.EntityType)
            {
                result = UserFetchXmlQueryExecutor.ExecuteSync<T>(query);
            }
            else
            {
                result = ExecuteWithTheRightType(query, queryMetadata.EntityType);
            }

            return result;
        }

        private object ExecuteWithTheRightType(string query, Type type)
        {
            var methodInfo = UserFetchXmlQueryExecutor.GetType().GetMethod("ExecuteSync", new Type[] { typeof(string) });
            var methodGeneric = methodInfo.MakeGenericMethod(type);
            var result = methodGeneric.Invoke(UserFetchXmlQueryExecutor, new object[] { query });

            return result;
        }
    }
}
