using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationKey {

    public int frame;
    public bool action;

    public AnimationKey (int frame) {
        this.frame = frame;

        Debug.Log("Created a new keyframe at frame " + frame + ".");
    }

}