using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LED : MonoBehaviour {

    private Vector3 pos;
    public Color colour = Color.black;
    private int ledIndex;
    ArtNetSender artNetSend;


    private LightTube tube;
    private Vector3 size = new Vector3(0.05f, 0.05f, 0.05f);
    

    private void Start () {
        this.tube = transform.parent.GetComponent<LightTube>();
        this.pos = transform.position;
        artNetSend = GameObject.Find("ArtNet Controller").GetComponent<ArtNetSender>();
    }

    public void UpdateColour (LightObject[] lightObjects) {
        Color colour = Color.black;

        for (int i = 0; i < lightObjects.Length; i++) {
            colour += lightObjects[i].Colour * Mathf.InverseLerp(lightObjects[i].Collider.radius * lightObjects[i].Collider.radius, 0, Vector3.SqrMagnitude(lightObjects[i].Pos - pos));
        }

        SetColour( colour );

        int red = (int)(colour.r * 255);
        int green = (int)(colour.g * 255);
        int blue = (int)(colour.b * 255);
        artNetSend.SendArtNet(this.ledIndex, (byte)red, (byte)green, (byte)blue);

    }

    private void SetColour (Color colour) {
        this.colour = colour;
    }

    public LED SetIndex(int index)
    {
        this.ledIndex = index;
        return this;
    }

}
