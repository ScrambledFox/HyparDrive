using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObject : MonoBehaviour {

    public Vector3 Pos { get {
            return transform.position; } }

    [SerializeField]
    private Color colour = Color.white;
    [SerializeField]
    private float radius = 1f;

    const float moveThreshold = 0.1f;
    const float sqrMoveThreshold = moveThreshold * moveThreshold;

    public Color Colour { get {
            return colour; } }
    public float Radius { get {
            return radius; } }

    private Vector3 positionLast;

    public delegate void moved ( LightObject lightObject );
    public event moved Moved;

    private void Start () {
        InstallationManager.INSTANCE.SubscribeLightObject(this);
    }

    private void Update () {

        if ((positionLast - this.Pos).magnitude > sqrMoveThreshold) {
            positionLast = transform.position;

            if (Moved != null) Moved(this);
        }

    }

    private void OnDestroy () {
        InstallationManager.INSTANCE.RemoveLightObject(this);
    }

    private void OnDrawGizmos () {
        Gizmos.color = this.colour * new Color(0, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, this.radius);
    }
}