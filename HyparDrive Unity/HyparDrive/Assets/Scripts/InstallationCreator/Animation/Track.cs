﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track {

    /// <summary>
    /// List of all the keys on this track.
    /// </summary>
    public List<AnimationKey> animationKeys;

    /// <summary>
    /// Creates an empty track.
    /// </summary>
    public Track () {

    }

    public void AddKey (int frame) {
        animationKeys.Add(new AnimationKey(frame));
    }

}
