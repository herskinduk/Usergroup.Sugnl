using Lucene.Net.Search;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.LuceneProvider;
using Sitecore.ContentSearch.Maintenance;
using Sitecore.ContentSearch.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usergroup.Sugnl.Hack
{
    public class SpatialLuceneIndex : LuceneIndex
    {
        public SpatialLuceneIndex(string name, string folder, IIndexPropertyStore propertyStore): base(name, folder, propertyStore)
        {
        }

        private IndexSearcher cachedReadonlyIndexSearcher;

        public override Lucene.Net.Search.IndexSearcher CreateSearcher(CreateSearcherOption options)
        {
            this.EnsureInitialized();
            if ((options & CreateSearcherOption.Readonly) == CreateSearcherOption.Readonly)
            {
                if ((options & CreateSearcherOption.Cached) != CreateSearcherOption.Cached)
                {
                    return new SpatialIndexSearcher(this.CreateReader(true));
                }
                if (this.cachedReadonlyIndexSearcher != null)
                {
                    return this.cachedReadonlyIndexSearcher;
                }
                return (this.cachedReadonlyIndexSearcher = new SpatialIndexSearcher(this.CreateReader(true)));
            }

            return new SpatialIndexSearcher(this.CreateReader(false));
        }

        public override IndexSearcher CreateSearcher(bool readOnly)
        {
            this.EnsureInitialized();
            if (!readOnly)
            {
                return new SpatialIndexSearcher(this.CreateReader(false));
            }
            return new SpatialIndexSearcher(this.Directory, true);
        }

        public override IProviderSearchContext CreateSearchContext(SearchSecurityOptions securityOptions = SearchSecurityOptions.EnableSecurityCheck)
        {
            this.EnsureInitialized();
            return new SpatialLuceneSearchContext(this, securityOptions);
        }
    }
}
