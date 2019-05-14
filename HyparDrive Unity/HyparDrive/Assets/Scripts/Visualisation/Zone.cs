using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {

    new Collision.AABB collider;
    bool active = false;

    public bool Active {
        get { return active; }
    }

    private List<LightObject> lightObjectsInZone = new List<LightObject>();

    Cube[] cubes;

    private void Awake () {
        cubes = FindCubesWithinCollider();
        if (cubes.Length <= 0) {
            Destroy(gameObject);
        } else {
            InstallationManager.INSTANCE.HandleLightObject += HandleLightObjectRegistrationChange;
        }

        collider = new Collision.AABB(transform.position, transform.localScale);

        // Wait one second to initialise the cubes for load reasons.

        Invoke("InitCubes", 1f);
    }

    /// <summary>
    /// Initialize the LEDs.
    /// </summary>
    private void InitCubes () {
        for (int i = 0; i < cubes.Length; i++) {
            cubes[i].SetZone(this);
            cubes[i].UpdateLEDs();
        }
    }

    /// <summary>
    /// Get the active light objects affecting this zone.
    /// </summary>
    /// <returns>Returns an array of Light Objects.</returns>
    public LightObject[] GetLightObjects () {
        return lightObjectsInZone.ToArray();
    }

    /// <summary>
    /// Handle a new light object.
    /// </summary>
    /// <param name="lo">The light object to subscribe to.</param>
    /// <param name="removed">Is the light object removed?</param>
    private void HandleLightObjectRegistrationChange ( LightObject lo, bool removed ) {
        if (removed) {
            NotifyCubes(lo);
            lo.Moved -= CheckLightObjectPosition;
        } else {
            NotifyCubes(lo);
            lo.Moved += CheckLightObjectPosition;
        }
    }

    /// <summary>
    /// Manually notify cubes in this zone of an update.
    /// </summary>
    public void NotifyCubes (LightObject lo) {
        for (int i = 0; i < cubes.Length; i++) {
            cubes[i].UpdateLEDs();
        }
    }

    /// <summary>
    /// Checks if the given light object touches this zone.
    /// </summary>
    /// <param name="lo">LightObject</param>
    /// <param name="pos">Position of the LO</param>
    /// <param name="radius">Radius of the LO</param>
    private void CheckLightObjectPosition (LightObject lo) {
        if (Collision.HasIntersection(collider, lo.Collider)) {
            if (!lightObjectsInZone.Contains(lo)) {
                lightObjectsInZone.Add(lo);
                lo.Moved += NotifyCubes;
            }
            if (!active) SetActive(true);
        } else {
            lightObjectsInZone.Remove(lo);
            NotifyCubes(lo);
            lo.Moved -= NotifyCubes;
            if (lightObjectsInZone.Count == 0) SetActive(false);
        }
    }

    /// <summary>
    /// Finds all of the cubes of this zone.
    /// </summary>
    /// <returns>Returns an array of cubes that lie in this zone.</returns>
    private Cube[] FindCubesWithinCollider () {
        return InstallationManager.INSTANCE.GetCubesInArea(transform.position, transform.localScale);
    }

    /// <summary>
    /// Sets the state of the zone.
    /// </summary>
    /// <param name="active">Zone state.</param>
    public void SetActive ( bool active ) {
        InstallationManager.INSTANCE.RegisterActiveZone(this, active);
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