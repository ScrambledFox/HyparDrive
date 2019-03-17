using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LED : MonoBehaviour {

    //public int index;
    private Vector3 pos;
    public Color colour = Color.black;

    private LightTube tube;
    private Vector3 size = new Vector3(0.05f, 0.05f, 0.05f);

    private void Start () {
        this.tube = transform.parent.GetComponent<LightTube>();
        this.pos = transform.position;
    }

    //public LED SetIndex (int index) {
    //    this.index = index;

    //    return this;
    //}

    public void UpdateColour (LightObject[] lightObjects) {
        if (lightObjects == null) return;
        if (lightObjects.Length == 0) return;

        Color colour = Color.black;

        for (int i = 0; i < lightObjects.Length; i++) {
            colour += lightObjects[i].Colour * Mathf.InverseLerp(lightObjects[i].Collider.radius * lightObjects[i].Collider.radius, 0, Vector3.SqrMagnitude(lightObjects[i].Pos - pos));
        }

        SetColour( colour );
    }

    private void SetColour (Color colour) {
        this.colour = colour;
    }

}
