using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usergroup.Sugnl.Search
{
    public class CachingSearchServiceDecorator : ISearchService
    {
        public readonly ISearchService innerService;
        public readonly ICache<ISearchResult> cache;

        public CachingSearchServiceDecorator(ISearchService innerService, ICache<ISearchResult> cache)
        {
            this.innerService = innerService;
            this.cache = cache;
        }

        public ISearchResult Search(SearchCriteria criteria)
        {
            var key = "LazySearchService";
            if (criteria != null)
                key += criteria.GetHashCode().ToString();

            if (!cache.ContainsKey(key))
            {
                cache.AddEntity(key, innerService.Search(criteria));
            }

            return cache.GetEntity(key);
        }
    }
}
