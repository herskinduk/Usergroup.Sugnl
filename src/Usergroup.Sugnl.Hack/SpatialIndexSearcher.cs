namespace Usergroup.Sugnl.Hack
{
    using Lucene.Net.Analysis;
    using Lucene.Net.Index;
    using Lucene.Net.Search;
    using Lucene.Net.Search.Function;
    using Lucene.Net.Spatial.Prefix;
    using Lucene.Net.Spatial.Prefix.Tree;
    using Lucene.Net.Spatial.Queries;
    using Lucene.Net.Spatial.Util;
    using Lucene.Net.Store;
    using Sitecore.Common;
    using Spatial4n.Core.Context;
    using Spatial4n.Core.Distance;
    using Spatial4n.Core.Shapes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    public class SpatialIndexSearcher : IndexSearcher, IDisposable
    {
        public SpatialIndexSearcher(IndexReader r)
            : base(r)
        { }
        public SpatialIndexSearcher(Directory path)
            : base(path)
        { }
        public SpatialIndexSearcher(Directory path, bool readOnly)
            : base(path, readOnly)
        { }
        public SpatialIndexSearcher(IndexReader reader, IndexReader[] subReaders, int[] docStarts)
            : base(reader, subReaders, docStarts)
        { }

        private Filter spatialFilter = null;
        private Weight spatialWeight = null;

        public void CreateSpatialFilterAndWeight(PointRadiusCriterion geoFilter, Filter currentFilter, Weight currentWeight)
        {
            var spatialContext = SpatialContext.GEO;
            var geohashTree = new GeohashPrefixTree(spatialContext, 10);
            var strategy = new RecursivePrefixTreeStrategy(geohashTree, geoFilter.FieldName);
            var point = spatialContext.MakePoint(geoFilter.Longitude, geoFilter.Latitude);

            var spatialArgs = new SpatialArgs(SpatialOperation.Intersects, spatialContext.MakeCircle(point,
                DistanceUtils.Dist2Degrees(geoFilter.RadiusKm, DistanceUtils.EARTH_MEAN_RADIUS_KM)));

            var circle = spatialContext.MakeCircle(point,
                    DistanceUtils.Dist2Degrees(geoFilter.RadiusKm, DistanceUtils.EARTH_MEAN_RADIUS_KM));
            var circleCells = strategy.GetGrid().GetWorldNode().GetSubCells(circle);

            var luceneFilters = new List<Filter>();
            if (currentFilter != null)
                luceneFilters.Add(currentFilter);

            var tempSpatial = strategy.MakeFilter(spatialArgs);
                luceneFilters.Add(tempSpatial);
            
            if (geoFilter.Sort != PointRadiusCriterion.SortOption.None)
            {
                var valueSource = strategy.MakeDistanceValueSource(point);
                var funcQ = new FunctionQuery(valueSource);
                // this is a bit odd... but boosting the score negatively orders results 
                if (geoFilter.Sort == PointRadiusCriterion.SortOption.Ascending)
                {
                    funcQ.Boost = -1;
                }
                spatialWeight = funcQ.CreateWeight(this);
                spatialWeight.GetSumOfSquaredWeights();

                luceneFilters.Add(new QueryWrapperFilter(currentWeight.Query));
            }

            spatialFilter = new ChainedFilter(luceneFilters.ToArray(), 1);
        }

        public override void Search(Query query, Collector results)
        {
            base.Search(query, results);
        }

        public override TopDocs Search(Query query, Filter filter, int n)
        {
            return base.Search(query, filter, n);
        }

        public override void Search(Weight weight, Filter filter, Collector collector)
        {
            if (SpatialFilterContext.CurrentValue != null && SpatialFilterContext.CurrentValue.Filter != null)
            {
                CreateSpatialFilterAndWeight(SpatialFilterContext.CurrentValue.Filter, filter, weight);
            }

            base.Search(spatialWeight ?? weight, spatialFilter ?? filter, collector);

            spatialFilter = null;
            spatialWeight = null;
        }


        public override TopFieldDocs Search(Weight weight, Filter filter, int nDocs, Sort sort)
        {
            return base.Search(weight, filter, nDocs, sort);
        }

    }
}
