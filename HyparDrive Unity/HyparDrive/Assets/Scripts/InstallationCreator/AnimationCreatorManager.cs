using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AnimationCreatorManager : MonoBehaviour {

    public static AnimationCreatorManager INSTANCE;

    //public delegate void OnAnimationTrackAdded ( Track track );
    //public event OnAnimationTrackAdded onAnimationTrackAdded;

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


    private void Awake()
    {
        INSTANCE = this;

        // Init list of tracks
        //tracks = new List<Track>();

        animator = GameObject.FindGameObjectWithTag("UIAnimator").GetComponent<Animator>();

        // Init list of trackUIs
        trackSlots = new List<TrackSlot>();
    }

    private void Start()
    {
        //AnimationCreatorManager.INSTANCE.onAnimationTrackAdded += AddNewTrackSlot;
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

    // Edit speed of playback with inputfield
    public void changeSpeed(Text input)
    {
        playbackSpeed = float.Parse(input.text.ToString());
        Debug.Log(playbackSpeed);
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
        FileManagement.SaveAnimation(animationName, trackSlots.ToArray());
    }


    //Create a list with all the tracks itself
    public void AddNewTrack () {
        //Track track = new Track();
        //tracks.Add(track);
        //onAnimationTrackAdded?.Invoke(track);
        
        AddNewTrackSlot();
    }
    
    //Create a list with the full trackslots 
    public void AddNewTrackSlot()
    {
        TrackSlot slot = new TrackSlot(GenerateNewSlot());
        trackSlots.Add(slot);
        for (int i = 0; i < trackSlots.Count; i++)
        {
            trackSlots[i].slider.value = 0;
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

    public void removeThisTrack(GameObject trackObj)
    {
        trackSlots.Remove(trackSlots.Single(t => t.trackUI == trackObj));
        Debug.Log(trackSlots.Count());
    }

    // Find the index of a track using its gameobject
    public int indexOfTrack(GameObject trackObj)
    {
        int index = trackSlots.FindIndex(t => t.trackUI == trackObj);
        return index;
    }
    
    // Create a keyframe using the current location and color etc
    public void addKeyframe(GameObject parentOfKeyFrame, KeyFrame keyFrame)
    {
        int index = indexOfTrack(parentOfKeyFrame);
        keyFrame.trackIndex = index;
        keyFrame.position = lightObjects[index].transform.position;
        keyFrame.rotation = lightObjects[index].transform.rotation;
        keyFrame.scale = lightObjects[index].transform.localScale;
        keyFrame.color = lightObjects[index].Colour;
        keyFrame.keyFrameObject.GetComponent<Image>().color = lightObjects[index].Colour;
        trackSlots[index].keyFrames.Add(keyFrame);
    }

    public void removeKeyframe(GameObject parentOfKeyFrame, float thisKeyFramePos) //TODO: Add keyframe details joris needs --> percentage
    {
        int index = indexOfTrack(parentOfKeyFrame);
        // Remove keyframe from the list inside its trackslot

        // TODO: Change parentOfKeyFrame to trackIndex

        trackSlots[index].keyFrames.Remove(trackSlots[index].keyFrames.Single(k => k.keyFrameTime == thisKeyFramePos));
    }
}

// Trackslots list --> TrackSlot --> trackUI + keyFrames List --> keyFrameObject + position, color, trackindex, time, etc.

public class TrackSlot
{

    public GameObject trackUI;
    public Slider slider;
    public List<KeyFrame> keyFrames;

    public TrackSlot(GameObject trackUI)
    {
        this.trackUI = trackUI;
        this.slider = trackUI.GetComponentInChildren<Slider>();
        this.keyFrames = new List<KeyFrame>();
    }

    public GameObject GetGameObject()
    {
        return trackUI;
    }

    public List<KeyFrame> GetKeyFrames()
    {
        return keyFrames;
    }
}