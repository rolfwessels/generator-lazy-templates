using System.Collections.Generic;

namespace MainSolutionTemplate.Api.WebApi.ODataSupport
{
    public class ODataPageResult<T>
    {
        public List<T> Items { get; set; }

        public long? Count { get; set; }
    }
}