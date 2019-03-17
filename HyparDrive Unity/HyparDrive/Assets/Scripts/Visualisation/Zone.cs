using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {

    public int x, y, z;
    bool active = false;

    Cube[] cubes;

    private void Awake () {
        cubes = FindMyCubes();
        if (cubes.Length <= 0) {
            Destroy(gameObject);
        } else {
            InstallationManager.INSTANCE.HandleLightObject += HandleLightObjectChange;
        }
    }

    private void HandleLightObjectChange ( LightObject lo, bool removed ) {
        if (removed) {
            lo.Moved -= CheckLightObjectPosition;
        } else {
            lo.Moved += CheckLightObjectPosition;
        }
    }

    /// <summary>
    /// Checks if the given light object touches this zone.
    /// </summary>
    /// <param name="lo">LightObject</param>
    /// <param name="pos">Position of the LO</param>
    /// <param name="radius">Radius of the LO</param>
    private void CheckLightObjectPosition (LightObject lo) {
        if (Vector3.SqrMagnitude(lo.Pos - transform.position) < lo.Radius * lo.Radius) {
            for (int i = 0; i < cubes.Length; i++) {
                lo.Moved -= cubes[i].UpdateLEDs;
                lo.Moved += cubes[i].UpdateLEDs;
            }

            SetActive(true);
        } else {
            for (int i = 0; i < cubes.Length; i++) {
                lo.Moved -= cubes[i].UpdateLEDs;
            }

            SetActive(false);
        }
    }

    /// <summary>
    /// Finds all of the cubes of this zone.
    /// </summary>
    /// <returns>Returns an array of cubes that lie in this zone.</returns>
    private Cube[] FindMyCubes () {
        return InstallationManager.INSTANCE.GetCubesInArea(transform.position, transform.localScale);
    }

    /// <summary>
    /// Sets the state of the zone.
    /// </summary>
    /// <param name="active">Zone state.</param>
    public void SetActive ( bool active ) {
        this.active = active;
    }

    private void OnDrawGizmos () {
        if (active) {
            Gizmos.color = new Color(0, 1, 0, 0.25f);
            Gizmos.DrawCube(transform.position, transform.localScale);
        } else {
            Gizmos.color = new Color(1, 1, 1, 1);
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }

}