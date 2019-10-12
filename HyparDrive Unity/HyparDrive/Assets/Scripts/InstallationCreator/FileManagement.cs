using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class FileManagement {

    private static readonly string INSTALLATION_SAVE_FOLDER = Application.persistentDataPath + "/installations/";
    private static readonly string FILE_EXTENSION = ".hype";

    public static void Init () {
        if (!Directory.Exists(INSTALLATION_SAVE_FOLDER)) {
            Debug.Log("Created installation save folder.");
            Directory.CreateDirectory(INSTALLATION_SAVE_FOLDER);
        }
    }

    public static void SaveInstallation ( string fileName, GameObject[] cubes ) {

        Init();

        InstallationSaveState installationSaveState = new InstallationSaveState();
        installationSaveState.lastSaveTime = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();

        installationSaveState.hyparCubes = new InstallationSaveState.HyparCube[cubes.Length];
        for (int i = 0; i < installationSaveState.hyparCubes.Length; i++) {
            installationSaveState.hyparCubes[i] = new InstallationSaveState.HyparCube(
                0,
                cubes[i].gameObject.transform.position,
                cubes[i].gameObject.transform.rotation,
                cubes[i].gameObject.transform.localScale
                );
        }

        string jsonData = JsonUtility.ToJson( installationSaveState , true );
        File.WriteAllText(INSTALLATION_SAVE_FOLDER + fileName + FILE_EXTENSION, jsonData);

        Debug.Log("Saved " + fileName + " to " + INSTALLATION_SAVE_FOLDER + fileName + FILE_EXTENSION);

    }

}

public class InstallationSaveState {

    public string lastSaveTime;
    public HyparCube[] hyparCubes;

    [Serializable]
    public class HyparCube {
        public int id;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public HyparCube ( int id, Vector3 position, Quaternion rotation, Vector3 scale ) {
            this.id = id;
            this.position = position;
            this.rotation = rotation.eulerAngles;
            this.scale = scale;
        }
    }

}