﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CreatorManager : MonoBehaviour {

    public static CreatorManager INSTANCE;

    private static string currentFileName;
    private static string currentFilePath;

    public GameObject installation;
    private List<Cube> cubes = new List<Cube>();

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

            if (FileManagement.CheckIfFileExists(fileName)) {
                Debug.Log("LastInstallationFile key found: " + PlayerPrefs.GetString("LastInstallationFile") + ", file also exists: " + fileName + FileManagement.FILE_EXTENSION + ", loading it now...");

                UIManager.INSTANCE.CloseAllElements();
                CreatorManager.INSTANCE.LoadInstallation(FileManagement.GetInstallationSaveData(FileManagement.INSTALLATION_SAVE_FOLDER + fileName + FileManagement.FILE_EXTENSION));
                //TODO: Load startup file
            } else {
                Debug.Log("LastInstallationFile key found: " + PlayerPrefs.GetString("LastInstallationFile") + ", however no file with name " + fileName + " found. Is the file extension " + FileManagement.FILE_EXTENSION + " correctly set?");
                Debug.Log("Starting new project...");
            }
        } else {
            Debug.Log("No last file key found, starting new project...");
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

    public void RemoveCube (Cube cube) {
        Destroy(cube.gameObject);
        cubes.Remove(cube);
    }

    // UI METHODS
    public void ToggleSpawnPanel () {
        UIManager.INSTANCE.ToggleSpawnPanelState();
    }



    public void SetInstallationName ( string fileName ) {
        if (fileName.Equals("")) {
            fileName = "unnamed_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString();
        }

        currentFileName = fileName;
    }

    private void ResetManager () {
        foreach (Cube cube in cubes) {
            Destroy(cube.gameObject);
        }

        cubes.Clear();
    }

    public void SaveInstallation ( ) {
        FileManagement.SaveInstallation(currentFileName, cubes.ToArray());
    }

    public void LoadInstallation ( InstallationSaveData installationSaveData ) {
        ResetManager();

        foreach (InstallationSaveData.HyparCube hyparCube in installationSaveData.hyparCubes) {
            GameObject cube = Instantiate(logicObjects[0], installation.transform);
            cube.GetComponent<Cube>().SetIndex(cubes.Count);
            cubes.Add(cube.GetComponent<Cube>());

            cube.transform.position = hyparCube.position;
            cube.transform.rotation = Quaternion.Euler(hyparCube.rotation);
            cube.transform.localScale = hyparCube.scale;
            cube.transform.GetComponent<Cube>().SetIndex(hyparCube.id);
        }
    }
}