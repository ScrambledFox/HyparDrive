using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CubeTest : MonoBehaviour {

    private Mesh mesh;

    void Start() {

        mesh = CubeGenerator.GetCubeMesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void OnValidate () {
        mesh = CubeGenerator.GetCubeMesh();
    }

    private void OnDrawGizmosSelected () {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(Vector3.zero, Vector3.one * 0.5f);

        foreach (Vector3 vertex in mesh.vertices) {
            Gizmos.DrawSphere(vertex, 0.005f);
        }
    }
}
