using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    Vector3 originalPosition;
    Vector3 inspectPosition = new Vector3(0, 5f, 0);
    public float positionSmoothing = 0.1f;

    SelectableObject selectableObject;

    private void Awake () {
        selectableObject = GetComponent<SelectableObject>();

        originalPosition = transform.localPosition;
    }


    private void Update () {

        Vector3 targetPosition;
        if (selectableObject.Selected) {
            targetPosition = inspectPosition;
        } else {
            targetPosition = originalPosition;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, positionSmoothing);
    }
}