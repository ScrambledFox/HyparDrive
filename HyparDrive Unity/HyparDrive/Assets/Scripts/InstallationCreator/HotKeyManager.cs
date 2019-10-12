using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotKeyManager : MonoBehaviour {

    /// <summary>
    /// Holds a list of available hot keys.
    /// </summary>
    private void Update () {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            CreatorManager.INSTANCE.ToggleSpawnPanel();
        }

    }

}
