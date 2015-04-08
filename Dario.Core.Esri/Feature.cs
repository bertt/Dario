using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace Dario.Core.Esri
{
    public class Feature
    {
        public Feature()
        {
            geometry = new ExpandoObject();
        }
        public dynamic geometry { get; set; }
        public Dictionary<string, object> attributes { get; set; }

        public Extent GetEnvelope()
        {
            var minx = Double.MinValue;
            var miny = Double.MinValue;
            var maxx = Double.MaxValue; 
            var maxy = Double.MaxValue;
            var rings = geometry.rings;

            foreach (var outerring in rings)
            {
                foreach (var innerring in outerring)
                {
                    if (((JArray)innerring).Count > 2)
                    {
                        foreach (var s in innerring)
                        {
                            var lon = (double)s[0];
                            var lat = (double)s[1];
                            minx = lon > minx ? lon : minx;
                            maxx = lon < maxx ? lon : maxx;
                            miny = lat > miny ? lat : miny;
                            maxy = lat < maxy ? lat : maxy;
                        }
                    }
                    else
                    {
                        var lon = (double)innerring[0];
                        var lat = (double)innerring[1];
                        minx = lon > minx ? lon : minx;
                        maxx = lon < maxx ? lon : maxx;
                        miny = lat > miny ? lat : miny;
                        maxy = lat < maxy ? lat : maxy;
                    }
                }
            }
            return new Extent()
            {
                xmax = (float)maxx, xmin = (float)minx,
                ymax = (float)maxy, ymin = (float)miny
            };
        }
    }
}
