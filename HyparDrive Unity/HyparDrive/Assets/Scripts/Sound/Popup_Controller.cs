using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Controller : MonoBehaviour
{
    public Dropdown microphone, animations;
    public Slider sensitivitySlider, thresholdSlider, rChannelSlider, gChannelSlider, bChannelSlider;
    public Toggle colourAutopilot, animationAutopilot;
    public TMP_InputField colourAutopilotInterval, animationAutopilotInterval;
    public GameObject settingsPanel;
    public GameObject openButton;

    private bool panelActive = false;

    // Use this for initialization
    void Start()
    {
        microphone.value = SoundPrefManager.GetMicrophone();
        sensitivitySlider.value = SoundPrefManager.GetSensitivity();
        thresholdSlider.value = SoundPrefManager.GetThreshold();

        Color animColour = SoundPrefManager.GetColor();
        rChannelSlider.value = animColour.r;
        gChannelSlider.value = animColour.g;
        bChannelSlider.value = animColour.b;

        colourAutopilot.isOn = SoundPrefManager.GetColourAutopilot();
        animationAutopilot.isOn = SoundPrefManager.GetAnimationAutopilot();

        colourAutopilotInterval.SetTextWithoutNotify(SoundPrefManager.GetColourAutopilotInterval().ToString());
        animationAutopilotInterval.SetTextWithoutNotify(SoundPrefManager.GetColourAutopilotInterval().ToString());
    }

    public void SaveAndExit()
    {
        SoundPrefManager.SetMicrophone(microphone.value);
        SoundPrefManager.SetSensitivity(sensitivitySlider.value);
        SoundPrefManager.SetThreshold(thresholdSlider.value);

        SoundPrefManager.SetColorRChannel(rChannelSlider.value);
        SoundPrefManager.SetColorGChannel(gChannelSlider.value);
        SoundPrefManager.SetColorBChannel(bChannelSlider.value);

        SoundPrefManager.SetColourAutopilot(colourAutopilot.isOn);
        SoundPrefManager.SetAnimationAutopilot(animationAutopilot.isOn);

        SoundPrefManager.SetColourAutopilotInterval(float.Parse(colourAutopilotInterval.text));
        SoundPrefManager.SetAnimationAutopilotInterval(float.Parse(animationAutopilotInterval.text));

        panelActive = !panelActive;
        settingsPanel.GetComponent<Animator>().SetBool("PanelActive", panelActive);
    }

    public void SetSelectedAnimation ( int index ) {
        animations.value =  Mathf.Clamp(index, 0, 3);
    }

    public void UpdateAnimationColor () {
        INSTALLATION_CONFIG.DDW_ANIMATION_COLOR = new Color(rChannelSlider.value, gChannelSlider.value, bChannelSlider.value);
    }

    public void UpdateColorChannelSliders () {
        rChannelSlider.SetValueWithoutNotify(INSTALLATION_CONFIG.DDW_ANIMATION_COLOR.r);
        gChannelSlider.SetValueWithoutNotify(INSTALLATION_CONFIG.DDW_ANIMATION_COLOR.g);
        bChannelSlider.SetValueWithoutNotify(INSTALLATION_CONFIG.DDW_ANIMATION_COLOR.b);
    }

    public void UpdateAutopilotSettings () {
        SoundPrefManager.SetColourAutopilot(colourAutopilot.isOn);
        SoundPrefManager.SetAnimationAutopilot(animationAutopilot.isOn);

        SoundPrefManager.SetColourAutopilotInterval(float.Parse(colourAutopilotInterval.text));
        SoundPrefManager.SetAnimationAutopilotInterval(float.Parse(animationAutopilotInterval.text));
    }

    public void SetDefaults()
    {
        microphone.value = 0;
        sensitivitySlider.value = 100f;
        thresholdSlider.value = 0.001f;

        rChannelSlider.value = 1.0f;
        gChannelSlider.value = 0.0f;
        bChannelSlider.value = 0.0f;

        colourAutopilot.isOn = false;
        animationAutopilot.isOn = false;

        colourAutopilotInterval.text = "1.0";
        animationAutopilotInterval.text = "1.0";
    }

    public void OpenSettings()
    {
        panelActive = !panelActive;
        settingsPanel.GetComponent<Animator>().SetBool("PanelActive", panelActive);
    }

    public void TogglePanel()
    {
        if (!panelActive)
        {
            OpenSettings();
        }
        else
        {
            SaveAndExit();
        }
    }
}

