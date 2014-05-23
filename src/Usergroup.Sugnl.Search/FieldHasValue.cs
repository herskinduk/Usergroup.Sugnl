using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Usergroup.Sugnl.Search
{
    public class FieldHasValue : IComputedIndexField
    {
        public FieldHasValue() : base()
        {

        }

        public FieldHasValue(XmlNode configNode)
            : base()
        {
            string sourceFieldName = XmlUtil.GetAttribute("sourceFieldName", configNode, true);
            Sitecore.Diagnostics.Log.Info("---> Constructor (XmlNode) " + sourceFieldName, this);

            this.SourceFieldName = sourceFieldName;
        }

        public string FieldName
        {
            get;
            set;
        }

        public string SourceFieldName
        {
            get;
            set;
        }

        public string ReturnType
        {
            get;
            set;
        }

        public object ComputeFieldValue(Sitecore.ContentSearch.IIndexable indexable)
        {
            var indexableItem = indexable as SitecoreIndexableItem;
            if (indexableItem == null)
                return null;

            var item = (Sitecore.Data.Items.Item)indexableItem;
            if (item == null)
                return null;

            var value = item[SourceFieldName];

            return (value != "").ToString();
        }
    }
}
