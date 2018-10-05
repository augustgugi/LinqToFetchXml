using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gugi.LinqToFetchXml.Query.Executor
{
    public interface ICustomFetchXmlQueryExecutor
    {
        IEnumerable<T> ExecuteSync<T>(string fetchXml);
    }
}
