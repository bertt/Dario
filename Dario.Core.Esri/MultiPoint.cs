using System.Collections.Generic;

namespace Dario.Core.Esri
{
    public class MultiPoint: Geometry
    {
        public List<Point> Points { get; set; }
    }
}
