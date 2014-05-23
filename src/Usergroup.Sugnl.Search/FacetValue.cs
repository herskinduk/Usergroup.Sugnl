using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usergroup.Sugnl.Search
{
    public class FacetValue : IFacetValue
    {
        public string DisplayName
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public int DocumentCount
        {
            get;
            set;
        }

        public IEnumerable<IFacet> NestedFacets
        {
            get;
            set;
        }
    }
}
