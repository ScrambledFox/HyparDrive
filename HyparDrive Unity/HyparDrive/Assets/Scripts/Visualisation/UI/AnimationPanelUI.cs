using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPanelUI : MonoBehaviour {

    private Animator animator;

    private AnimationClip animationPanelShowHideClip;

    private List<TrackSlot> trackSlots;
    public GameObject trackUIPrefab;

    public Transform addNewTrackButton;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();

        trackSlots = new List<TrackSlot>();
    }

    private void Start () {
        AnimationCreatorManager.INSTANCE.onAnimationTrackAdded += AddNewTrackSlot;
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

    private void Update () {
        CheckForNewTracks();
    }

    public void CheckForNewTracks () {
        if (AnimationCreatorManager.INSTANCE.tracks.Count != trackSlots.Count) {
            Debug.Log("Things do not match..");
        }
    }

    public void AddNewTrackSlot (Track newTrack) {
        TrackSlot slot = new TrackSlot(GenerateNewSlot(), newTrack);
        trackSlots.Add(slot);

        for (int i = 0; i < trackSlots.Count; i++) {
            trackSlots[i].GetGameObject().GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 50 + i * -125);
        }

        MoveAddTrackButton();
    }

    private GameObject GenerateNewSlot () {
        return Instantiate(trackUIPrefab, GameObject.FindWithTag("TrackContent").transform);
    }

    private void MoveAddTrackButton () {
        addNewTrackButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -50);
    }

}

public class TrackSlot {

    GameObject trackUI;
    Track track;

    public TrackSlot (GameObject trackUI, Track track) {
        this.trackUI = trackUI;
        this.track = track;
    }

    public GameObject GetGameObject () {
        return trackUI;
    }

}