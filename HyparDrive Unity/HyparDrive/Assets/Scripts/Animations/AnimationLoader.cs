using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationLoader : MonoBehaviour {

    public void PlayAnimation (string name) {

        switch (name) {
            case "testing":
                LoadAnimation(FileManagement.ANIMATION_SAVE_FOLDER + "testing" + FileManagement.FILE_EXTENSION);
                break;
            default:
                Debug.LogError("Animation name not known...");
                break;
        }

    }

    private void LoadAnimation (string filepath) {
        if (FileManagement.CheckIfFileExists(filepath)) {

            FindObjectOfType<AnimationPlayer>().HandleNewAnimation(FileManagement.GetAnimationSaveData(filepath));

        } else {
            Debug.LogError("File not found.");
        }
    }

}