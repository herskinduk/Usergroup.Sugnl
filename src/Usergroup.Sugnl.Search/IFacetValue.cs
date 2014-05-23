using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Usergroup.Sugnl.Search
{
    public interface IFacetValue
    {
        string DisplayName { get; }
        string Value { get;  }
        int DocumentCount { get; }

        IEnumerable<IFacet> NestedFacets { get; }
    }
}
