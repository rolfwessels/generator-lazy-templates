using System.Collections.Generic;

namespace MainSolutionTemplate.Sdk.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int Count { get; set; }
    }
}