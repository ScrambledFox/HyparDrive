using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AnimationCreatorManager : MonoBehaviour {

    public static AnimationCreatorManager INSTANCE;

    public delegate void OnAnimationTrackAdded ( Track track );
    public event OnAnimationTrackAdded onAnimationTrackAdded;

    public List<Track> tracks;

    private Animator animator;

    private AnimationClip animationPanelShowHideClip;

    private List<TrackSlot> trackSlots;
    public GameObject trackUIPrefab;

    public Transform addNewTrackButton;
    public Transform playTracksButton;

    private bool playing;
    public static float playbackSpeed=1;


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

  

    //Create a list with all the tracks itself
    public void AddNewTrack () {
        //Track track = new Track();
        //tracks.Add(track);
        //onAnimationTrackAdded?.Invoke(track);
        
        AddNewTrackSlot();
    }
    
    //Create a list with the full trackslots OLD: AddNewTrackSlot(Track track)
    public void AddNewTrackSlot()
    {
        TrackSlot slot = new TrackSlot(GenerateNewSlot());
        trackSlots.Add(slot);               
    }
    private GameObject GenerateNewSlot()
    {
        GameObject newSlot = Instantiate(trackUIPrefab, GameObject.FindWithTag("TrackContent").transform);
        newSlot.transform.SetAsFirstSibling();       
        return newSlot;
    }

    public void changeSpeed(Text input)
    {
        playbackSpeed = float.Parse(input.text.ToString());
        Debug.Log(playbackSpeed);        
    }

    public void PlayTracks()
    {
        playing = !playing;        
    }

    public void removeThisTrack(GameObject trackObj)
    {
        trackSlots.Remove(trackSlots.Single(t => t.trackUI == trackObj));
        Debug.Log(trackSlots.Count());
    }

    public int indexOfTrack(GameObject trackObj)
    {
        int index = trackSlots.FindIndex(t => t.trackUI == trackObj);
        return index;
    }
}



public class TrackSlot
{

    public GameObject trackUI;
    //Track track;
    public Slider slider;
    public List<GameObject> keyFrames;

    //public TrackSlot(GameObject trackUI, Track track)
    public TrackSlot(GameObject trackUI)
    {
        this.trackUI = trackUI;
        this.slider = trackUI.GetComponentInChildren<Slider>();
    }

    public GameObject GetGameObject()
    {
        return trackUI;
    }
}