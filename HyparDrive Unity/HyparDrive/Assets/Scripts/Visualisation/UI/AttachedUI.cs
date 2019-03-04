using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachedUI : MonoBehaviour {

    Transform targetObject;

    public void SetTarget ( Transform targetObject ) {
        this.targetObject = targetObject;
    }

    void Update() {
        transform.position = Camera.main.WorldToScreenPoint(targetObject.position);
    }
}
