
using System;

namespace Dario.Projections
{
    public class Projections
    {

        public static Point MercatorToLatLon(double mercX, double mercY)
        {
            var rMajor = 6378137; //Equatorial Radius, WGS84     
            var shift = Math.PI*rMajor;
            var lon = mercX/shift*180.0;
            var lat = mercY/shift*180.0;
            lat = 180/Math.PI*(2*Math.Atan(Math.Exp(lat*Math.PI/180.0)) - Math.PI/2.0);
            return new Point {Latitude = lat, Longitude = - lon};
        }

        public static Point LatLonToMercator(double lat, double lon)
        {
            var rMajor = 6378137; //Equatorial Radius, WGS84     
            var shift = Math.PI*rMajor;
            var x = lon*shift/180;
            var y = Math.Log(Math.Tan((90 + lat)*Math.PI/360))/(Math.PI/180);
            y = y*shift/180;
            return new Point {Latitude = x, Longitude = y};
        }
    }


    public class Point
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    };
}
