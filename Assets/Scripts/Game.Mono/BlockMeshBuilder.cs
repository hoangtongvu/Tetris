using Unity.Mathematics;
using UnityEngine;

namespace Game.Mono;

public static class BlockMeshBuilder
{
    public static GameObject CreateBlock(
        BoardConfig config,
        int2 position,
        int2[] cellOffsets,
        Material material,
        Transform parent = null)
    {
        GameObject go = new GameObject("BlockMesh");
        if (parent != null)
            go.transform.SetParent(parent, false);

        var meshFilter = go.AddComponent<MeshFilter>();
        var meshRenderer = go.AddComponent<MeshRenderer>();

        meshRenderer.material = material;

        Mesh mesh = BuildMesh(config, position, cellOffsets);
        meshFilter.mesh = mesh;

        return go;
    }

    private static Mesh BuildMesh(
        BoardConfig config,
        int2 position,
        int2[] offsets)
    {
        int cellCount = offsets.Length;

        Vector3[] vertices = new Vector3[cellCount * 4];
        int[] triangles = new int[cellCount * 6];
        Vector2[] uvs = new Vector2[cellCount * 4];

        float size = config.CellWorldSize;

        for (int i = 0; i < cellCount; i++)
        {
            int vIndex = i * 4;
            int tIndex = i * 6;

            var cell = position + offsets[i];

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
            uvs[vIndex + 0] = new Vector2(0, 0);
            uvs[vIndex + 1] = new Vector2(1, 0);
            uvs[vIndex + 2] = new Vector2(1, 1);
            uvs[vIndex + 3] = new Vector2(0, 1);
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}