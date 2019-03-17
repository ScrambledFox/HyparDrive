using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    int index;

    Vector3 originalPosition;
    Vector3 inspectPosition = new Vector3(0, 5f, 0);
    float positionSmoothing = 2f;

    SelectableObject selectableObject;

    public Zone zone;

    private LightTube[] tubes;
    private LED[] leds;

    private void Start() {
        selectableObject = GetComponent<SelectableObject>();
        originalPosition = transform.localPosition;

        tubes = new LightTube[transform.childCount];
        for (int i = 0; i < tubes.Length; i++) {
            tubes[i] = transform.GetChild(i).GetComponent<LightTube>().SetIndex(i);
        }
    }

    public Color[] GetLEDData () {
        if (leds == null) return null;

        Color[] data = new Color[leds.Length];

        for (int i = 0; i < leds.Length; i++) {
            data[i] = leds[i].colour;
        }

        return data;
    }

    /// <summary>
    /// Gets all LEDs of this cube.
    /// </summary>
    /// <returns>Returns an array of all LED components in this cube</returns>
    private LED[] GetAllLEDs () {
        LED[] leds = new LED[tubes.Length * INSTALLATION_CONFIG.LEDS_PER_STRIP];

        for (int i = 0; i < tubes.Length; i++) {
            for (int j = 0; j < INSTALLATION_CONFIG.LEDS_PER_STRIP; j++) {
                leds[i + j * tubes.Length] = tubes[i].transform.GetChild(j).GetComponent<LED>();
            }
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

        for (int i = 0; i < tubes.Length; i++) {
            tubes[i].UpdateTextureData();
        }
    }

    public int GetIndex () {
        return index;
    }

    public Cube SetIndex (int index) {
        this.index = index;
        return this;
    }

    /*
    private void Update () {

        Vector3 targetPosition;
        if (selectableObject.Selected) {
            targetPosition = inspectPosition;
        } else {
            targetPosition = originalPosition;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, positionSmoothing * Time.deltaTime);
    }
    */
}