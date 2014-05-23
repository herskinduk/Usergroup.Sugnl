using Sitecore.ContentSearch.LuceneProvider;
using Sitecore.ContentSearch.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usergroup.Sugnl.Hack
{
    public class SpatialLuceneSearchContext : LuceneSearchContext, IDisposable
    {
        SpatialFilterContext spatialFilterContext;

        public SpatialLuceneSearchContext(ILuceneProviderIndex index, SearchSecurityOptions securityOptions = SearchSecurityOptions.EnableSecurityCheck)
            : base(index, securityOptions)
        {
            spatialFilterContext = new SpatialFilterContext(new SpatialFilterContext.CriterionContainer());
        }

        protected SpatialLuceneSearchContext(ILuceneProviderIndex index, CreateSearcherOption options = CreateSearcherOption.Writeable, SearchSecurityOptions securityOptions = SearchSecurityOptions.EnableSecurityCheck)
            : base(index, options, securityOptions)
        {
        }

        protected SpatialIndexSearcher GeoIndexSearcher
        {
            get
            {
                return this.Searcher as SpatialIndexSearcher;
            }
        }

        public void SetGeoFilter(PointRadiusCriterion geoFilter)
        {
            SpatialFilterContext.CurrentValue.Filter = geoFilter;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (spatialFilterContext != null) spatialFilterContext.Dispose();
            }
        }
    }
}
