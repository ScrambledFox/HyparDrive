using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTube : MonoBehaviour {

    private bool reverseLEDIndex = true;

    private LED[] leds;
    private Color[] ledColourData;

    private Material mat;
    private Texture2D tex;

    Vector3 STRIP_START;
    Vector3 STRIP_END;

    private void Start () {
        mat = GetComponent<MeshRenderer>().material;

        STRIP_START = transform.GetChild(0).position;
        STRIP_END = transform.GetChild(1).position;

        Destroy(transform.GetChild(0).gameObject);
        Destroy(transform.GetChild(1).gameObject);

        leds = new LED[INSTALLATION_CONFIG.LEDS_PER_STRIP];
        ledColourData = new Color[INSTALLATION_CONFIG.LEDS_PER_STRIP];

        Vector3[] ledPositions = new Vector3[INSTALLATION_CONFIG.LEDS_PER_STRIP];
        ledPositions = GenerateLEDPositions(INSTALLATION_CONFIG.LEDS_PER_STRIP);

        for (int i = 0; i < INSTALLATION_CONFIG.LEDS_PER_STRIP; i++) {
            leds[i] = CreateNewLED(i, ledPositions[i]);
        }

        GenerateColourTexture();
    }

    private LED CreateNewLED (int index, Vector3 position) {
        GameObject newLED = new GameObject("LED: " + (index + 1));

        newLED.transform.parent = transform;
        newLED.transform.position = position;

        return newLED.AddComponent<LED>().SetIndex(reverseLEDIndex ? INSTALLATION_CONFIG.LEDS_PER_STRIP - index - 1 : index);
    }

    private Vector3[] GenerateLEDPositions (int length) {
        Vector3[] pos = new Vector3[length];

        for (int i = 0; i < length; i++) {
            if (length <= 0) {
                throw new System.Exception("LedPositions cannot be lower or equal to zero!");
            } else if (length == 1) {
                pos[i] = Vector3.Lerp(STRIP_START, STRIP_END, 0.5f);
            } else {
                pos[i] = Vector3.Lerp(STRIP_START, STRIP_END, i * (1f / (length - 1)));
            }
        }

        return pos;
    }

    public Color GetLEDColour ( int index ) {
        return ledColourData[index];
    }

    public void SetLEDColour ( int i, Color colour ) {
        ledColourData[i] = colour;

        EditTexture(i, colour);
    }

    private void GenerateColourTexture () {
        tex = ImageGenerator.GenerateImage(ledColourData, INSTALLATION_CONFIG.LEDS_PER_STRIP, 1);
        mat.mainTexture = tex;
    }

    private void EditTexture (int i, Color colour) {
        this.tex.SetPixel(i, 0, colour);
        this.tex.Apply();
    }

}