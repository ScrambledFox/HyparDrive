using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CubeTest : MonoBehaviour {

    private Mesh mesh;

    public Vector3 size = Vector3.one * 0.5f;

    void Start() {
        mesh = CubeGenerator.GetCubeMesh(size);
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void OnValidate () {
        mesh = CubeGenerator.GetCubeMesh(size);
    }

    private void OnDrawGizmosSelected () {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(Vector3.zero, size);

        foreach (Vector3 vertex in mesh.vertices) {
            Gizmos.DrawSphere(vertex, 0.005f);
        }
    }
}
