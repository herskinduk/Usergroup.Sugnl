using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Usergroup.Sugnl.Search;

namespace Website.Models
{
    public class SearchInputModel
    {
        public SearchCriteria Criteria { get; set; }
        public int NumberOfResults { get; set; }
        public int ZoomLevel { get; set; }
        public int TimeTaken { get; set; }
        
    }
}