using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Controller : MonoBehaviour
{
    public Dropdown animations;
    public Toggle directionToggle;
    public Slider speedSlider, primaryRChannelSlider, primaryGChannelSlider, primaryBChannelSlider, secondaryRChannelSlider, secondaryGChannelSlider, secondaryBChannelSlider;
    public GameObject settingsPanel;
    public GameObject openButton;

    private bool panelActive = false;

    // Use this for initialization
    void Start()
    {
        //microphone.value = SoundPrefManager.GetMicrophone();
        //sensitivitySlider.value = SoundPrefManager.GetSensitivity();
        //thresholdSlider.value = SoundPrefManager.GetThreshold();

        Color animColour = SoundPrefManager.GetColor();
        primaryRChannelSlider.value = animColour.r;
        primaryGChannelSlider.value = animColour.g;
        primaryBChannelSlider.value = animColour.b;

        //colourAutopilot.isOn = SoundPrefManager.GetColourAutopilot();
        //animationAutopilot.isOn = SoundPrefManager.GetAnimationAutopilot();

        //colourAutopilotInterval.SetTextWithoutNotify(SoundPrefManager.GetColourAutopilotInterval().ToString());
        //animationAutopilotInterval.SetTextWithoutNotify(SoundPrefManager.GetColourAutopilotInterval().ToString());
    }

    public void SaveAndExit()
    {
        //SoundPrefManager.SetMicrophone(microphone.value);
        //SoundPrefManager.SetSensitivity(sensitivitySlider.value);
        //SoundPrefManager.SetThreshold(thresholdSlider.value);

        SoundPrefManager.SetColorRChannel(primaryRChannelSlider.value);
        SoundPrefManager.SetColorGChannel(primaryGChannelSlider.value);
        SoundPrefManager.SetColorBChannel(primaryBChannelSlider.value);

        //SoundPrefManager.SetColourAutopilot(colourAutopilot.isOn);
        //SoundPrefManager.SetAnimationAutopilot(animationAutopilot.isOn);

        //SoundPrefManager.SetColourAutopilotInterval(float.Parse(colourAutopilotInterval.text));
        //SoundPrefManager.SetAnimationAutopilotInterval(float.Parse(animationAutopilotInterval.text));

        panelActive = !panelActive;
        settingsPanel.GetComponent<Animator>().SetBool("PanelActive", panelActive);
    }

    public void SetSelectedAnimation ( int index ) {
        animations.value = Mathf.Clamp(index, 0, 3);
    }

    public void UpdateAnimationColor () {
        INSTALLATION_CONFIG.PRIMARY_COLOUR = new Color(primaryRChannelSlider.value, primaryGChannelSlider.value, primaryBChannelSlider.value);
        INSTALLATION_CONFIG.SECONDARY_COLOUR = new Color(secondaryRChannelSlider.value, secondaryGChannelSlider.value, secondaryBChannelSlider.value);

    }

    public void UpdateColorChannelSliders () {
        primaryRChannelSlider.SetValueWithoutNotify(INSTALLATION_CONFIG.PRIMARY_COLOUR.r);
        primaryGChannelSlider.SetValueWithoutNotify(INSTALLATION_CONFIG.PRIMARY_COLOUR.g);
        primaryBChannelSlider.SetValueWithoutNotify(INSTALLATION_CONFIG.PRIMARY_COLOUR.b);

        secondaryRChannelSlider.SetValueWithoutNotify(INSTALLATION_CONFIG.SECONDARY_COLOUR.r);
        secondaryGChannelSlider.SetValueWithoutNotify(INSTALLATION_CONFIG.SECONDARY_COLOUR.g);
        secondaryBChannelSlider.SetValueWithoutNotify(INSTALLATION_CONFIG.SECONDARY_COLOUR.b);
    }

    public void SetDefaults()
    {
        primaryRChannelSlider.value = 1.0f;
        primaryGChannelSlider.value = 0.0f;
        primaryBChannelSlider.value = 0.0f;

        secondaryRChannelSlider.value = 1.0f;
        secondaryGChannelSlider.value = 0.0f;
        secondaryBChannelSlider.value = 0.0f;
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

