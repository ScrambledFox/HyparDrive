using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Controller : MonoBehaviour
{
    public Dropdown microphone;
    public Slider sensitivitySlider, thresholdSlider;
    public GameObject settingsPanel;
    public GameObject openButton;

    private bool panelActive = false;

    // Use this for initialization
    void Start()
    {
        microphone.value = SoundPrefManager.GetMicrophone();
        sensitivitySlider.value = SoundPrefManager.GetSensitivity();
        thresholdSlider.value = SoundPrefManager.GetThreshold();
    }

    public void SaveAndExit()
    {
        SoundPrefManager.SetMicrophone(microphone.value);
        SoundPrefManager.SetSensitivity(sensitivitySlider.value);
        SoundPrefManager.SetThreshold(thresholdSlider.value);

        panelActive = !panelActive;
        settingsPanel.GetComponent<Animator>().SetBool("PanelActive", panelActive);
    }

    public void SetDefaults()
    {
        microphone.value = 0;
        sensitivitySlider.value = 100f;
        thresholdSlider.value = 0.001f;
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

