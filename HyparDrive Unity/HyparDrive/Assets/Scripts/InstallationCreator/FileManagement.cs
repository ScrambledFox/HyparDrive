using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class FileManagement {

    public static readonly string INSTALLATION_SAVE_FOLDER = Application.persistentDataPath + "/installations/";
    public static readonly string FILE_EXTENSION = ".hype";

    public static bool CheckIfDirectoryExists () {
        if (!Directory.Exists(INSTALLATION_SAVE_FOLDER)) {
            Debug.Log("Created installation save folder at " + INSTALLATION_SAVE_FOLDER);
            Directory.CreateDirectory(INSTALLATION_SAVE_FOLDER);
        }

        return true;
    }

    public static bool CheckIfFileExists ( string fileName ) {
        CheckIfDirectoryExists();


        /// THERE IS A FILE.EXISTS METHOD.... SO DO A REWRITE!

        if (Directory.GetFiles(INSTALLATION_SAVE_FOLDER, fileName, SearchOption.AllDirectories).Length > 0) {
            return true;
        } else {
            return false;
        }
        
    }

    public static int GetInstallationFileCount () {
        return Directory.GetFiles(INSTALLATION_SAVE_FOLDER).Length;
    }

    public static string[] LoadInstallationFiles (  ) {
        CheckIfDirectoryExists();
        return Directory.GetFiles(INSTALLATION_SAVE_FOLDER);
    }

    public static void SaveInstallation ( string fileName, Cube[] cubes ) {

        CheckIfDirectoryExists();

        InstallationSaveData installationSaveState = new InstallationSaveData();
        installationSaveState.lastSaveTime = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();

        installationSaveState.hyparCubes = new InstallationSaveData.HyparCube[cubes.Length];
        for (int i = 0; i < installationSaveState.hyparCubes.Length; i++) {
            installationSaveState.hyparCubes[i] = new InstallationSaveData.HyparCube(
                cubes[i].GetIndex(),
                cubes[i].gameObject.transform.position,
                cubes[i].gameObject.transform.rotation,
                cubes[i].gameObject.transform.localScale
                );
        }

        string jsonData = JsonUtility.ToJson( installationSaveState , true );
        File.WriteAllText(INSTALLATION_SAVE_FOLDER + fileName + FILE_EXTENSION, jsonData);

        PlayerPrefs.SetString("LastInstallationFile", INSTALLATION_SAVE_FOLDER + fileName + FILE_EXTENSION);

        Debug.Log("Saved " + fileName + " to " + INSTALLATION_SAVE_FOLDER + fileName + FILE_EXTENSION);

    }

}

public class InstallationSaveData {

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