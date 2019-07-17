using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourTest : MonoBehaviour {

    private new MeshRenderer renderer;
    private Material material;

    private void Start () {
        renderer = transform.GetComponent<MeshRenderer>();
        material = renderer.material;
    }

    private void Update () {
        material.color = Random.ColorHSV();
    }

}
