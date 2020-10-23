using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DDWColourVisualisation : MonoBehaviour {

    private RawImage image;

    private void Start () {
        image = this.GetComponent<RawImage>();
    }

    void Update() {
        image.color = INSTALLATION_CONFIG.DDW_ANIMATION_COLOR;
    }
}
