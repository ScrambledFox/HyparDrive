using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveHeight : MonoBehaviour {

    public float heightChange = -10f;

    private float heightLimit;
    private bool downwards;

    private void Start () {
        heightLimit = transform.position.y + heightChange;

        downwards = heightLimit < transform.position.y; 
    }

    private void Update () {
        if (downwards) {
            if (transform.position.y < heightLimit) {
                Destroy(gameObject);
            }
        } else {
            if (transform.position.y > heightLimit) {
                Destroy(gameObject);
            }
        }
    }

}