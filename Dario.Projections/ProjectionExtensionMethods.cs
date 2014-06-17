using Dario.Core.Esri;

namespace Dario.Projections
{
    public static class ProjectionExtensionMethods
    {
        public static Geometry ToLatLon(this Geometry geometry)
        {
            Geometry result = null;
            if (geometry is Extent)
            {
                var extent = (Extent) geometry;
                var resultExtent = new Extent();
                var lo = Projections.MercatorToLatLon(extent.xmin, extent.ymin);
                var rb = Projections.MercatorToLatLon(extent.xmax, extent.ymax);
                resultExtent.xmin = (float)lo.Longitude;
                resultExtent.ymin = (float)lo.Latitude;
                resultExtent.xmax = (float)rb.Longitude;
                resultExtent.ymax = (float)rb.Latitude;
                result = resultExtent;
            }
            return result;
        }
    }
}
