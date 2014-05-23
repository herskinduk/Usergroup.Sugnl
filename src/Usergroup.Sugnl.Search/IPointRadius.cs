using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usergroup.Sugnl.Search
{
    public interface IPointRadius
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
        double RadiusKm { get; set; }
    }
}
