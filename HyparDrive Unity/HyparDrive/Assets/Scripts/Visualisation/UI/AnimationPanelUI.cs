using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPanelUI : MonoBehaviour {

    private Animator animator;

    private AnimationClip animationPanelShowHideClip;

    private List<TrackSlot> trackSlots;
    public GameObject trackUIPrefab;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();

        trackSlots = new List<TrackSlot>();
        trackSlots.Add(new TrackSlot(trackSlots.Count, Instantiate(trackUIPrefab), new Track()));
    }

    public void ShowPanel () {
        animator.SetBool("AnimationPanelState", true);
    }

    public void HidePanel () {
        animator.SetBool("AnimationPanelState", false);
    }

    public void TogglePanel () {
        if (!animator.GetBool("AnimationPanelState"))
            ShowPanel();
        else
            HidePanel();
    }

}

public struct TrackSlot {

    int index;
    GameObject trackUI;
    Track track;

    public TrackSlot (int index, GameObject trackUI, Track track) {
        this.index = index;
        this.trackUI = trackUI;
        this.track = track;
    }

}