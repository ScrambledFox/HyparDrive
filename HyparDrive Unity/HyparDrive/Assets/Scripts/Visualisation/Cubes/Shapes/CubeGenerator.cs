using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CubeGenerator {

    private static Mesh hyparCube;

    private static Vector3[] cornerPoints = new Vector3[8] { new Vector3(-1, -1, -1),
                                                             new Vector3(1, -1, -1),
                                                             new Vector3(1, -1, 1),
                                                             new Vector3(-1, -1, 1),
                                                             new Vector3(-1, 1, -1),
                                                             new Vector3(1, 1, -1),
                                                             new Vector3(1, 1, 1),
                                                             new Vector3(-1, 1, 1)};

    public static Mesh GetCubeMesh ( Vector3 size ) {
        if (hyparCube != null) {
            return hyparCube;
        } else {
            return hyparCube = GenerateCubeMesh( size, 0.025f);
        }
    }

    private static Mesh GenerateCubeMesh ( Vector3 cubeSize, float borderSize ) {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        GenerateStructure( cubeSize, borderSize, vertices, triangles );

        return MeshGenerator.GenerateMesh(new MeshData(vertices, triangles));
    }

    private static void GenerateStructure ( Vector3 cubeSize, float borderSize, List<Vector3> vertices, List<int> triangles ) {
        
        /// Adding all vertices to the mesh
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                vertices.Add(new Vector3( cubeSize.x / 2 * cornerPoints[i].x + borderSize / 2 * cornerPoints[j].x - borderSize / 2 * cornerPoints[i].x,
                                          cubeSize.y / 2 * cornerPoints[i].y + borderSize / 2 * cornerPoints[j].y - borderSize / 2 * cornerPoints[i].y,
                                          cubeSize.z / 2 * cornerPoints[i].z + borderSize / 2 * cornerPoints[j].z - borderSize / 2 * cornerPoints[i].z)
                                         );
            }
        }


        for (int i = 0; i < 4; i++) {
            // Top tris
            triangles.Add(0 + i * 8);
            triangles.Add(1 + i * 8);
            triangles.Add(2 + i * 8);

            triangles.Add(0 + i * 8);
            triangles.Add(2 + i * 8);
            triangles.Add(3 + i * 8);

            // Bottom tris
            triangles.Add(2 + 4 + i * 8 + 32);
            triangles.Add(1 + 4 + i * 8 + 32);
            triangles.Add(0 + 4 + i * 8 + 32);

            triangles.Add(3 + 4 + i * 8 + 32);
            triangles.Add(2 + 4 + i * 8 + 32);
            triangles.Add(0 + 4 + i * 8 + 32);
        }

        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 2; j++) {
                // Bottom rimming ;)
                int c = 0 + i * 8 + 1 * i - j;
                int v = 4 + i * 8 + 1 * i - j;
                int b = 5 + i * 8 + 1 * i - j;

                c = c < 0 ? 0 : c;
                b = b > 31 ? b - 4 : b;
                //Debug.Log("!! " + c + ", " + v + ", " + b + "    [" + i + ", " + j + "]");

                triangles.Add(c);
                triangles.Add(v);
                triangles.Add(b);

                c = 0 + i * 8 + 1 * i - j;
                v = 5 + i * 8 + 1 * i - j;
                b = 1 + i * 8 + 1 * i - j;

                c = c < 0 ? 3 : c;
                v = v <= 4 ? v + 3 : v;
                v = v > 31 ? v - 4 : v;
                b = b == 0 ? b + 4 : b;
                b = b > 27 ? b - 4 : b;
                //Debug.Log("!! " + c + ", " + v + ", " + b + "    [" + i + ", " + j + "]");

                triangles.Add(c);
                triangles.Add(v);
                triangles.Add(b);
            }
        }

        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 2; j++) {
                // Top rimming ;)
                int c = 0 + 32 + i * 8 + 1 * i - j;
                int v = 4 + 32 + i * 8 + 1 * i - j;
                int b = 5 + 32 + i * 8 + 1 * i - j;

                Debug.Log("!1.1! " + c + ", " + v + ", " + b + "    [" + i + ", " + j + "]");
                c = c < 32 ? 32 : c;
                v = v > 63 ? v - 4 : v;
                b = b > 63 ? b - 4 : b;
                Debug.Log("!1.2! " + c + ", " + v + ", " + b + "    [" + i + ", " + j + "]");

                triangles.Add(c);
                triangles.Add(v);
                triangles.Add(b);

                c = 0 + 32 + i * 8 + 1 * i - j;
                v = 5 + 32 + i * 8 + 1 * i - j;
                b = 1 + 32 + i * 8 + 1 * i - j;

                Debug.Log("!2.1! " + c + ", " + v + ", " + b + "    [" + i + ", " + j + "]");
                c = c < 32 ? 32 + 3 : c;
                v = v > 63 ? v - 4 : v;
                b = b > 63 ? b - 4 : b;
                Debug.Log("!2.2! " + c + ", " + v + ", " + b + "    [" + i + ", " + j + "]");

                triangles.Add(c);
                triangles.Add(v);
                triangles.Add(b);
            }
        }

    }

}