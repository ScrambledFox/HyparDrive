using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTube : MonoBehaviour {

    int index;

    private bool reverseLEDIndex = true;

    private Cube cube;

    private LED[] leds;

    private Material mat;

    Vector3 STRIP_START;
    Vector3 STRIP_END;

    private void Start () {
        mat = GetComponent<MeshRenderer>().material;
        cube = transform.parent.GetComponent<Cube>();

        STRIP_START = transform.GetChild(0).position;
        STRIP_END = transform.GetChild(1).position;

        Destroy(transform.GetChild(0).gameObject);
        Destroy(transform.GetChild(1).gameObject);

        leds = new LED[INSTALLATION_CONFIG.LEDS_PER_STRIP];

        Vector3[] ledPositions = new Vector3[INSTALLATION_CONFIG.LEDS_PER_STRIP];
        ledPositions = GenerateLEDPositions(INSTALLATION_CONFIG.LEDS_PER_STRIP);

        for (int i = 0; i < INSTALLATION_CONFIG.LEDS_PER_STRIP; i++) {
            leds[i] = CreateNewLED(i, ledPositions[i]);
        }

        if(InstallationManager.INSTANCE.doVisualisation) UpdateTextureData();
    }

    private LED CreateNewLED ( int index, Vector3 position ) {
        GameObject newLED = new GameObject("LED: " + (index + 1));

        newLED.transform.parent = transform;
        newLED.transform.position = position;

        return newLED.AddComponent<LED>();
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

    public void UpdateTextureData () {
        if (mat.mainTexture == null) {
            Color[] data = new Color[INSTALLATION_CONFIG.LEDS_PER_STRIP];
            for (int i = 0; i < data.Length; i++) {
                data[i] = Color.black;
            }
            mat.mainTexture = ImageGenerator.GenerateImage(data, INSTALLATION_CONFIG.LEDS_PER_STRIP, 1);
        }

        if (InstallationManager.INSTANCE.textureMap == null) return;

        Texture2D tex = (Texture2D)mat.mainTexture;
        for (int i = 0; i < leds.Length; i++) {
            tex.SetPixel(i, 0, InstallationManager.INSTANCE.textureMap.GetPixel(cube.GetIndex(), this.index + i));
        }
        tex.Apply();
        mat.mainTexture = tex;
    }

    public int GetIndex () {
        return index;
    }

    public LightTube SetIndex ( int index ) {
        this.index = index;
        return this;
    }
}