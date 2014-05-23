using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Usergroup.Sugnl.Search;

namespace Website.Models
{
    public class SearchPagingModel
    {
        public SearchCriteria Criteria { get; set; }
        public int NumberOfPages { get; set; }
    }
}