using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationLoader : MonoBehaviour {
    public static AnimationLoader INSTANCE = null;
    public void Awake()
    {
        INSTANCE = this;
    }

    public void LoadAnimation (string filename) {
        //check if nam is something nice
        //then put in list
        //else fu
        string filePath = FileManagement.ANIMATION_SAVE_FOLDER + filename + FileManagement.FILE_EXTENSION;
        if (FileManagement.CheckIfFileExists(filename)) {

            FindObjectOfType<AnimationPlayer>().HandleNewAnimation(FileManagement.GetAnimationSaveData(filePath));

        } else {
            Debug.LogError("File not found.");
        }
    }

}