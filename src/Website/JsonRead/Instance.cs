using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Usergroup.Sugnl.JsonReadSpike
{
    public class Instance
    {
        public Instance ()
        {
            Categories = new List<string>();
        }

        public string ID { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Point { get; set; }
        public List<string> Categories { get; set; }
    }
}
