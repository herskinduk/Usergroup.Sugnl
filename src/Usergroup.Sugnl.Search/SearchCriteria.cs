using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Usergroup.Sugnl.Search;

namespace Usergroup.Sugnl.Search
{
    public class SearchCriteria : ISearchCriteria
    {
        public SearchCriteria()
        {
            Text = "";
            CurrentPage = 1;
            PageSize = 10;
            FacetFilters = new Dictionary<string, string>(); 
        }

        public string Text { get; set; }
        public IDictionary<string, string> FacetFilters { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public PointRadius PointRadius { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Text != null ? Text.GetHashCode() : 0);
                result = (result * 397) ^ PageSize.GetHashCode();
                foreach(var facet in FacetFilters.OrderBy(kv => kv.Key))
                {
                    result = (result * 397) ^ (facet.Key != null ? facet.Key.GetHashCode() : 0);
                    result = (result * 397) ^ (facet.Value != null ? facet.Value.GetHashCode() : 0);
                }
                result = (result * 397) ^ CurrentPage.GetHashCode();
                return result;
            }
        }
    }
}