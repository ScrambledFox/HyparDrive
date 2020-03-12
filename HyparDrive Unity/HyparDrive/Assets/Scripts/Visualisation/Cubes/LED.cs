using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LED : MonoBehaviour {

    private Color colour = Color.black;
    private Material material;

    private int index;

    private Vector3 size = new Vector3(0.05f, 0.05f, 0.05f);

    private void Awake () {
        //material = GetComponent<MeshRenderer>().material;
    }

    public void UpdateColour (LightObject[] lightObjects) {
        /*float r = 0;
        float g = 0;
        float b = 0;

        int count = 0;

        for (int i = 0; i < lightObjects.Length; i++) {
            if (lightObjects[i] == null) return;

            float distance = Mathf.InverseLerp(lightObjects[i].Collider.radius * lightObjects[i].Collider.radius, 0, Vector3.SqrMagnitude(lightObjects[i].Pos - transform.position));

            if (distance > 0.001f)
            {
                r += lightObjects[i].Colour.r * distance;
                g += lightObjects[i].Colour.g * distance;
                b += lightObjects[i].Colour.b * distance;

                count++;
            }

            
        }

        r /= count;
        g /= count;
        b /= count;

        r = Mathf.Min(1.0f, r);
        g = Mathf.Min(1.0f, g);
        b = Mathf.Min(1.0f, b);

        SetColour( new Color(r, g, b) );

        ArtNetController.INSTANCE.SendArtNet(index, (byte)(r * 255), (byte)(g * 255), (byte)(b * 255));*/

        Color colour = Color.black;

        for (int i = 0; i < lightObjects.Length; i++)
        {
            if (lightObjects[i] == null) return;

            float distance = Mathf.InverseLerp(lightObjects[i].Collider.radius * lightObjects[i].Collider.radius, 0, Vector3.SqrMagnitude(lightObjects[i].Pos - transform.position));
            colour += lightObjects[i].Colour * distance;


        }

        SetColour( colour );

        ArtNetController.INSTANCE.SendArtNet(index, (byte)(colour.r * 255), (byte)(colour.g * 255), (byte)(colour.b * 255));
    }

    private void SetColour (Color colour) {
        this.colour = colour;

        if (INSTALLATION_CONFIG.VISUALISE) {
            UpdateMaterialColour(colour);
        }
    }

    public Color GetColour () {
        return this.colour;
    }

    private void UpdateMaterialColour ( Color colour ) {
        //material.color = colour;
    }

    public void SetIndex ( int index ) {
        this.index = index;
    }

}
