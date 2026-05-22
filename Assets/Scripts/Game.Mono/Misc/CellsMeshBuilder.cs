using System;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Mono;

public static class CellsMeshBuilder
{
    public static readonly Vector2[] QuadUVs =
    {
        new(0, 0),
        new(1, 0),
        new(1, 1),
        new(0, 1)
    };

    public static GameObject CreateCellsPresenterGO(
        float cellWorldSize,
        int2 startPos,
        ReadOnlySpan<int2> offsets,
        ReadOnlySpan<Color> cellColors,
        Material material,
        string gameObjName = "CellsMesh")
    {
        var go = new GameObject(gameObjName);

        var meshFilter = go.AddComponent<MeshFilter>();
        var meshRenderer = go.AddComponent<MeshRenderer>();

        meshRenderer.material = material;
        meshFilter.mesh = BuildCellsMesh(cellWorldSize, startPos, offsets, cellColors);

        return go;
    }

    public static Mesh BuildCellsMesh(
        float cellWorldSize,
        int2 startPos,
        ReadOnlySpan<int2> offsets,
        ReadOnlySpan<Color> cellColors)
    {
        int cellCount = offsets.Length;
        bool useSingleColor = cellColors.Length == 1;

        Vector3[] vertices = new Vector3[cellCount * 4];
        int[] triangles = new int[cellCount * 6];
        Vector2[] uvs = new Vector2[cellCount * 4];
        Color[] colors = new Color[cellCount * 4];

        float size = cellWorldSize;

        for (int i = 0; i < cellCount; i++)
        {
            int vIndex = i * 4;
            int tIndex = i * 6;

            var cell = startPos + offsets[i];
            var color = useSingleColor ? cellColors[0] : cellColors[i];

            float x = cell.x * size;
            float y = cell.y * size;

            // Quad vertices
            vertices[vIndex + 0] = new Vector3(x, y, 0);
            vertices[vIndex + 1] = new Vector3(x + size, y, 0);
            vertices[vIndex + 2] = new Vector3(x + size, y + size, 0);
            vertices[vIndex + 3] = new Vector3(x, y + size, 0);

            // Triangles
            triangles[tIndex + 0] = vIndex + 0;
            triangles[tIndex + 1] = vIndex + 2;
            triangles[tIndex + 2] = vIndex + 1;

            triangles[tIndex + 3] = vIndex + 0;
            triangles[tIndex + 4] = vIndex + 3;
            triangles[tIndex + 5] = vIndex + 2;

            // UVs (simple full quad)
            uvs[vIndex + 0] = QuadUVs[0];
            uvs[vIndex + 1] = QuadUVs[1];
            uvs[vIndex + 2] = QuadUVs[2];
            uvs[vIndex + 3] = QuadUVs[3];

            // Colors (same for all 4 vertices = flat color per cell)
            colors[vIndex + 0] = color;
            colors[vIndex + 1] = color;
            colors[vIndex + 2] = color;
            colors[vIndex + 3] = color;
        }

        var mesh = new Mesh
        {
            vertices = vertices,
            triangles = triangles,
            uv = uvs,
            colors = colors
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}