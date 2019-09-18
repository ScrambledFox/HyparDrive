using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationKey {

    public int frame;
    
    public GameObject attachedObject;
    public Vector3 position;
    public Color colour;
    public float size;

    public AnimationKey (int frame) {
        this.frame = frame;

        Debug.Log("Created a new keyframe at frame " + frame + ".");
    }

}