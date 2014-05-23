using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usergroup.Sugnl.Search
{
    public class SearchResult : ISearchResult
    {
        public SearchResult()
        {
            ResultDocuments = new List<Sitecore.ContentSearch.SearchTypes.SearchResultItem>();
            Facets = new List<IFacet>();
        }

        public IEnumerable<Sitecore.ContentSearch.SearchTypes.SearchResultItem> ResultDocuments
        {
            get;
            set;
        }

        public int TotalSearchResults
        {
            get;
            set;
        }

        public int TotalMilliseconds
        {
            get;
            set;
        }


        public IEnumerable<IFacet> Facets
        {
            get;
            set;
        }
    }
}
