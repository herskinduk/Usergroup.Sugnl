using Sitecore.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usergroup.Sugnl.Hack
{
    public class SpatialFilterContext : Switcher<SpatialFilterContext.CriterionContainer>
    {
        public class CriterionContainer
        {
            public PointRadiusCriterion Filter { get; set; }
        }

        public SpatialFilterContext(CriterionContainer criterionHolder)
            : base(criterionHolder)
        {
        }
    }
}
