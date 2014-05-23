using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Usergroup.Sugnl.Hack;

namespace Usergroup.Sugnl.Search
{
    public class ContentSearchService : ISearchService
    {
        // todo: this should be injected
        private readonly DisplayNameResolver resolver = new DisplayNameResolver();

        public ContentSearchService()
        {

        }

        public ISearchResult Search(SearchCriteria criteria)
        {
            if (criteria == null || string.IsNullOrWhiteSpace(criteria.Text))
                return new SearchResult();

            var timer = new Sitecore.Diagnostics.HighResTimer(true);

            var index = ContentSearchManager.GetIndex("sitecore_master_index") as SpatialLuceneIndex;

            using (var context = index.CreateSearchContext() as SpatialLuceneSearchContext)
            {
                if (criteria.PointRadius != null)
                {
                    context.SetGeoFilter(new PointRadiusCriterion()
                    {
                        FieldName = "_geohash",
                        RadiusKm = criteria.PointRadius.RadiusKm,
                        Latitude = criteria.PointRadius.Latitude,
                        Longitude = criteria.PointRadius.Longitude,
                        Sort = PointRadiusCriterion.SortOption.Ascending
                    });
                }

                var queriable = context.GetQueryable<SearchResultItem>();

                Expression<Func<SearchResultItem, bool>> predicate = PredicateBuilder.True<SearchResultItem>();
                foreach(var entry in criteria.Text.Split(' '))
                {
                    predicate = predicate.And(i => i.Content == entry);
                }
                
                var result = queriable
                    .Where(predicate)
                    .Page(criteria.CurrentPage-1, criteria.PageSize)
                    .FacetPivotOn(p => p.FacetOn(d => d["categories"]).FacetOn(d => d["haspoint"]))
                    .GetResults();

                var searchResult = new SearchResult()
                {
                    ResultDocuments = result.Hits.Select(hit => hit.Document).ToList(),
                    Facets = CollectFacets(result.Facets.Categories),
                    TotalSearchResults = result.TotalSearchResults,
                };
                searchResult.TotalMilliseconds = timer.ElapsedTimeSpan.Milliseconds;

                return searchResult;
            }
        }

        // TODO: Refactor FacetCollector

        private IEnumerable<IFacet> CollectFacets(List<FacetCategory> facetCategoryList)
        {
            var list = new List<Facet>();

            foreach (var resultFacetCategory in facetCategoryList)
            {
                var neestedFacetNames = resultFacetCategory.Name.Split(',');

                list.Add(new Facet()
                {
                    DisplayName = resolver.ResolveFacetName(neestedFacetNames[0]), // todo: use name resolver
                    FieldName = neestedFacetNames[0],
                    FacetValues = CollectFacetValues(resultFacetCategory.Values, neestedFacetNames, 0, new List<string>()),
                });
            }

            return list;
        }

        private IEnumerable<IFacet> CollectNestedFacets(List<Sitecore.ContentSearch.Linq.FacetValue> facetValueList, string[] neestedFacetNames, int level, List<string> parentPathList)
        {
            var list = new List<Facet>();

            var parentPath = string.Join("/", parentPathList.ToArray());

            foreach (var resultFacetCategory in facetValueList
                    .Where(d=> d.Name.StartsWith(parentPath))
                    .Select(d=> d.Name)
                    .Select(s=> s.Substring(parentPath.Length))
                    .Where(s => s.Length > 0)
                    .Select(s=> s.Substring(0, (s.IndexOf('/') < 0) ? s.Length : s.IndexOf('/')))
                    .Distinct())
            {
                list.Add(new Facet()
                {
                    DisplayName = resolver.ResolveFacetName(neestedFacetNames[level]), // todo: use name resolver
                    FieldName = neestedFacetNames[level],
                    FacetValues = CollectFacetValues(facetValueList, neestedFacetNames, level + 1, parentPathList)
                });
            }

            return list;
        }

        private IEnumerable<IFacetValue> CollectFacetValues(List<Sitecore.ContentSearch.Linq.FacetValue> facetValueList, string[] neestedFacetNames, int level, List<string> parentPathList)
        {
            var list = new List<FacetValue>();

            var parentPath = string.Join("/", parentPathList.ToArray());

            if (level > neestedFacetNames.Length)
                return list;

            foreach (var resultFacetValue in 
                facetValueList
                    .Where(d=> d.Name.StartsWith(parentPath))
                    .Select(d=> d.Name.Split('/'))
                    .Where(l => l.Count() > parentPathList.Count())
                    .Select(l=> l.Skip(parentPathList.Count()).First())
                    .Distinct())
            {
                var nextLevelPathList = new List<string>(parentPathList);
                nextLevelPathList.Add(resultFacetValue);
                var nextLevelPath = string.Join("/", nextLevelPathList.ToArray());

                var sum = facetValueList.Where(d => d.Name.StartsWith(nextLevelPath)).Select(d => d.AggregateCount).Sum();

                list.Add(new FacetValue()
                {
                    DisplayName = resolver.ResolveFacetValueName(resultFacetValue), // todo: use value resolver
                    Value = resultFacetValue,
                    DocumentCount = sum,
                    NestedFacets = CollectNestedFacets(facetValueList, neestedFacetNames, level + 1, nextLevelPathList)
                });
            }

            return list;
        }

    }
}
