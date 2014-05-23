using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Usergroup.Sugnl.Hack
{
    public class PointRadiusCriterion
    {
        public enum SortOption
        {
            None,
            Ascending,
            Descending
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double RadiusKm { get; set; }
        public string FieldName { get; set; }
        public SortOption Sort { get; set; }
    }
}