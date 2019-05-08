using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PentagonTubeGeneration {

    private static Mesh tubeMiddle;
    private static Mesh tubeBegin;
    private static Mesh tubeEnd;

    private static List<Vector3> vertices;
    private static List<int> triangles;

    /// <summary>
    /// Generates a light tube mesh.
    /// </summary>
    /// <param name="tubeType">0: middle, 1: begin, 2: end</param>
    /// <returns></returns>
    public static Mesh GetTube ( int tubeType ) {

        if (tubeType == 0) {
            if (tubeMiddle != null) {
                return tubeMiddle;
            } else {
                return tubeMiddle = GenerateTube(tubeType);
            }
        } else if (tubeType == 1) {
            if (tubeBegin != null) {
                return tubeBegin;
            } else {
                return tubeBegin = GenerateTube(tubeType);
            }
        } else if (tubeType == 2) {
            if (tubeEnd != null) {
                return tubeEnd;
            } else {
                return tubeEnd = GenerateTube(tubeType);
            }
        } else {
            Debug.LogError("TubeType not known, or some other error occured!");
            return null;
        }

    }

    private static Mesh GenerateTube ( int tubeType ) {

        vertices = new List<Vector3>();
        triangles = new List<int>();

        AddTubeVertices( 10f, 10 );
        AddTubeTriangles( 10f, 10, false );

        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();   
        mesh.RecalculateNormals();

        return mesh;
    }

    private static void AddPentagonVertices ( float depth ) {
        float step = 2 * Mathf.PI / 5;
        float offset = 0.30f;

        vertices.Add(new Vector3(Mathf.Cos(offset), Mathf.Sin(offset), depth));
        vertices.Add(new Vector3(Mathf.Cos(step + offset), Mathf.Sin(step + offset), depth));
        vertices.Add(new Vector3(Mathf.Cos(2 * step + offset), Mathf.Sin(2 * step + offset), depth));
        vertices.Add(new Vector3(Mathf.Cos(3 * step + offset), Mathf.Sin(3 * step + offset), depth));
        vertices.Add(new Vector3(Mathf.Cos(4 * step + offset), Mathf.Sin(4 * step + offset), depth));
    }

    private static void AddPentagonTriangles (   ) {
        triangles.Add(0);
        triangles.Add(3);
        triangles.Add(4);

        triangles.Add(0);
        triangles.Add(2);
        triangles.Add(3);

        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);
    }

    private static void AddTubeVertices ( float length, int segments ) {
        for (int i = 0; i < segments - 1; i++) {
            AddPentagonVertices(length / (2 + segments - i));
        }
    }

    private static void AddTubeTriangles ( float length, int segments, bool hasBegin = false) {
        int index = 0;

        if (hasBegin) {
            index = 5;
        }

        for (int i = 0; i < segments; i++) {
            triangles.Add(index);
            triangles.Add(index + 6);
            triangles.Add(index + 1);

            triangles.Add(index);
            triangles.Add(index + 5);
            triangles.Add(index + 6);

            triangles.Add(index + 1);
            triangles.Add(index + 7);
            triangles.Add(index + 6);

            triangles.Add(index + 1);
            triangles.Add(index + 1);
        }

    }

}