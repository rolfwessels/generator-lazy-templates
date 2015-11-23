using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Antlr.Runtime.Misc;
using LinqToQuerystring;
using MainSolutionTemplate.Api.Properties;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;

namespace MainSolutionTemplate.Api.WebApi.ODataSupport
{
    public class QueryToODataWrapper<TDto, TModel> : IEnumerable<TModel>, IQueryToODataWrapper
    {
        private readonly IQueryable<TDto> _filtered;
        private readonly Func<IQueryable<TDto>, IEnumerable<TModel>> _map;
        private readonly IQueryable<TDto> _queryable;
        private string _originalDataQuery;


        public QueryToODataWrapper(IQueryable<TDto> queryable, string query,
            Func<IQueryable<TDto>, IEnumerable<TModel>> map)
        {
            _originalDataQuery = "";

            _queryable = queryable;
            _map = map;
            if (string.IsNullOrEmpty(query))
            {
                _filtered = _queryable;

            }
            else
            {
                HackRemoveInlinecount(query);
                _filtered = _queryable.LinqToQuerystring(_originalDataQuery, false, GetDefaultMaxResultCount(query));
            }
        }


        public IQueryable<TDto> NoCountFilter
        {
            get
            {
                string dataStringWithNoTop = GetDataStringWithNoTop();
                return _queryable.LinqToQuerystring(dataStringWithNoTop);
            }
        }

        public IEnumerable<TModel> Items
        {
            get { return _map(_filtered); }
        }

        #region Implementation of IEnumerable

        public IEnumerator<TModel> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of IQueryToODataWrapper

        public long? Count
        {
            get { return NoCountFilter.LongCount(); }
        }

        public bool RequiresPagedValue { get; private set; }

        public object GetPagedResult()
        {
            return new ODataPageResult<TModel>
            {
                Count = NoCountFilter.LongCount(),
                Items = Items.ToList()
            };
        }

        #endregion

        #region Private Methods

        private static int GetDefaultMaxResultCount(string query)
        {
            return query.Contains("$top") ? -1 : Settings.Default.DefaultMaxResultCount;
        }

        private void HackRemoveInlinecount(string query)
        {
            _originalDataQuery = HttpUtility.UrlDecode(query);

            if (_originalDataQuery != null)
            {
                RequiresPagedValue = _originalDataQuery.Contains("$inlinecount=allpages");
                _originalDataQuery = _originalDataQuery.Replace("$inlinecount=allpages", "");
            }
        }

        private string GetDataStringWithNoTop()
        {
            string noTopQuery = Regex.Replace(_originalDataQuery, @"\$top=[0-9]+", "", RegexOptions.IgnoreCase);
            noTopQuery = Regex.Replace(noTopQuery, @"\$skip=[0-9]+", "", RegexOptions.IgnoreCase);
            return noTopQuery;
        }

        #endregion
    }
}