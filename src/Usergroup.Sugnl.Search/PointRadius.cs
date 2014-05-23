using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Usergroup.Sugnl.Search
{
    public class PointRadius : IPointRadius
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double RadiusKm { get; set; }
    }
}