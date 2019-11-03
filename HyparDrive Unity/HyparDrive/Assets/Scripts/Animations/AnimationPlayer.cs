using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour {

    public GameObject lightObject;

    [SerializeField]
    private List<AnimationObject> animationObjects = new List<AnimationObject>();

    public void HandleNewAnimation ( AnimationSaveData animation) {
        foreach (AnimationSaveData.Track track in animation.tracks) {
            GameObject go = Instantiate(lightObject);

            RegisterNewAnimationObject(go, track);
        }
    }

    private void RegisterNewAnimationObject ( GameObject go, AnimationSaveData.Track track ) {

        animationObjects.Add(new AnimationObject(go, track));

    }

    private void FixedUpdate () {

        foreach (AnimationObject animationObject in animationObjects) {

            //animationObject.gameObject.transform.position;
            animationObject.track.PrintKeyFrames();

        }

    }

    struct AnimationObject {

        public GameObject gameObject;
        public AnimationSaveData.Track track;

        public AnimationObject ( GameObject gameObject, AnimationSaveData.Track track ) {
            this.gameObject = gameObject;
            this.track = track;
        }
    }



}