using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCamera : MonoBehaviour {

    public float rotationSpeed = 1f;
    public float scrollSpeed = 1.0f;
    public float translationSpeed = 1f;

    Vector2 distanceLimits = new Vector2(0.1f, 25f);
    [Range(0.1f, 25f)]
    public float distance = 10f;

    public float rotationSmoothing = 0.1f;
    public float positionSmoothing = 0.1f;

    public Vector2 rotationLimits = new Vector2(0, 90);

    public bool rotationInvertX = false;
    public bool rotationInvertY = false;

    public bool translationInvertX = false;
    public bool translationInvertY = false;

    private Transform target;

    private Vector2 rotation;

    private bool autoRotate = false;

    /// <summary>
    /// Setup camera focus point.
    /// </summary>
    private void Awake () {
        target = new GameObject().transform;
        target.name = "FocusPoint";
    }

    public void ToggleAuto () {
        autoRotate = !autoRotate;
    }

    /// <summary>
    /// Sets the position and rotation for the orbital camera as well   -   movable by holding the right mouse button
    /// </summary>
    private void Update () {

        if (autoRotate) rotation += new Vector2(0.01f * rotationSpeed * Time.deltaTime, 0f * rotationSpeed * Time.deltaTime);

        // You can rotate by holding down the right mouse button.
        if (Input.GetMouseButton(1)) {
            float x = rotationInvertX ? Input.GetAxis("Mouse X") : -Input.GetAxis("Mouse X");
            float y = rotationInvertY ? -Input.GetAxis("Mouse Y") : Input.GetAxis("Mouse Y");
            rotation += new Vector2(x * rotationSpeed * Time.deltaTime, y * rotationSpeed * Time.deltaTime);
            rotation = new Vector2(rotation.x, Mathf.Clamp(rotation.y, 5, 90));
        }

        /// When holding down middle mouse button you can move your target point.
        if (Input.GetMouseButton(2)) {
            float x = translationInvertX ? Input.GetAxis("Mouse X") : -Input.GetAxis("Mouse X");
            float y = translationInvertY ? Input.GetAxis("Mouse Y") : -Input.GetAxis("Mouse Y");

            if (Input.GetKey(KeyCode.LeftShift)) {
                target.Translate(new Vector3(x, y, 0) * translationSpeed, Space.Self);
            } else {
                target.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                target.Translate(new Vector3(x, 0, y) * translationSpeed, Space.Self);
            }
        }

        /// When holding down left alt, you zoom slower.
        float scrollSpeedAbs = scrollSpeed;
        if (Input.GetKey(KeyCode.LeftAlt)) {
            scrollSpeedAbs = scrollSpeed / 5.0f;
        }

        distance -= Input.mouseScrollDelta.y * scrollSpeedAbs;
        distance = Mathf.Clamp(distance, distanceLimits.x, distanceLimits.y);
        

        Quaternion targetRotation = Quaternion.Euler(rotation.y, rotation.x, 0);
        Vector3 targetPosition = target.position - (targetRotation * Vector3.forward * distance);

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotationSmoothing * Time.deltaTime);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, positionSmoothing * Time.deltaTime);

    }

    /// <summary>
    /// Sets the target to orbit around.
    /// </summary>
    /// <param name="target">The target to orbit around.</param>
    public void SetTarget ( Transform target ) {
        this.target.position = target.position;
    }

    public Vector3 GetTarget () {
        return this.target.position;
    }

}