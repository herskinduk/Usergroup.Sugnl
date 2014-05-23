using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usergroup.Sugnl.Search
{
    public interface IFacet
    {
        string DisplayName { get; }
        string FieldName { get; }
        IEnumerable<IFacetValue> FacetValues { get;  }
    }
}
