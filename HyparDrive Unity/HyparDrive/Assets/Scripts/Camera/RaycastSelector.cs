using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSelector : MonoBehaviour {

    OrbitalCamera orbitalCamera;

    MovableObject selectedObject;

    private void Awake () {
        orbitalCamera = Camera.main.GetComponent<OrbitalCamera>();
    }

    public void DeselectCurrentObject () {
        if (selectedObject) {
            selectedObject.Deselect();
            selectedObject = null;
        }
    }

    private void Update () {

        LayerMask arrowMask = 1 << 9;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f, arrowMask)) {
            // Hovering over Arrow Collider

            bool clicked = Input.GetMouseButtonDown(0) ? true : false;

            MoveArrow arrow = hit.transform.GetComponent<MoveArrow>();
            arrow.Hover();

            if (clicked) {
                arrow.Move();
            }

        } else if (Physics.Raycast(ray, out hit, 100f, ~arrowMask)) {
            // Hovering over collider

            bool clicked = Input.GetMouseButtonDown(0) ? true : false;
            if (clicked) {
                if (hit.transform.GetComponent<MovableObject>()) {
                    // Is a selectable object.

                    selectedObject = hit.transform.GetComponent<MovableObject>();
                    selectedObject.Select();
                } else {
                    DeselectCurrentObject();
                }
            }
        } else {
            // Hovering over nothing

            bool clicked = Input.GetMouseButtonDown(0) ? true : false;
            if (clicked) {
                DeselectCurrentObject();
            }
        }
    }

}