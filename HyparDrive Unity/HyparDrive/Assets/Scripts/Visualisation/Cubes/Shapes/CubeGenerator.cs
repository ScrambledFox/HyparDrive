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

    public static Mesh GetCubeMesh () {
        if (hyparCube != null) {
            return hyparCube;
        } else {
            return hyparCube = GenerateCubeMesh( 0.5f, 0.025f);
        }
    }

    private static Mesh GenerateCubeMesh ( float cubeSize, float borderSize ) {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        GenerateStructure( cubeSize, borderSize, vertices, triangles );

        return MeshGenerator.GenerateMesh(new MeshData(vertices, triangles));
    }

    private static void GenerateStructure ( float cubeSize, float borderSize, List<Vector3> vertices, List<int> triangles ) {

        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                vertices.Add(new Vector3( cubeSize / 2 * cornerPoints[i].x + borderSize / 2 * cornerPoints[j].x - borderSize / 2 * cornerPoints[i].x,
                                          cubeSize / 2 * cornerPoints[i].y + borderSize / 2 * cornerPoints[j].y - borderSize / 2 * cornerPoints[i].y,
                                          cubeSize / 2 * cornerPoints[i].z + borderSize / 2 * cornerPoints[j].z - borderSize / 2 * cornerPoints[i].z)
                                         );
            }
        }


        int triangleVertexIndex = 0;

        for (int i = 0; i < 4; i++) {
            triangles.Add(0 + i * 8);
            triangles.Add(1 + i * 8);
            triangles.Add(2 + i * 8);

            triangles.Add(0 + i * 8);
            triangles.Add(2 + i * 8);
            triangles.Add(3 + i * 8);

            triangles.Add(2 + 4 + i * 8 + 32);
            triangles.Add(1 + 4 + i * 8 + 32);
            triangles.Add(0 + 4 + i * 8 + 32);

            triangles.Add(3 + 4 + i * 8 + 32);
            triangles.Add(2 + 4 + i * 8 + 32);
            triangles.Add(0 + 4 + i * 8 + 32);
        }

    }

}