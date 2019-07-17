using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrow : MonoBehaviour {

    private Material material;

    public enum ArrowType { Up, Forward, Right }
    public ArrowType type;

    private bool hover;

    Color colour = Color.white;

    private void Start () {
        material = GetComponent<MeshRenderer>().material;

        SetStartColour();
    }

    private void Update () {
        if (hover) {
            Select();
        } else {
            Deselect();
        }

        hover = false;
    }

    private void SetStartColour () {
        switch (type) {
            case ArrowType.Up:
                colour = Color.blue;
                break;
            case ArrowType.Forward:
                colour = Color.green;
                break;
            case ArrowType.Right:
                colour = Color.red;
                break;
            default:
                colour = Color.magenta;
                break;
        }

        material.color = colour;
    }

    public void Move () {
        
    }

    public void Hover () {
        hover = true;
    }

    public void Select () {
        material.color = Color.yellow;
    }

    public void Deselect () {
        material.color = colour;
    }



}