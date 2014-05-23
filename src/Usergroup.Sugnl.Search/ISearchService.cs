using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Usergroup.Sugnl.Search
{
    public interface ISearchService
    {
        ISearchResult Search(SearchCriteria criteria);
    }
}
