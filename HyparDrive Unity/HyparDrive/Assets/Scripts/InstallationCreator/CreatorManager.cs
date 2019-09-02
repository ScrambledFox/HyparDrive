using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorManager : MonoBehaviour {

    public static CreatorManager INSTANCE;

    public GameObject installation;

    public GameObject[] logicObjects;

    public GridManager GridManager { get => gridManager; }
    private GridManager gridManager;

    void Start () {
        if (INSTANCE == null) {
            INSTANCE = this;
        }

        gridManager = FindObjectOfType<GridManager>();
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
}
