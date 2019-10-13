using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadInstallationSelectorUI : MonoBehaviour
{

    public GameObject itemPrefab;
    public GameObject content;

    private void OnEnable () {
        if (itemPrefab == null) {
            Debug.LogError("Item Prefab not set.");
            return;
        }

        // Gets all the filepaths of files inside the installation folder.
        foreach (string filepath in FileManagement.LoadInstallationFiles()) {
            if (!filepath.Contains(".hype")) continue;

            GameObject go = Instantiate(itemPrefab);
            go.transform.SetParent(content.transform, true);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            TextMeshProUGUI[] texts = go.transform.GetComponentsInChildren<TextMeshProUGUI>();

            texts[0].text = filepath;
            texts[1].text = FileManagement.GetDateAndTime(filepath);

            go.GetComponent<InstallationSaveFile>().SetSaveData(filepath);
        }
    }

    private void OnDisable () {
        for (int i = content.transform.childCount; i > 0; i--) {
            Destroy(content.transform.GetChild(i - 1).gameObject);
        }
    }

}
