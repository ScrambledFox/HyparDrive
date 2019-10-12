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

        foreach (string fileName in FileManagement.LoadInstallationFiles()) {
            GameObject go = Instantiate(itemPrefab);
            go.transform.SetParent(content.transform, true);
            go.transform.localPosition = Vector3.zero;

            go.transform.GetComponentInChildren<TextMeshProUGUI>().text = fileName;
        }
    }

}
