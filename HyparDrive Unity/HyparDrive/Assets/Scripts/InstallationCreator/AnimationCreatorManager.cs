using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCreatorManager : MonoBehaviour {

    public static AnimationCreatorManager INSTANCE;

    public List<Track> tracks;

    private void Awake () {
        tracks = new List<Track>();
        
        //tracks.Add(new Track());
    }

}