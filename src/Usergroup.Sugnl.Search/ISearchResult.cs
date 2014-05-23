using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Usergroup.Sugnl.Search
{
    public interface ISearchResult
    {
        IEnumerable<SearchResultItem> ResultDocuments { get; set; }
        IEnumerable<IFacet> Facets { get; set; }
        int TotalSearchResults { get; set; }
        int TotalMilliseconds { get; set; }
    }
}
