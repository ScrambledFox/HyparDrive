using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{

    public static Mesh GenerateMesh ( MeshData data ) {
        Mesh mesh = new Mesh();

        mesh.vertices = data.vertices;
        mesh.triangles = data.triangles;
        mesh.uv = data.uv;

        if (data.hasNormals) mesh.normals = data.normals;
        else mesh.RecalculateNormals();

        return mesh;
    }

}

[System.Serializable]
public class MeshData
{

    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uv;

    public bool hasNormals;
    public Vector3[] normals;

    public MeshData ( List<Vector3> vertices, List<int> triangles, List<Vector2> uv, List<Vector3> normals ) {
        this.vertices = vertices.ToArray();
        this.triangles = triangles.ToArray();

        if (uv != null) this.uv = uv.ToArray();

        if (normals == null) {
            this.hasNormals = false;
            this.normals = null;
        } else {
            this.hasNormals = true;
            this.normals = normals.ToArray();
        }
    }

    public MeshData ( List<Vector3> vertices, List<int> triangles, List<Vector2> uv ) : this(vertices, triangles, uv, null) { }

    public MeshData ( List<Vector3> vertices, List<int> triangles ) : this(vertices, triangles, null, null) { }

}