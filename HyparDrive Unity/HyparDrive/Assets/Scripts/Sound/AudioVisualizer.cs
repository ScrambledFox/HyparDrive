using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic; // So we can use List<>
using System.Collections;
using System;

[Serializable]

public class AudioVisualizer : MonoBehaviour {
		
	[Range(1, 2000)] public static float heightMultiplier;
    [Range(0, 1)] public static float threshold;
	[Range(64, 8192)] public static int numberOfSamples = 1024; //step by 2
	public FFTWindow fftWindow;
	public float lerpTime;
	public Slider sensitivitySlider;
    public Slider threshholdSlider;
    public MonoScript[] animations;
    public Dropdown animationDropdown;
    private List<string> options = new List<string>();
    private bool prevLoop = false;
    public bool loop = false;
    private int currentAnimation;
    public int loopTime;

    [Tooltip("Min hertz for beat detection")]
    public float minHertz;      //Min hertz for beat detection
    [Tooltip("Max hertz for beat detection")]
    public float maxHertz;      //Max hertz for beat detection
    public RuntimeAnimatorController spiralAnimationController;
    [Tooltip("Number of spheres around the spiral")]
    public int sphereAmount;    //Number of spheres around the spiral
    [Tooltip("Beats needed before progressing in animation")]
    public int beatAmount;      //Beats needed before progressing in animation
    public Vector3[] spiralLocations;   //Location of cubes in spiral
    public GameObject lightSphere;



    /*
	 * The intensity of the frequencies found between 0 and 44100 will be
	 * grouped into 1024 elements. So each element will contain a range of about 43.06 Hz.
	 * The average human voice spans from about 60 hz to 9k Hz
	 * we need a way to assign a range to each object that gets animated. that would be the best way to control and modify animatoins.
	*/

    void Start() {
        for (int i = 0; i < animations.Length; i++)
        {
            options.Add(animations[i].name);
        }
        animationDropdown.AddOptions(options);
        currentAnimation = 0;

        ChangeAnimation(animations[currentAnimation].name);




        animationDropdown.onValueChanged.AddListener(delegate
        {
            animationDropdownValueChangedHandler(animationDropdown);
        });


        heightMultiplier = SoundPrefManager.GetSensitivity ();

		sensitivitySlider.onValueChanged.AddListener(delegate {
			SensitivityValueChangedHandler(sensitivitySlider);
		});
        threshholdSlider.onValueChanged.AddListener(delegate {
            thresholdValueChangedHandler(threshholdSlider);
        });
    }

	void Update() {


        if(loop && !prevLoop)
        {
            prevLoop = loop;
            InvokeRepeating("Loop", 0, loopTime);
        }
        if (!loop && prevLoop)
        {
            prevLoop = loop;
            CancelInvoke("Loop");
        }

    }

    void ChangeAnimation(string newAnimation)
    {
        for (int i = 0; i < animations.Length; i++)
        {
            if (GetComponent(animations[i].name))
            {
                Destroy(GetComponent(animations[i].name));
            }
        }
        var type = Type.GetType(newAnimation);
        gameObject.AddComponent(type);

    }

	public void SensitivityValueChangedHandler(Slider sensitivitySlider){
		heightMultiplier = sensitivitySlider.value;
	}
    public void thresholdValueChangedHandler(Slider thresholdSlider)
    {
        threshold = thresholdSlider.value;
    }

    public void animationDropdownValueChangedHandler(Dropdown animation)
    {
        if (!loop)
        {
            currentAnimation = animation.value;
            ChangeAnimation(animations[currentAnimation].name);
        }
    }

    public void Loop()
    {
        currentAnimation++;
        if(currentAnimation > animations.Length - 1)
        {
            currentAnimation = 0;
        }
        ChangeAnimation(animations[currentAnimation].name);
    }

}
