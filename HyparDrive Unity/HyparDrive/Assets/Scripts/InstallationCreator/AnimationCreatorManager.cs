using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCreatorManager : MonoBehaviour {

    public static AnimationCreatorManager INSTANCE;

    public delegate void OnAnimationTrackAdded ( Track track );
    public event OnAnimationTrackAdded onAnimationTrackAdded;

    public List<Track> tracks;

    private void Awake () {
        INSTANCE = this;

        // Init list of tracks
        tracks = new List<Track>();
    }

    public void AddNewTrack () {
        Track track = new Track();
        tracks.Add(track);
        onAnimationTrackAdded?.Invoke(track);
    }
}