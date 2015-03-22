﻿
using ActionStreetMap.Core.Scene.Buildings;
using ActionStreetMap.Explorer.Geometry;

namespace ActionStreetMap.Explorer.Scene.Buildings.Facades
{
    /// <summary> Defines facade builder logic. </summary>
    public interface IFacadeBuilder
    {
        /// <summary> Name of facade builder. </summary>
        string Name { get; }

        /// <summary> Builds MeshData which contains information how to construct facade. </summary>
        /// <param name="building">Building.</param>
        /// <returns>MeshData.</returns>
        MeshData Build(Building building);
    }
}
