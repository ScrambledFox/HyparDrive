using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour {

    private Gizmo gizmoControl;
    private bool shiftDown;

    private void Start () {
        gizmoControl = Resources.FindObjectsOfTypeAll<Gizmo>()[0];
    }

    private void OnMouseDown () {
        Select();
    }

    public void Select () {
        if (gizmoControl != null) {
            if (!shiftDown) {
                gizmoControl.ClearSelection();
            }
            gizmoControl.Show();
            gizmoControl.SelectObject(transform);
            gameObject.layer = 2;
        } else {
            Debug.LogError("No Gizmo Registered.");
        }
    }

    private void Update () {
        // Check for holding shift
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            shiftDown = true;
        } else {
            shiftDown = false;
        }
    }

    public void Deselect () {
        gameObject.layer = 0;
    }


}