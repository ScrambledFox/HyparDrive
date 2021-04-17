using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CustomAnimationController : MonoBehaviour
{

    public Dropdown animationDropdown;
    public Slider speedSlider;
    public Toggle directionToggle;
    public static float speedMultiplier = 1.0f;
    public static bool direction;

    public MonoScript[] animations;
    private List<string> options = new List<string>();

    private int currentAnimation;

    private bool prevLoop = false;
    public bool loop = false;
    public int loopTime;
    
    public GameObject lightSpherePrefab;

    // Update is called once per frame
    void Start ()
    {
        for (int i = 0; i < animations.Length; i++) {
            options.Add(animations[i].name);
        }
        animationDropdown.AddOptions(options);
        currentAnimation = 0;

        ChangeAnimation(animations[currentAnimation].name);




        animationDropdown.onValueChanged.AddListener(delegate {
            animationDropdownValueChangedHandler(animationDropdown);
        });

        speedSlider.onValueChanged.AddListener(delegate {
            SpeedValueChangedHandler(speedSlider);
        });

        directionToggle.onValueChanged.AddListener(delegate {
            DirectionValueChangedHandler(directionToggle);
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

	public void SpeedValueChangedHandler(Slider speedSlider){
        //heightMultiplier = sensitivitySlider.value;
        speedMultiplier = speedSlider.value;
	}

    public void DirectionValueChangedHandler ( Toggle directionToggle ) {
        direction = directionToggle.isOn;
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
