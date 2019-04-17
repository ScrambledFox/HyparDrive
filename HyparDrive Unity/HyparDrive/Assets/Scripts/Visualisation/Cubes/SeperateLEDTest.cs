using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeperateLEDTest : MonoBehaviour {

    List<SeperateLED> seperateLEDs = new List<SeperateLED>();

    private void Start () {
        GetAllLEDs();
    }

    private void Update () {
        if (Input.GetKeyDown(KeyCode.R)) {
            seperateLEDs.ForEach(led => led.RandomizeColour());
        }
    }

    void GetAllLEDs () {
        for (int i = 0; i < transform.GetChild(0).childCount; i++) {
            seperateLEDs.Add(new SeperateLED(transform.GetChild(0).GetChild(i).gameObject, Color.black));
        }

        UpdateLEDColours();
    }

    void UpdateLEDColours () {
        seperateLEDs.ForEach(led => led.Update());
    }

}

[System.Serializable]
struct SeperateLED {

    public GameObject gameObject;
    public Color colour;
    public MeshRenderer renderer;

    public SeperateLED ( GameObject gameObject, Color colour ) {
        this.gameObject = gameObject;
        this.colour = colour;

        this.renderer = gameObject.GetComponent<MeshRenderer>();
    }

    public Color RandomizeColour () {
        this.colour = Random.ColorHSV();
        this.Update();

        return this.colour;
    }

    public void Update () {
        this.renderer.material.color = this.colour;
    }

}