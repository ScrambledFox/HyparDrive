using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorManager : MonoBehaviour {

    public static CreatorManager INSTANCE;

    public GameObject installation;

    public GameObject[] logicObjects;

    public int gridSize = 10;
    [Range(0.01f, 10f)]
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

        for (int y = 0; y < gridSize / cellSize; y++) {
            for (int x = 0; x < gridSize / cellSize; x++) {
                Gizmos.color = Color.gray;
                Gizmos.DrawCube(new Vector3(x * cellSize - gridSize / 2f + cellSize / 2f, 0, y * cellSize - gridSize / 2f + cellSize / 2f), Vector3.one * cellSize / 4);
            }
        }
    }
}
