using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour {

    bool selected = false;

    public bool Selected { get => selected; }

    public void Select () {
        selected = true;

        if (GetComponent<MoveArrow>()) {
            GetComponent<MoveArrow>().Select();
        }

        if (GetComponent<Cube>()) {
            UIManager.INSTANCE.SetTranslationArrows(transform.position);
        }
    }

    public void Deselect () {
        selected = false;

        if (GetComponent<MoveArrow>()) {
            GetComponent<MoveArrow>().Deselect();
        } else {
            UIManager.INSTANCE.HideTranslationArrows();
        }
    }


}