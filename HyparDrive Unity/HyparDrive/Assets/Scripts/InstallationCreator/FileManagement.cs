using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class FileManagement {

    private static readonly string INSTALLATION_SAVE_FOLDER = Application.persistentDataPath + "/installations/";
    private static readonly string ANIMATION_SAVE_FOLDER = Application.persistentDataPath + "/animations/";
    private static readonly string FILE_EXTENSION = ".hype";

    public static void CheckIfDirectoryExists () {
        if (!Directory.Exists(INSTALLATION_SAVE_FOLDER)) {
            Debug.Log("Created installation save folder at " + INSTALLATION_SAVE_FOLDER);
            Directory.CreateDirectory(INSTALLATION_SAVE_FOLDER);
        }
        if (!Directory.Exists(ANIMATION_SAVE_FOLDER))
        {
            Debug.Log("Created installation save folder at " + ANIMATION_SAVE_FOLDER);
            Directory.CreateDirectory(ANIMATION_SAVE_FOLDER);
        }
    }

    public static int GetInstallationFileCount () {
        return Directory.GetFiles(INSTALLATION_SAVE_FOLDER).Length;
    }

    public static string[] LoadInstallationFiles (  ) {
        CheckIfDirectoryExists();
        return Directory.GetFiles(INSTALLATION_SAVE_FOLDER);
    }

    public static void SaveInstallation ( string fileName, Cube[] cubes ) {

        CheckIfDirectoryExists();

        InstallationSaveState installationSaveState = new InstallationSaveState();
        installationSaveState.lastSaveTime = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();

        installationSaveState.hyparCubes = new InstallationSaveState.HyparCube[cubes.Length];
        for (int i = 0; i < installationSaveState.hyparCubes.Length; i++) {
            installationSaveState.hyparCubes[i] = new InstallationSaveState.HyparCube(
                cubes[i].GetIndex(),
                cubes[i].gameObject.transform.position,
                cubes[i].gameObject.transform.rotation,
                cubes[i].gameObject.transform.localScale
                );
        }

        string jsonData = JsonUtility.ToJson( installationSaveState , true );
        File.WriteAllText(INSTALLATION_SAVE_FOLDER + fileName + FILE_EXTENSION, jsonData);

        Debug.Log("Saved " + fileName + " to " + INSTALLATION_SAVE_FOLDER + fileName + FILE_EXTENSION);

    }

    public static void SaveAnimation(string fileName, TrackSlot[] tracks)
    {

        CheckIfDirectoryExists();


        AnimationSaveState animationSaveState = new AnimationSaveState();
        animationSaveState.lastSaveTime = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();

        // Make appropriate amount of tracks in save file
        animationSaveState.tracks = new AnimationSaveState.Track[tracks.Length];

        // Loop through all tracks in save file
        for (int i = 0; i < animationSaveState.tracks.Length; i++)
        {
            // Create new trackitems in save file, with appropriate amount of keyframes
            animationSaveState.tracks[i] = new AnimationSaveState.Track(
                i,
                tracks[i].keyFrames.Count
                );

            // Loop through all keyframes in track, make them with properties
            for (int j = 0; j < tracks[i].keyFrames.Count; j++)
            {
                animationSaveState.tracks[i].keyFrames[j] = new AnimationSaveState.KeyFrame(
                    tracks[i].keyFrames[j].position,
                    tracks[i].keyFrames[j].rotation,
                    tracks[i].keyFrames[j].scale,
                    tracks[i].keyFrames[j].keyFrameTime,
                    tracks[i].keyFrames[j].color
                    );

            }
        }

        // Write to file as JSON
        string jsonData = JsonUtility.ToJson(animationSaveState, true);
        File.WriteAllText(ANIMATION_SAVE_FOLDER + fileName + FILE_EXTENSION, jsonData);
        Debug.Log("Saved " + fileName + " to " + ANIMATION_SAVE_FOLDER + fileName + FILE_EXTENSION);
    }

}

public class InstallationSaveState {

    public string lastSaveTime;
    public HyparCube[] hyparCubes;

    [Serializable]
    public class HyparCube {
        public int id;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public HyparCube ( int id, Vector3 position, Quaternion rotation, Vector3 scale ) {
            this.id = id;
            this.position = position;
            this.rotation = rotation.eulerAngles;
            this.scale = scale;
        }
    }

}

public class AnimationSaveState
{

    public string lastSaveTime;
    public Track[] tracks;

    [Serializable]
    public class Track
    {
        public int trackIndex;
        public KeyFrame[] keyFrames;

        public Track(int trackIndex, int keyframeLength)
        {
            this.trackIndex = trackIndex;
            this.keyFrames = new KeyFrame[keyframeLength]; ;
        }
    }

    [Serializable]
    public class KeyFrame
    {   
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public float time;
        public Color color;

        public KeyFrame(Vector3 position, Quaternion rotation, Vector3 scale, float time, Color color)
        {
            this.position = position;
            this.rotation = rotation.eulerAngles;
            this.scale = scale;
            this.time = time;
            this.color = color;
        }
    }   
}