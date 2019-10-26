using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotKeyManager : MonoBehaviour {

    /// <summary>
    /// Holds a list of available hot keys.
    /// </summary>
    private void Update () {

        if (Input.GetKeyDown(KeyCode.K)) {
            Camera.main.GetComponent<OrbitalCamera>().ToggleAuto();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            UIManager.INSTANCE.SetSpawnPanelState(true);
        }

    }

}
