using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    int index;

    Vector3 originalPosition;
    Vector3 inspectPosition = new Vector3(0, 5f, 0);
    float positionSmoothing = 2f;

    MovableObject selectableObject;

    List<Zone> zones = new List<Zone>();
    List<LightObject> lightObjects = new List<LightObject>();

    /// <summary>
    /// Array with all of the LEDs inside of this cube.
    /// </summary>
    [SerializeField]
    private LED[] leds;

    private void Start() {
        selectableObject = GetComponent<MovableObject>();
        originalPosition = transform.localPosition;

        GetAllLEDs();
    }

    /// <summary>
    /// Get the LED Data of all the LEDs inside a cube.
    /// </summary>
    /// <returns>LED Data Colour Array</returns>
    public Color[] GetLEDData () {
        if (leds == null) return null;

        Color[] data = new Color[leds.Length];

        for (int i = 0; i < leds.Length; i++) {
            data[i] = leds[i].GetColour();
        }

        return data;
    }

    /// <summary>
    /// Gets all LEDs of this cube. Only use this if the LED array is null or incorrect.
    /// </summary>
    /// <returns>Returns an array of all LED components in this cube</returns>
    private LED[] GetAllLEDs () {
        for (int l = 0; l < 120; l++) {
            leds[l].SetIndex(index * 120 + l);

            if (l < 10 || l > 109) {
                leds[l].SetFucked();
            }
        }

        return leds;
    }

    /// <summary>
    /// Updates the LEDs of this cube.
    /// </summary>
    public void UpdateLEDs () {
        for (int i = 0; i < leds.Length; i++) {
            leds[i].UpdateColour(lightObjects.ToArray());
        }
    }

    public void UpdateLightObjects () {
        lightObjects.Clear();

        foreach (Zone zone in zones) {
            foreach (LightObject lightObject in zone.GetLightObjects()) {
                if (!lightObjects.Contains(lightObject)) {
                    lightObjects.Add(lightObject);
                }
            }
        }
    }

    public void RemoveLightObjects (List<LightObject> lightObjectsToRemove) {
        lightObjects.RemoveAll(l => lightObjectsToRemove.Contains(l));
    }

    public void RemoveLightObject ( LightObject lightObjectToRemove ) {
        lightObjects.Remove(lightObjectToRemove);
    }

    public int GetIndex () {
        return index;
    }

    public Cube SetIndex (int index) {
        this.index = index;
        return this;
    }

    public void AddZone (Zone zone) {
        zones.Add(zone);
    }
}