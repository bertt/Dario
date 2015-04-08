﻿using System;
using System.Net.Http;
using System.Web.Http;

namespace Dario.Controllers
{
    public class GeometryServerController:ApiController
    {
        // sample http://localhost:49430/rest/services/geometry/GeometryServer/project?inSR=4326&outSR=102113&geometries={"geometryType":"esriGeometryPoint","geometries":[{"x":-117,"y":34}]} 
        [Route("rest/services/geometry/geometryserver/project")]
        public HttpResponseMessage GetProject(int inSr, int outSr, string geometries)
        {
            // todo return {"geometryType":"esriGeometryPoint","geometries":[{"x":-13024380.422813,"y":4028802.02613441}]}
            throw new NotImplementedException();
        }

        // sample http://localhost:49430/rest/services/geometry/GeometryServer/simplify?sr=4326&geometries={"geometryType":"esriGeometryPolygon","geometries":[{"rings":[[[-117,34],[-115,36],[-115,33],[-117,36],[-117,34]]]}]} 
        [Route("rest/services/geometry/geometryserver/simplify")]
        public HttpResponseMessage GetSimplify(int sr, string geometries)
        {
            // todo return  {"geometryType":"esriGeometryPolygon","geometries":[{"rings":[[[-116.2,34.8000000000001],[-117,34.0000000000001],[-117,36.0000000000001],[-116.2,34.8000000000001]],[[-116.2,34.8000000000001],[-115,36.0000000000001],[-115,33.0000000000001],[-116.2,34.8000000000001]]]}]}
            throw new NotImplementedException();
        }

        // http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/geometry/GeometryServer/buffer?geometries=-117,34&inSR=4326&outSR=4326&bufferSR=102113&distances=1000
        [Route("rest/services/geometry/geometryserver/buffer")]
        public HttpResponseMessage GetBuffer(string geometries, int inSR, int outSR, int bufferSR, double distances)
        {
            // todo return  {"geometries":[{"rings":[[[-117,34.0074470447458],[-116.999435943166,34.0074323503471],[-116.998874112407,34.0073883251278],[-116.998316725011,34.00731514279],[-116.997765980734,34.007213092076],[-116.997224053109,34.0070825756301],[-116.996693080879,34.0069241084112],[-116.996175159547,34.0067383156626],[-116.995672333114,34.0065259304466],[-116.995186586004,34.0062877907545],[-116.994719835241,34.0060248362025],[-116.994273922875,34.0057381043267],[-116.99385060872,34.005428726492],[-116.993451563403,34.0050979234309],[-116.993078361774,34.0047470004296],[-116.992732476688,34.0043773421803],[-116.992415273197,34.0039904073205],[-116.992128003157,34.00358772268],[-116.991871800293,34.0031708772587],[-116.991647675719,34.0027415159589],[-116.991456513953,34.0023013330968],[-116.991299069424,34.0018520657186],[-116.991175963493,34.0013954867475],[-116.991087682002,34.0009333979889],[-116.991034573359,34.0004676230212],[-116.991016847159,34],[-116.991034573359,33.9995323744045],[-116.991087682002,33.9990665917545],[-116.991175963493,33.9986044903268],[-116.991299069424,33.9981478938995],[-116.991456513953,33.9976986045533],[-116.991647675719,33.997258395558],[-116.991871800293,33.9968290043718],[-116.992128003157,33.9964121257822],[-116.992415273197,33.9960094052147],[-116.992732476688,33.9956224322357],[-116.993078361774,33.9952527342763],[-116.993451563403,33.9949017706],[-116.99385060872,33.9945709265407],[-116.994273922875,33.9942615080311],[-116.994719835241,33.9939747364451],[-116.995186586004,33.993711743774],[-116.995672333114,33.9934735681549],[-116.996175159547,33.9932611497706],[-116.996693080879,33.9930753271356],[-116.997224053109,33.9929168337835],[-116.997765980734,33.9927862953696],[-116.998316725011,33.9926842271994],[-116.998874112407,33.9926110321925],[-116.999435943166,33.9925669992909],[-117,33.9925523023179],[-117.000564056834,33.9925669992909],[-117.001125887593,33.9926110321925],[-117.001683274989,33.9926842271994],[-117.002234019266,33.9927862953696],[-117.002775946891,33.9929168337835],[-117.003306919121,33.9930753271356],[-117.003824840453,33.9932611497706],[-117.004327666886,33.9934735681549],[-117.004813413996,33.993711743774],[-117.005280164759,33.9939747364451],[-117.005726077125,33.9942615080311],[-117.00614939128,33.9945709265407],[-117.006548436597,33.9949017706],[-117.006921638226,33.9952527342763],[-117.007267523312,33.9956224322357],[-117.007584726803,33.9960094052147],[-117.007871996843,33.9964121257822],[-117.008128199707,33.9968290043718],[-117.008352324281,33.997258395558],[-117.008543486047,33.9976986045533],[-117.008700930576,33.9981478938995],[-117.008824036507,33.9986044903268],[-117.008912317998,33.9990665917545],[-117.008965426641,33.9995323744045],[-117.008983152841,34],[-117.008965426641,34.0004676230212],[-117.008912317998,34.0009333979889],[-117.008824036507,34.0013954867475],[-117.008700930576,34.0018520657186],[-117.008543486047,34.0023013330968],[-117.008352324281,34.0027415159589],[-117.008128199707,34.0031708772587],[-117.007871996843,34.00358772268],[-117.007584726803,34.0039904073205],[-117.007267523312,34.0043773421803],[-117.006921638226,34.0047470004296],[-117.006548436597,34.0050979234309],[-117.00614939128,34.005428726492],[-117.005726077125,34.0057381043267],[-117.005280164759,34.0060248362025],[-117.004813413996,34.0062877907545],[-117.004327666886,34.0065259304466],[-117.003824840453,34.0067383156626],[-117.003306919121,34.0069241084112],[-117.002775946891,34.0070825756301],[-117.002234019266,34.007213092076],[-117.001683274989,34.00731514279],[-117.001125887593,34.0073883251278],[-117.000564056834,34.0074323503471],[-117,34.0074470447458]]]}]}
            throw new NotImplementedException();
        }

        // todo: add services for Areas and Lengths, Lengths, Relation, Label Points, Auto Complete, Convex Hull, Cut, Densify, Difference, Generalize, Intersect, Offset, Reshape, Trim / Extend, Union   
    }
}