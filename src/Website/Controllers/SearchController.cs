using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Usergroup.Sugnl.Hack;
using Usergroup.Sugnl.Search;
using Website.Models;

namespace Website.Controllers
{
    public class SearchController : Controller
    {
        ISearchService searchService = null;

        public SearchController()
            : this(new CachingSearchServiceDecorator(new ContentSearchService(), new SitecoreContextItemCache()))
            //: this(new ContentSearchService())
        {
        }

        public SearchController(ISearchService service)
        {
            searchService = service;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Input(SearchInputModel model)
        {
            var criteria = (model != null) ? model.Criteria : new SearchCriteria();

            var searchResult = searchService.Search(criteria);

            return View(new SearchInputModel() 
            { 
                Criteria = criteria, 
                NumberOfResults = (searchResult != null) ? searchResult.TotalSearchResults : 0,
                TimeTaken = (searchResult != null) ? searchResult.TotalMilliseconds : 0
            });
        }

        public ActionResult Result(SearchCriteria criteria)
        {
            if (criteria == null)
                criteria = new SearchCriteria();

            var searchResult = searchService.Search(criteria);

            return View(searchResult);
        }


        public ActionResult Facet(SearchCriteria criteria)
        {
            if (criteria == null)
                criteria = new SearchCriteria();

            var searchResult = searchService.Search(criteria); 

            return View(new SearchFacetModel() 
            { 
                Criteria = criteria, 
                Result = searchResult
            });
        }

        public ActionResult Paging(SearchCriteria criteria)
        {
            if (criteria == null)
                criteria = new SearchCriteria();

            var searchResult = searchService.Search(criteria);
            var numberOfPages = Convert.ToInt32(Math.Ceiling(searchResult.TotalSearchResults / (double)criteria.PageSize));

            return View(new SearchPagingModel() { Criteria = criteria, NumberOfPages = numberOfPages });
        }


        public ActionResult Search(int radius)
        {
            if (radius != null)
            {
                var index = ContentSearchManager.GetIndex("sitecore_master_index") as SpatialLuceneIndex;

                using (var context = index.CreateSearchContext() as SpatialLuceneSearchContext)
                {
                    context.SetGeoFilter(new PointRadiusCriterion()
                    {
                        FieldName = "_geohash",
                        RadiusKm = Convert.ToDouble(radius),
                        Latitude = 51.891796,
                        Longitude = 0.901473,
                        Sort = PointRadiusCriterion.SortOption.Ascending
                    });

                    var queriable = context.GetQueryable<SearchResultItem>()
                        .Filter(i => i["haspoint"] == "true");
                        //.Page(1,3);
                        //.OrderByDescending(i => i.Name);

                    var result = queriable.GetResults();

                    return View(result.Hits.ToList());
                }
            }

            return View();
        } 
	}
}