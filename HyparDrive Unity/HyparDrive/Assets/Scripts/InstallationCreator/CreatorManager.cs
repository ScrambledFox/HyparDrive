using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorManager : MonoBehaviour {

    public static CreatorManager INSTANCE;

    public GameObject installation;

    public GameObject[] logicObjects;

    public bool gridActive = true;
    public int gridSize = 10;
    [Range(0.1f, 10f)]
    public float cellSize = 2f;

    void Start () {
        if (INSTANCE == null) {
            INSTANCE = this;
        }
    }

    // LOGIC METHODS
    public void AddObject ( string name) {
        switch (name) {
            case "Hypar Cube":
                Instantiate(logicObjects[0], installation.transform);
                break;
            default:
                break;
        }
    }

    // UI METHODS
    public void ToggleSpawnPanel () {
        UIManager.INSTANCE.ToggleSpawnPanelState();
    }


    // DEBUG GIZMOS
    private void OnDrawGizmos () {

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize, 1, gridSize));

        for (int y = (int)(-gridSize / cellSize / 2); y <= (int)(gridSize / cellSize / 2); y++) {
            for (int x = (int)(-gridSize / cellSize / 2); x <= (int)(gridSize / cellSize / 2); x++) {
                for (int z = 0; z < (int)(gridSize / cellSize); z++) {
                    Gizmos.color = Color.gray;
                    Gizmos.DrawCube(new Vector3(x * cellSize,
                                                z * cellSize,
                                                y * cellSize),
                                    Vector3.one * cellSize / 4);
                }
            }
        }
    }
}
