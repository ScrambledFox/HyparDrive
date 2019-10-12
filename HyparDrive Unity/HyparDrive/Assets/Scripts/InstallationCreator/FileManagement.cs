using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class FileManagement {

    private static readonly string INSTALLATION_SAVE_FOLDER = Application.dataPath + "/installations/";

    public static void Init () {
        if (!Directory.Exists(INSTALLATION_SAVE_FOLDER)) {
            Debug.Log("Created installation save folder.");
            Directory.CreateDirectory(INSTALLATION_SAVE_FOLDER);
        }
    }

    public static void SaveInstallation ( string fileName ) {
        string filePath = Application.dataPath + "";
    }

}