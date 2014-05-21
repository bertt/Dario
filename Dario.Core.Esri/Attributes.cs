using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dario.Core.Esri
{
    public class Attributes
    {
        public int status { get; set; }
        public int objectid { get; set; }
        public string req_id { get; set; }
        public string req_type { get; set; }
        public string req_date { get; set; }
        public string req_time { get; set; }
        public string address { get; set; }
        public string x_coord { get; set; }
        public string y_coord { get; set; }
        public string district { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateGps { get; set; }
        public double Speed { get; set; }
    }
}
