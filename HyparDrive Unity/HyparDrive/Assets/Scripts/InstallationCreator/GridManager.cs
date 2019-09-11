using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    public bool gridActive;

    public Vector3 gridSize = new Vector3(10f, 10f, 10f);
    [Range(0.1f, 10f)]
    public float cellSize = 1f;


    // DEBUG GIZMOS
    private void OnDrawGizmos () {
        if (!gridActive) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, gridSize.y / 2, 0), new Vector3(gridSize.x, gridSize.y, gridSize.z));

        for (int x = (int)(-gridSize.x / cellSize / 2); x <= (int)(gridSize.x / cellSize / 2); x++) {
            //for (int y = 0; y <= gridSize.y / cellSize; y++) {
                for (int z = (int)(-gridSize.z / cellSize / 2); z <= (int)(gridSize.z / cellSize / 2); z++) {
                    Gizmos.color = Color.gray;
                    Gizmos.DrawCube(new Vector3(x * cellSize,
                                                0,//y * cellSize,
                                                z * cellSize),
                                    Vector3.one * cellSize / 4);
                }
            //}
        }
    }
}
