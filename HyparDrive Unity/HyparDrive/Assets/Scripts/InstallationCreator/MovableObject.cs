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
        UIManager.INSTANCE.SetSettingsScreenState(true);
    }

    private void Update () {
        // Check for holding shift
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            shiftDown = true;
        } else {
            shiftDown = false;
        }

        // Grid Snapping
        if (GridManager.INSTANCE.gridActive) {
            Vector3 currentGridPos = transform.position / CreatorManager.INSTANCE.GridManager.cellSize;
            transform.position = new Vector3(Mathf.Round(currentGridPos.x) * CreatorManager.INSTANCE.GridManager.cellSize,
                                         Mathf.Round(currentGridPos.y) * CreatorManager.INSTANCE.GridManager.cellSize,
                                         Mathf.Round(currentGridPos.z) * CreatorManager.INSTANCE.GridManager.cellSize);
        }
        
    }

    public void Deselect () {
        gameObject.layer = 0;
    }


}