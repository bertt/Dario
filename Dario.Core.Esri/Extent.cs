namespace Dario.Core.Esri
{
    public class Extent:Geometry
    {
        public float xmin { get; set; }
        public float ymin { get; set; }
        public float xmax { get; set; }
        public float ymax { get; set; }


        public bool Intersects(Extent o)
        {
            if (xmin > o.xmax) return false;
            if (xmax < o.xmin) return false;
            if (ymin > o.ymax) return false;
            if (ymax < o.ymin) return false;
            return true;

        }
 

    }
}
