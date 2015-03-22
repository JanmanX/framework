﻿using System.Collections.Generic;
using System.Linq;
using ActionStreetMap.Core.Scene.Buildings;
using ActionStreetMap.Explorer.Geometry;
using ActionStreetMap.Explorer.Geometry.Polygons;
using ActionStreetMap.Explorer.Infrastructure;
using UnityEngine;

namespace ActionStreetMap.Explorer.Scene.Buildings.Roofs
{
    /// <summary> Builds hipped roof. </summary>
    public class HippedRoofBuilder: IRoofBuilder
    {
        /// <inheritdoc />
        public virtual string Name { get { return "hipped"; } }

        /// <inheritdoc />
        public virtual bool CanBuild(Building building) { return true; }

        /// <inheritdoc />
        public virtual MeshData Build(Building building)
        {
            var roofOffset = building.Elevation + building.MinHeight + building.Height;

            var skeleton = StraightSkeleton.Calculate(building.Footprint);
            
            var skeletVertices = skeleton.Item1;
            skeletVertices.Reverse();

            var vertices = new List<Vector3>(skeletVertices.Count);
            var triangles = new List<int>(skeletVertices.Count);
            var colors = new List<Color>(skeletVertices.Count);

            var color = building.RoofColor.ToUnityColor();
            for (int i = 0; i < skeletVertices.Count; i++)
            {
                var vertex = skeletVertices[i];
                var y = skeleton.Item2.Any(t => vertex == t) ? building.RoofHeight + roofOffset : roofOffset;
                vertices.Add(new Vector3(vertex.x, y, vertex.y));
                triangles.Add(i);
                colors.Add(color);
            }
           
            return new MeshData()
            {
                Vertices = vertices,
                Triangles = triangles,
                Colors = colors,
                MaterialKey = building.RoofMaterial,
            };
        }
    }
}
