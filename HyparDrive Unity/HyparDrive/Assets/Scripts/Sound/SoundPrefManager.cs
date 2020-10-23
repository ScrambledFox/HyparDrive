using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SoundPrefManager : MonoBehaviour
{
    const string MICROPHONE_KEY = "microphone";
    const string SENSITIVITY_KEY = "sensitivity";
    const string THRESHOLD_KEY = "threshold";

    const string ANIM_R_KEY = "animation_colour_red";
    const string ANIM_G_KEY = "animation_colour_green";
    const string ANIM_B_KEY = "animation_colour_blue";

    const string COLOUR_AUTOPILOT_KEY = "colour_autopilot";
    const string ANIMATION_AUTOPILOT_KEY = "animation_autopilot";
    const string COLOUR_AUTOPILOT_INTERVAL_KEY = "colour_autopilot_interval";
    const string ANIMATION_AUTOPILOT_INTERVAL_KEY = "animation_autopilot_interval";

    public static void SetMicrophone(int mic)
    {
        PlayerPrefs.SetInt(MICROPHONE_KEY, mic);
    }

    public static int GetMicrophone()
    {
        return PlayerPrefs.GetInt(MICROPHONE_KEY);
    }


    public static void SetSensitivity(float sensitivity)
    {
        if (sensitivity >= 1f && sensitivity <= 900f)
        {
            PlayerPrefs.SetFloat(SENSITIVITY_KEY, sensitivity);
        }
        else
        {
            Debug.LogError("Sensitivity out of range");
        }
    }

    public static float GetSensitivity()
    {
        return PlayerPrefs.GetFloat(SENSITIVITY_KEY);
    }

    public static void SetThreshold(float threshold)
    {
        if (threshold >= 0f && threshold <= 1f)
        {
            PlayerPrefs.SetFloat(THRESHOLD_KEY, threshold);
        }
        else
        {
            Debug.LogError("Threshold out of range");
        }
    }

    public static float GetThreshold()
    {
        return PlayerPrefs.GetFloat(THRESHOLD_KEY);
    }

    public static void SetColorRChannel ( float val ) {
        PlayerPrefs.SetFloat(ANIM_R_KEY, Mathf.Clamp(val, 0f, 1f));
    }

    public static void SetColorGChannel ( float val ) {
        PlayerPrefs.SetFloat(ANIM_G_KEY, Mathf.Clamp(val, 0f, 1f));
    }

    public static void SetColorBChannel ( float val ) {
        PlayerPrefs.SetFloat(ANIM_B_KEY, Mathf.Clamp(val, 0f, 1f));
    }

    public static Color GetColor () {
        float r = PlayerPrefs.GetFloat(ANIM_R_KEY);
        float g = PlayerPrefs.GetFloat(ANIM_G_KEY);
        float b = PlayerPrefs.GetFloat(ANIM_B_KEY);
        return new Color(r, g, b);
    }

    public static void SetColourAutopilot ( bool state ) {
        PlayerPrefs.SetInt(COLOUR_AUTOPILOT_KEY, state ? 1 : 0);
    }

    public static bool GetColourAutopilot () {
        return PlayerPrefs.GetInt(COLOUR_AUTOPILOT_KEY) == 1 ? true : false;
    }

    public static void SetAnimationAutopilot ( bool state ) {
        PlayerPrefs.SetInt(ANIMATION_AUTOPILOT_KEY, state ? 1 : 0);
    }

    public static bool GetAnimationAutopilot () {
        return PlayerPrefs.GetInt(ANIMATION_AUTOPILOT_KEY) == 1 ? true : false;
    }

    public static void SetColourAutopilotInterval ( float val ) {
        PlayerPrefs.SetFloat(COLOUR_AUTOPILOT_INTERVAL_KEY, Mathf.Max(0.001f, val));
    }

    public static float GetColourAutopilotInterval () {
        return PlayerPrefs.GetFloat(COLOUR_AUTOPILOT_INTERVAL_KEY);
    }

    public static void SetAnimationAutopilotInterval ( float val ) {
        PlayerPrefs.SetFloat(ANIMATION_AUTOPILOT_INTERVAL_KEY, Mathf.Max(0.001f, val));
    }

    public static float GetAnimationAutopilotInterval () {
        return PlayerPrefs.GetFloat(ANIMATION_AUTOPILOT_INTERVAL_KEY);
    }

}
