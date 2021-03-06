﻿using System.Collections.Generic;

namespace Dario.Core.Esri
{
    public class FeatureCollection
    {
        public FeatureCollection()
        {
            fields=new List<Field>();
            features=new List<Feature>();
        }

        public string objectIdFieldName { get; set; }
        public string globalIdFieldName { get; set; }
        public string geometryType { get; set; }
        public SpatialReference spatialReference { get; set; }
        public List<Field> fields { get; set; }
        public List<Feature> features { get; set; }

        public List<Feature> FilterFeatureCollection(Extent ext)
        {
            var filteredFeatures = new List<Feature>();
            foreach (var f in features)
            {
                var envelope = f.GetEnvelope();
                if (envelope.Intersects(ext))
                {
                    filteredFeatures.Add(f);
                }
            }
            return filteredFeatures;
        }

    }
}
