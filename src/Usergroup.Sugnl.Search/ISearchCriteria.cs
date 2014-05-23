using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usergroup.Sugnl.Search
{
    public interface ISearchCriteria
    {
        string Text { get; set; }
        IDictionary<string,string> FacetFilters { get; set; }
        int CurrentPage { get; set; }
        int PageSize { get; set; }
    }
}
