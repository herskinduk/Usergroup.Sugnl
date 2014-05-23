using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.ContentSearch;
using Spatial4n.Core.Context;
using Lucene.Net.Spatial.Prefix.Tree;
using Lucene.Net.Spatial.Prefix;
using System.Xml;
using Sitecore.Xml;

namespace Usergroup.Sugnl.Hack
{
    public class GeohashComputedField : IComputedIndexField
    {
        public GeohashComputedField() : base()
        {


        }

        public GeohashComputedField(XmlNode configNode)
            : base()
        {
            string sourceFieldName = XmlUtil.GetAttribute("sourceFieldName", configNode, true);
            Sitecore.Diagnostics.Log.Info("---> Constructor (XmlNode) " + sourceFieldName, this);

            this.SourceFieldName = sourceFieldName;
        }

        
        public object ComputeFieldValue(Sitecore.ContentSearch.IIndexable indexable)
        {
            var indexableItem = indexable as SitecoreIndexableItem;
            if (indexableItem == null)
                return null;

            var item = (Sitecore.Data.Items.Item)indexableItem;
            if (item == null)
                return null;

            var latLon = item[SourceFieldName];
            if (latLon == "" || latLon == "NULL")
                return null;

            // Convert from georss point to normal X, Y format
            latLon = latLon.Replace(" ", ", ");

            var spatialContext = SpatialContext.GEO;
            var geohashTree = new GeohashPrefixTree(spatialContext, 10);
            var strategy = new RecursivePrefixTreeStrategy(geohashTree, FieldName);

            var shape = spatialContext.ReadShape(latLon);
            var grid = strategy.GetGrid();
            int levelForDistance = grid.GetLevelForDistance(strategy.DistErrPct);
            IList<Node> list = grid.GetNodes(shape, levelForDistance, true);

            return list.Select(node => node.GetTokenString());
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
    }
}
