using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallationSaveFile : MonoBehaviour {

    string filepath;
    InstallationSaveData saveData;

    public InstallationSaveData GetSaveDataFromFileManagement () {
        if (filepath == null) {
            Debug.LogError("filepath of this save not set yet.");
            return null;
        }

        return FileManagement.GetInstallationSaveData(filepath);
    }

    public void SetSaveData (string filepath) {
        this.filepath = filepath;
        this.saveData = GetSaveDataFromFileManagement();
    }

    public void LoadThisSave () {
        if (saveData == null) {
            Debug.LogError("Save Data of this save not set yet.");
            return;
        }

        UIManager.INSTANCE.CloseAllElements();
        CreatorManager.INSTANCE.LoadInstallation( saveData );
    }

}