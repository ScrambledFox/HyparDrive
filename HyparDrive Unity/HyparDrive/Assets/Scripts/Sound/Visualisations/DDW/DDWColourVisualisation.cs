using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DDWColourVisualisation : MonoBehaviour {

    private RawImage image;

    public enum ColorType {
        PRIMARY, SECONDARY
    }

    public ColorType colourType;

    private void Start () {
        image = this.GetComponent<RawImage>();
    }

    void Update() {
        switch (colourType) {
            case ColorType.PRIMARY:
                image.color = INSTALLATION_CONFIG.PRIMARY_COLOUR;
                break;
            case ColorType.SECONDARY:
                image.color = INSTALLATION_CONFIG.SECONDARY_COLOUR;
                break;
            default:
                break;
        }
    }
}
