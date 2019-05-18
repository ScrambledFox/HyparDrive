using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObject : MonoBehaviour {

    public Vector3 Pos { get {
            return transform.position; } }

    [SerializeField]
    private new Collision.SPHERE collider = new Collision.SPHERE(0, 0, 0, 10f);

    [SerializeField]
    private Color colour = Color.white;

    public Collision.SPHERE Collider {
        get {
            return collider;
        }
    }

    public Color Colour {
        get {
            return colour;
        }
    }

    const float moveThreshold = 0.1f;
    const float sqrMoveThreshold = moveThreshold * moveThreshold;

    private Vector3 positionLast;

    public delegate void moved ( LightObject lightObject );
    public event moved Moved;



    private void Start () {
        InstallationManager.INSTANCE.SubscribeLightObject(this);
    }

    private void Update () {

        if ((positionLast - this.Pos).magnitude > sqrMoveThreshold) {
            positionLast = transform.position;
            UpdateColliderPosition();

            Moved?.Invoke(this);
        }

    }

    private void UpdateColliderPosition () {
        this.collider.x = transform.position.x;
        this.collider.y = transform.position.y;
        this.collider.z = transform.position.z;
    }

    private void OnDestroy () {
        InstallationManager.INSTANCE.RemoveLightObject(this);
    }

    private void OnDisable () {
        InstallationManager.INSTANCE.RemoveLightObject(this);
    }

    private void OnDrawGizmos () {
        Gizmos.color = this.colour * new Color(1, 1, 1, 0.75f);
        Gizmos.DrawSphere(transform.position, collider.radius);
    }
}