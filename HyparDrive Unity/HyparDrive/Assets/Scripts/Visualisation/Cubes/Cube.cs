using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    int index;

    Vector3 originalPosition;
    Vector3 inspectPosition = new Vector3(0, 5f, 0);
    float positionSmoothing = 2f;

    MovableObject selectableObject;

    /// <summary>
    /// Zone assigned to this cube.
    /// </summary>
    private Zone zone;

    /// <summary>
    /// Array with all of the LEDs inside of this cube.
    /// </summary>
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
        if (leds != null) return leds;

        leds = new LED[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            leds[i] = transform.GetChild(i).GetComponent<LED>();
        }

        return leds;
    }

    /// <summary>
    /// Updates the LEDs of this cube.
    /// </summary>
    public void UpdateLEDs () {
        if (this.zone == null) return;
        LightObject[] los = this.zone.GetLightObjects();

        if (leds == null) leds = GetAllLEDs();
        if (los == null) return;
        if (los.Length == 0) return;

        for (int i = 0; i < leds.Length; i++) {
            leds[i].UpdateColour(los);
        }
    }

    public int GetIndex () {
        return index;
    }

    public Cube SetIndex (int index) {
        this.index = index;
        return this;
    }

    public void SetZone (Zone zone) {
        this.zone = zone;
    }
}