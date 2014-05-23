using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Usergroup.Sugnl.Search;

namespace Website.Models
{
    public class SearchFacetModel
    {
        public ISearchCriteria Criteria { get; set; }
        public ISearchResult Result { get; set; }
        //public IEnumerable<IFacet> HasPoint { get; set; }
        //public IEnumerable<IFacet> HasDescription { get; set; }
        //public IEnumerable<IFacet> Categories { get; set; }
    }
}