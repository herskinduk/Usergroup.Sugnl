using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usergroup.Sugnl.Search
{
    public class Facet : IFacet
    {

        public string DisplayName
        {
            get;
            set;
        }

        public string FieldName
        {
            get;
            set;
        }




        public IEnumerable<IFacetValue> FacetValues
        {
            get;
            set;
        }
    }
}
