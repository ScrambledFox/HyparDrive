using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCamera : MonoBehaviour {

    public float speed = 1f;
    public float scrollSpeed = 1.0f;

    Vector2 distanceLimits = new Vector2(0.1f, 25f);
    [Range(0.1f, 25f)]
    public float distance = 10f;

    public float rotationSmoothing = 0.1f;
    public float positionSmoothing = 0.1f;

    public bool invertX = false;
    public bool invertY = false;

    public Transform target;

    private Vector2 rotation;

    /// <summary>
    /// Sets the position and rotation for the orbital camera as well   -   movable by holding the right mouse button
    /// </summary>
    private void Update () {

        if (Input.GetMouseButton(1)) {
            float x = invertX ? Input.GetAxis("Mouse X") : -Input.GetAxis("Mouse X");
            float y = invertY ? -Input.GetAxis("Mouse Y") : Input.GetAxis("Mouse Y");
            rotation += new Vector2(x * speed * Time.deltaTime, y * speed * Time.deltaTime);
        }

        /// When holding down left shift, you zoom slower
        float scrollSpeedAbs = scrollSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) {
            scrollSpeedAbs = scrollSpeed / 5.0f;
        }

        distance -= Input.mouseScrollDelta.y * scrollSpeedAbs;
        distance = Mathf.Clamp(distance, distanceLimits.x, distanceLimits.y);
        

        Quaternion targetRotation = Quaternion.Euler(rotation.y, rotation.x, 0);
        Vector3 targetPosition = target.position - (targetRotation * Vector3.forward * distance);

        //transform.localRotation = targetRotation;
        //transform.localPosition = targetPosition;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotationSmoothing * Time.deltaTime);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, positionSmoothing * Time.deltaTime);

    }

    /// <summary>
    /// Sets the target to orbit around.
    /// </summary>
    /// <param name="target">The target to orbit around.</param>
    public void SetTarget ( Transform target ) {
        this.target = target;
    }

}