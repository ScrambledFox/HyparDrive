using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AnimationCreatorManager : MonoBehaviour {

    public static AnimationCreatorManager INSTANCE;

    public static readonly int KEYFRAME_RATE = 600;

    public List<Track> tracks;

    private Animator animator;

    private AnimationClip animationPanelShowHideClip;

    private List<TrackSlot> trackSlots;
    public GameObject trackUIPrefab;

    public Transform addNewTrackButton;
    public Transform playTracksButton;

    private bool playing;
    public static float playbackSpeed=1f;

    public List<LightObject> lightObjects = new List<LightObject>();
    public GameObject[] logicAnimObjects;
    public static string animationName;
    public static string animationDescription;



    private void Awake()
    {
        INSTANCE = this;

        animator = GameObject.FindGameObjectWithTag("UIAnimator").GetComponent<Animator>();

        // Init list of trackUIs
        trackSlots = new List<TrackSlot>();
    }

    private void Update()
    {
        // If playing --> move slider value

        //TODO: Add playback function of lightobjects

        if (playing == true)
        {
            for (int i = 0; i < trackSlots.Count; i++)
            {
                if (trackSlots[i].slider.value < 1)
                {
                    trackSlots[i].slider.value += Time.deltaTime*playbackSpeed*0.1f;
                }
                else
                {
                    trackSlots[i].slider.value = 0;
                }
            }
        }
    }

    public void AddAnimObject(string name)
    {
        //TODO: Add more shapes

        //Depending on which button was clicked, spawn a new lightobject
        GameObject go;
        switch (name)
        {
            case "Blob":
                Debug.Log("Spawning a blobbie");
                go = Instantiate(logicAnimObjects[0]);
                break;
            default:
                go = Instantiate(logicAnimObjects[0]);
                break;
        }

        go.GetComponent<LightObject>().SetTrackIndex(trackSlots.Count);

        // Add new lightobject to list of lightobjects
        lightObjects.Add(go.GetComponent<LightObject>());

        AddNewTrack();
    }


    // Animation Panel actions
    public void ShowPanel()
    {
        animator.SetBool("AnimationPanelState", true);
    }

    public void HidePanel()
    {
        animator.SetBool("AnimationPanelState", false);
    }

    public void TogglePanel()
    {
        if (!animator.GetBool("AnimationPanelState"))
            ShowPanel();
        else
            HidePanel();
    }


    // Change name of animation
    // TODO: Give a default name like "LightObject_01"
    public void SetAnimationName(string fileName)
    {
        animationName = fileName;
    }

    public void SetAnimationDescription (string description) {
        animationDescription = description;
    }

    // Edit speed of playback with inputfield
    public void changeSpeed(Text input)
    {
        try {
            playbackSpeed = float.Parse(input.text.ToString());
        } catch (System.Exception) {
            input.text = "1";
            //throw;
        }
    }

    // Change color of a lightobject
    public void changeColors(GameObject colorButton)
    {
        UIManager.INSTANCE.ToggleColorPickerState();
        Color color = FlexibleColorPicker.INSTANCE.color;
        colorButton.GetComponent<Image>().color = color;
        int trackIndex = Gizmo.INSTANCE.GetSelectedObjects[0].GetComponent<LightObject>().trackIndex;
        lightObjects[trackIndex].SetColor(color);
    }

    // Save full animation to file
    public void SaveAnimation ( ) {
        FileManagement.SaveAnimation(animationName, animationDescription, trackSlots.ToArray());
    }


    //Create a list with all the tracks itself
    public void AddNewTrack () {
        AddNewTrackSlot();
    }

    //Create a list with the full trackslots 
    public void AddNewTrackSlot()
    {
        TrackSlot slot = new TrackSlot(GenerateNewSlot());
        trackSlots.Add(slot);
        slot.slider.onValueChanged.AddListener(delegate { SetGlobalTime(slot.slider.value); });
        slot.slider.onValueChanged.AddListener(delegate { UpdateObjects(slot.slider.value); });
        for (int i = 0; i < trackSlots.Count; i++)
        {
            trackSlots[i].slider.value = 0;
        }

    }

    public void SetGlobalTime(float time)
    {
        for (int i = 0; i < trackSlots.Count; i++)
        {
            trackSlots[i].slider.value = time;
        }
    }

    // Update data of all LO's.
    public void UpdateObjects(float time) {

        foreach (TrackSlot track in trackSlots) {
            if (track.HasPreviousFrame(time)) {
                KeyFrame referenceFrame = track.GetPreviousFrame(time);
                lightObjects[GetIndexOfTrack(track.GetGameObject())].transform.position = referenceFrame.position;
                lightObjects[GetIndexOfTrack(track.GetGameObject())].transform.rotation = referenceFrame.rotation;
                lightObjects[GetIndexOfTrack(track.GetGameObject())].transform.localScale = referenceFrame.scale;
                lightObjects[GetIndexOfTrack(track.GetGameObject())].SetColor(referenceFrame.colour);
            } else {
                // INVISILBE YAS
                lightObjects[GetIndexOfTrack(track.GetGameObject())].SetColor(Color.clear);
            }
        }
        
    }


    // Spawn the new track
    private GameObject GenerateNewSlot()
    {
        GameObject newSlot = Instantiate(trackUIPrefab, GameObject.FindWithTag("TrackContent").transform);
        newSlot.transform.SetAsFirstSibling();       
        return newSlot;
    }

    public void PlayTracks()
    {
        if (!playing)       // If it wasn't playing, start playing. Reset when reaching the end of the slider
        {
            float minValue = 0;
            for (int i = 0; i < trackSlots.Count; i++)
            {
                if (trackSlots[i].slider.value > minValue)
                {
                    minValue = trackSlots[i].slider.value;
                }
            }
            for (int j = 0; j < trackSlots.Count; j++)
            {
                trackSlots[j].slider.value = minValue;
            }
        }
        playing = !playing;
    }

    public void RemoveThisTrack(GameObject trackObj)
    {
        //trackSlots.Find(trackSlots.Single(t => t.trackUI == trackObj)).GetGameObject();
        trackSlots.Remove(trackSlots.Single(t => t.trackUI == trackObj));
    }

    // Find the index of a track using its gameobject
    public int GetIndexOfTrack(GameObject trackObj)
    {
        int index = trackSlots.FindIndex(t => t.trackUI == trackObj);
        return index;
    }
    
    // Create a keyframe using the current location and color etc
    public void AddKeyframe(GameObject parentOfKeyFrame, KeyFrame keyFrame)
    {
        int index = GetIndexOfTrack(parentOfKeyFrame);
        keyFrame.position = lightObjects[index].transform.position;
        keyFrame.rotation = lightObjects[index].transform.rotation;
        keyFrame.scale = lightObjects[index].transform.localScale;
        keyFrame.colour = lightObjects[index].Colour;
        keyFrame.keyFrameObject.GetComponent<Image>().color = lightObjects[index].Colour;
        trackSlots[index].keyFrames.Add(keyFrame);
        trackSlots[index].keyFrames = trackSlots[index].keyFrames.OrderBy( t => t.time).ToList();
        trackSlots[index].RecalculateBuffer();

        //Edit buffering
    }

    public void removeKeyframe(GameObject parentOfKeyFrame, float thisKeyFramePos) //TODO: Add keyframe details joris needs --> percentage
    {
        int index = GetIndexOfTrack(parentOfKeyFrame);
        // Remove keyframe from the list inside its trackslot

        // TODO: Change parentOfKeyFrame to trackIndex

        trackSlots[index].keyFrames.Remove(trackSlots[index].keyFrames.Single(k => k.time == thisKeyFramePos));
    }
}

// Trackslots list --> TrackSlot --> trackUI + keyFrames List --> keyFrameObject + position, color, trackindex, time, etc.

public class TrackSlot {

    public GameObject trackUI;
    public Slider slider;
    public List<KeyFrame> keyFrames;
    public List<KeyFrame> frameBuffer;

    public TrackSlot(GameObject trackUI)
    {
        this.trackUI = trackUI;
        this.slider = trackUI.GetComponentInChildren<Slider>();
        this.keyFrames = new List<KeyFrame>();
        this.frameBuffer = new List<KeyFrame>();
    }

    public GameObject GetGameObject() {
        return trackUI;
    }

    public List<KeyFrame> GetKeyFrames() {
        return keyFrames;
    }
    public List<KeyFrame> GetKeyFrameBuffer() {
        return frameBuffer;
    }

    public bool HasPreviousFrame ( float time ) {        
        return frameBuffer.FindLast(f => f.time <= time) != null ? true : false;
    }

    public KeyFrame GetPreviousFrame ( float time ) {
        return frameBuffer.FindLast(f => f.time <= time);
    }

    public KeyFrame GetLastKeyFrame ( float time ) {
        return keyFrames.FindLast(k => k.time <= time);
    }

    public bool HasLastKeyFrame ( float time ) {
        return keyFrames.FindLast(k => k.time <= time) == null ? false : true;
    }

    public void AddFrameToBuffer ( KeyFrame frame ) {
        this.frameBuffer.Add(frame);
        //Debug.Log("BUF" + frame.time + ", V:" + frame.position + ", R:{" + frame.position.x + ", " + frame.position.y + ", " + frame.position.z + "}");
    }
        
    public KeyFrame GetNextKeyFrame ( float time ) {
        return keyFrames.Find(k => k.time > time);
    }

    public bool HasNextKeyFrame ( float time ) {
        return keyFrames.Find(k => k.time > time) == null ? false : true;
    }

    public void RecalculateBuffer()
    {
        int KEYFRAME_START;
        int KEYFRAME_END;

        if (keyFrames.Count == 0) {
            // Do nothing, there are no keyframes.
            KEYFRAME_START = 0;
            KEYFRAME_END = -1;
        } else if (keyFrames.Count == 1) {
            KEYFRAME_START = Mathf.RoundToInt(keyFrames[0].time * AnimationCreatorManager.KEYFRAME_RATE);
            KEYFRAME_END = KEYFRAME_START;
        } else {
            KEYFRAME_START = Mathf.RoundToInt(keyFrames[0].time * AnimationCreatorManager.KEYFRAME_RATE);
            KEYFRAME_END = AnimationCreatorManager.KEYFRAME_RATE;
        }

#if UNITY_EDITOR
        Debug.Log("Added " + (KEYFRAME_END - KEYFRAME_START) + " keyframes.");
#endif


        for (int t = KEYFRAME_START; t <= KEYFRAME_END; t++) {
            if (frameBuffer.Count == 0) {
                KeyFrame referenceFrame = GetLastKeyFrame((t + 0.50000f) / AnimationCreatorManager.KEYFRAME_RATE);
                AddFrameToBuffer(new KeyFrame(referenceFrame.time, referenceFrame.position, referenceFrame.rotation, referenceFrame.scale, referenceFrame.colour));
            }

            if (HasNextKeyFrame((t + 0.5f) / AnimationCreatorManager.KEYFRAME_RATE)) {
                KeyFrame firstReferenceFrame = GetLastKeyFrame((t + 0.50000f) / AnimationCreatorManager.KEYFRAME_RATE);
                KeyFrame lastReferenceFrame = GetNextKeyFrame((t + 0.50000f) / AnimationCreatorManager.KEYFRAME_RATE);

                float timeConstant = (((float)t - firstReferenceFrame.time * AnimationCreatorManager.KEYFRAME_RATE) / (AnimationCreatorManager.KEYFRAME_RATE * (lastReferenceFrame.time - firstReferenceFrame.time)));
                //Debug.Log("frame " + t + ", with tc " + timeConstant + ", lerped time " + Mathf.Lerp(firstReferenceFrame.time, lastReferenceFrame.time, timeConstant));

                //Debug.Log(Vector3.Lerp(firstReferenceFrame.position, lastReferenceFrame.position, timeConstant));

                AddFrameToBuffer(new KeyFrame(
                    Mathf.Lerp(firstReferenceFrame.time, lastReferenceFrame.time, timeConstant),
                    Vector3.Lerp(firstReferenceFrame.position, lastReferenceFrame.position, timeConstant),
                    Quaternion.Lerp(firstReferenceFrame.rotation, lastReferenceFrame.rotation, timeConstant),
                    Vector3.Lerp(firstReferenceFrame.scale, lastReferenceFrame.scale, timeConstant),
                    Color.Lerp(firstReferenceFrame.colour, lastReferenceFrame.colour, timeConstant))
                );

            } else {
                break;
            }
        }
    }
}