using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CreatorManager : MonoBehaviour {

    public static CreatorManager INSTANCE;

    public static string installationName;

    public GameObject installation;
    public List<Cube> cubes = new List<Cube>();

    public GameObject[] logicObjects;

    public GridManager GridManager { get => gridManager; }
    private GridManager gridManager;

    void Start () {
        if (INSTANCE == null) {
            INSTANCE = this;
        }

        gridManager = FindObjectOfType<GridManager>();

        if (PlayerPrefs.HasKey("LastInstallationFile")) {
            string fileName = PlayerPrefs.GetString("LastInstallationFile");
            fileName = fileName.Substring(fileName.LastIndexOf('/') + 1);
            fileName = fileName.Substring(0, fileName.IndexOf('.'));

            Debug.Log("Sanitised input: " + fileName);

            if (FileManagement.CheckIfFileExists(fileName)) {
                Debug.Log("LastInstallationFile key found, file also exists, loading it now...");

                //Load..
            } else {
                Debug.Log("LastInstallationFile key found, however no file found.");
            }
        }
    }

    // LOGIC METHODS
    public void AddObject ( string name) {
        switch (name) {
            case "Hypar Cube":
                GameObject go = Instantiate(logicObjects[0], installation.transform);
                go.GetComponent<Cube>().SetIndex(cubes.Count);
                cubes.Add(go.GetComponent<Cube>());
                break;
            default:
                break;
        }
    }

    // UI METHODS
    public void ToggleSpawnPanel () {
        UIManager.INSTANCE.ToggleSpawnPanelState();
    }



    public void SetInstallationName ( string fileName ) {
        installationName = fileName;
    }

    public void SaveInstallation ( ) {
        FileManagement.SaveInstallation(installationName, cubes.ToArray());
    }

    public void LoadInInstallation ( InstallationSaveData installationSaveData ) {

    }
}