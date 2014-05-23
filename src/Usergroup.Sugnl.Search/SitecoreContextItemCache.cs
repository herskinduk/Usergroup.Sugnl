using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usergroup.Sugnl.Search
{
    public class SitecoreContextItemCache : ICache<ISearchResult>
    {
        public bool ContainsKey(string key)
        {
            return Sitecore.Context.Items.Contains(key);
        }

        public ISearchResult GetEntity(string key)
        {
            return Sitecore.Context.Items[key] as ISearchResult;
        }

        public void AddEntity(string key, ISearchResult result)
        {
            Sitecore.Context.Items.Add(key, result);
        }
    }
}