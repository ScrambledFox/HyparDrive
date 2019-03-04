using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour {

    bool selected = false;

    public bool Selected { get => selected; }

    public void Select () {
        selected = true;
    }

    public void Deselect () {
        selected = false;
    }


}