using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LED : MonoBehaviour {

    public int index;
    private Vector3 pos;
    public Color Colour => tube.GetLEDColour(index);

    private LightTube tube;
    private Vector3 size = new Vector3(0.05f, 0.05f, 0.05f);

    private void Start () {
        this.tube = transform.parent.GetComponent<LightTube>();
        this.pos = transform.position;
    }

    public LED SetIndex (int index) {
        this.index = index;

        return this;
    }

    public void UpdateColour (LightObject lo) {
        float lightLevel = Mathf.InverseLerp(lo.Radius * lo.Radius, 0, Vector3.SqrMagnitude(lo.Pos - pos));

        SetColour(new Color(lo.Colour.r * lightLevel, lo.Colour.g * lightLevel, lo.Colour.b * lightLevel));
    }

    private void SetColour (Color colour) {
        tube.SetLEDColour(index, colour);
    }

}
