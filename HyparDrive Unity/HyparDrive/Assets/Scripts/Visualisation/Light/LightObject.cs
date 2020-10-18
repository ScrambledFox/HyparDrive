using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObject : MonoBehaviour {

    public Vector3 Pos { get {
            return transform.position; } }

    [SerializeField]
    private new Collision.SPHERE collider = new Collision.SPHERE(Vector3.zero, 2f);

    [SerializeField]
    private Color colour = Color.white;

    private new MeshRenderer renderer;

    public int trackIndex;

    public LightObject SetTrackIndex(int index)
    {
        this.trackIndex = index;
        return this;
    }

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

    const float moveThreshold = 0.001f;
    const float sqrMoveThreshold = moveThreshold * moveThreshold;

    private Vector3 positionLast;

    public delegate void moved ( LightObject lightObject );
    public event moved Moved;

    private void Start() {
        InstallationManager.INSTANCE.SubscribeLightObject(this);
        Debug.Log("subbed");
        renderer = this.GetComponent<MeshRenderer>();
    }

    //OnEnable to subscribe when the script is enabled again --ADDED BY GINO
    void OnEnable()
    {
        InstallationManager.INSTANCE.SubscribeLightObject(this);
        Debug.Log("subbed");
        renderer = this.GetComponent<MeshRenderer>();
    }

    private void Update () {

        if ((positionLast - this.Pos).magnitude > sqrMoveThreshold) {
            positionLast = transform.position;
            UpdateColliderPosition();

            Moved?.Invoke(this);
        }

        transform.localScale = Vector3.one * this.collider.radius * 2;
        renderer.material.color = new Color(colour.r, colour.g, colour.b, 0.1f);

    }

    public void SetRadius( float radius ) {
        this.collider.radius = radius;
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
        Debug.Log("disabled");
    }

    //private void OnDrawGizmos () {
    //    Gizmos.color = this.colour * new Color(1, 1, 1, 0.2f);
    //    Gizmos.DrawSphere(transform.position, collider.radius);
    //}

    public void SetColor(Color color)
    {
        colour = new Color(color.r, color.g, color.b, 1.0f);

        //renderer.material.color = new Color(color.r, color.g, color.g, 0.5f);
    }
}