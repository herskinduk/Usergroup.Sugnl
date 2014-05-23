using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Usergroup.Sugnl.Search
{
    public class DisplayNameResolver
    {
        public string ResolveFacetName(string name)
        {
            return Sitecore.Context.Culture.TextInfo.ToTitleCase(name);
        }
        public string ResolveFacetValueName(string name)
        {
            Guid id = Guid.Empty;
            if (Guid.TryParse(name, out id))
            {
                var item = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(id));
                if (item != null)
                {
                    return Sitecore.Context.Culture.TextInfo.ToTitleCase(item.Name);
                }
            }

            return Sitecore.Context.Culture.TextInfo.ToTitleCase(name);
        }

    }
}
