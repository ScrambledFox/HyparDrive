using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutopilotManager : MonoBehaviour {

    private Popup_Controller popupController;
    private float colourAutopilotTimer = 0.0f;
    private float animationAutopilotTimer = 0.0f;

    private void Start () {
        popupController = this.GetComponent<Popup_Controller>();
    }

    private void Update () {
        if (SoundPrefManager.GetColourAutopilot()) {
            if (colourAutopilotTimer <= 0.0f) {
                INSTALLATION_CONFIG.DDW_ANIMATION_COLOR = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
                popupController.UpdateColorChannelSliders();
                colourAutopilotTimer = SoundPrefManager.GetColourAutopilotInterval();
            } else {
                colourAutopilotTimer -= Time.deltaTime;
            }
        }

        if (SoundPrefManager.GetAnimationAutopilot()) {
            if (animationAutopilotTimer <= 0.0f) {
                int nextAnimIndex = Random.Range(0, 4);
                popupController.SetSelectedAnimation(nextAnimIndex);
                animationAutopilotTimer = SoundPrefManager.GetAnimationAutopilotInterval();
            } else {
                animationAutopilotTimer -= Time.deltaTime;
            }
        }
    }

}