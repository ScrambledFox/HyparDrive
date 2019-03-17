using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    Vector3 originalPosition;
    Vector3 inspectPosition = new Vector3(0, 5f, 0);
    float positionSmoothing = 2f;

    SelectableObject selectableObject;

    private LightTube[] tubes;
    private LED[] leds;

    private void Start () {
        selectableObject = GetComponent<SelectableObject>();
        originalPosition = transform.localPosition;

        tubes = new LightTube[transform.childCount];
        for (int i = 0; i < tubes.Length; i++) {
            tubes[i] = transform.GetChild(i).GetComponent<LightTube>();
        }
    }

    /// <summary>
    /// Gets all LEDs of this cube.
    /// </summary>
    /// <returns>Returns an array of all LED components in this cube</returns>
    private LED[] GetAllLEDs () {
        LED[] leds = new LED[tubes.Length * INSTALLATION_CONFIG.LEDS_PER_STRIP];

        for (int i = 0; i < tubes.Length; i++) {
            for (int j = 0; j < tubes[i].transform.childCount; j++) {
                leds[i + j * tubes.Length] = tubes[i].transform.GetChild(j).GetComponent<LED>();
            }
        }

        return leds;
    }

    /// <summary>
    /// Updates the LEDs of this cube.
    /// </summary>
    /// <param name="lo">The LO to check.</param>
    public void UpdateLEDs (LightObject lo) {
        if (leds == null) leds = GetAllLEDs();

        for (int i = 0; i < leds.Length; i++) {
            leds[i].UpdateColour(lo);
        }
    }

    private void Update () {

        Vector3 targetPosition;
        if (selectableObject.Selected) {
            targetPosition = inspectPosition;
        } else {
            targetPosition = originalPosition;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, positionSmoothing * Time.deltaTime);
    }
}