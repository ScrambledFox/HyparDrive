using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LED : MonoBehaviour {

    private Color colour = Color.black;
    private Material material;

    private int index;

    private Vector3 size = new Vector3(0.05f, 0.05f, 0.05f);

    private void Awake () {
        material = GetComponent<MeshRenderer>().material;
    }

    public void UpdateColour (LightObject[] lightObjects) {
        Color colour = Color.black;

        for (int i = 0; i < lightObjects.Length; i++) {
            if (lightObjects[i] == null) return;
            colour += lightObjects[i].Colour * Mathf.InverseLerp(lightObjects[i].Collider.radius * lightObjects[i].Collider.radius, 0, Vector3.SqrMagnitude(lightObjects[i].Pos - transform.position));
        }

        SetColour( colour );

        ArtNetController.INSTANCE.SendArtNet(index, (byte)(colour.r * 255), (byte)(colour.g * 255), (byte)(colour.b * 255));

    }

    private void SetColour (Color colour) {
        this.colour = colour;

        UpdateMaterialColour(colour);
    }

    public Color GetColour () {
        return this.colour;
    }

    private void UpdateMaterialColour ( Color colour ) {
        material.color = colour;
    }

    public void SetIndex ( int index ) {
        this.index = index;
    }

}
