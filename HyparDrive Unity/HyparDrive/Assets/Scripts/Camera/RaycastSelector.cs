using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSelector : MonoBehaviour {

    OrbitalCamera orbitalCamera;

    SelectableObject selectedObject;

    public Transform defocusObjectButton;

    private void Awake () {
        orbitalCamera = Camera.main.GetComponent<OrbitalCamera>();
    }

    public void DeselectCurrentObject () {
        selectedObject.Deselect();
        selectedObject = null;
        defocusObjectButton.gameObject.SetActive(false);
    }

    private void Update () {

        /// Left mouse click     -   Select Cube
        /// Only if no other object is selected
        if (Input.GetMouseButtonDown(0)) {
            if (selectedObject != null) {
                if (orbitalCamera.target == selectedObject.transform) {
                    return;
                }
            }

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f)) {
                if (hit.transform.GetComponent<SelectableObject>()) {
                    SelectableObject clickedObject = hit.transform.GetComponent<SelectableObject>();

                    selectedObject = clickedObject;
                    orbitalCamera.SetTarget(selectedObject.transform);
                    selectedObject.Select();

                    defocusObjectButton.transform.position = selectedObject.transform.position;
                    defocusObjectButton.gameObject.SetActive(true);
                    defocusObjectButton.GetComponent<AttachedUI>().SetTarget(selectedObject.transform);
                }
            }
        }

        // Middle mouse click   -   Focus on Cube
        if (Input.GetMouseButtonDown(2)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f)) {
                orbitalCamera.SetTarget(hit.transform);
            }
        }
    }

}