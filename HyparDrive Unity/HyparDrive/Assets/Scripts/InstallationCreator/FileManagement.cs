﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class FileManagement
{

    public static readonly string INSTALLATION_SAVE_FOLDER = Application.persistentDataPath + "/installations/";
    public static readonly string ANIMATION_SAVE_FOLDER = Application.persistentDataPath + "/animations/";
    public static readonly string FILE_EXTENSION = ".hype";

    public static void CheckSaveFileDirectorySetup()
    {
        if (!Directory.Exists(INSTALLATION_SAVE_FOLDER))
        {
            Debug.Log("Created installation save folder at " + INSTALLATION_SAVE_FOLDER);
            Directory.CreateDirectory(INSTALLATION_SAVE_FOLDER);
        }

        if (!Directory.Exists(ANIMATION_SAVE_FOLDER))
        {
            Debug.Log("Created installation save folder at " + ANIMATION_SAVE_FOLDER);
            Directory.CreateDirectory(ANIMATION_SAVE_FOLDER);
        }
    }

    public static bool CheckIfFileExists(string fileName)
    {
        if (File.Exists(INSTALLATION_SAVE_FOLDER + fileName + FILE_EXTENSION))
        {
            Debug.Log("Found " + fileName + " at " + INSTALLATION_SAVE_FOLDER);
            return true;
        }
        else if (File.Exists(ANIMATION_SAVE_FOLDER + fileName + FILE_EXTENSION))
        {
            Debug.Log("Found " + fileName + " at " + ANIMATION_SAVE_FOLDER);
            return true;
        }
        else
        {
            Debug.Log("File " + fileName + " not found at any of the local save directories.");
            return false;
        }
    }

    public static string GetFilePath(string fileName)
    {
        string filepath = null;

        if (File.Exists(INSTALLATION_SAVE_FOLDER + fileName + FILE_EXTENSION))
        {
            filepath = INSTALLATION_SAVE_FOLDER + fileName + FILE_EXTENSION;
        }

        if (File.Exists(ANIMATION_SAVE_FOLDER + fileName + FILE_EXTENSION))
        {
            filepath = ANIMATION_SAVE_FOLDER + fileName + FILE_EXTENSION;
        }

        return filepath;
    }

    public static InstallationSaveData GetInstallationSaveData(string filepath)
    {
        try
        {
            return JsonUtility.FromJson<InstallationSaveData>(File.ReadAllText(filepath));
        }
        catch (Exception e)
        {
            Debug.LogError("Got an exception getting the save data of a file.");
            throw;
        }
    }

    public static int GetInstallationFileCount()
    {
        return Directory.GetFiles(INSTALLATION_SAVE_FOLDER).Length;
    }

    public static string[] LoadInstallationFiles()
    {
        CheckSaveFileDirectorySetup();
        return Directory.GetFiles(INSTALLATION_SAVE_FOLDER);
    }

    public static string GetDateAndTime(string filePath)
    {
        try
        {
            string jsonString = File.ReadAllText(filePath);
            InstallationSaveData installationSaveData = JsonUtility.FromJson<InstallationSaveData>(jsonString);
            return installationSaveData.lastSaveTime;
        }
        catch (Exception e)
        {
            Debug.LogError("Got an exception getting the time and date of a file.");
            throw;
        }

    }

    public static void SaveInstallation(string fileName, Cube[] cubes)
    {

        CheckSaveFileDirectorySetup();

        InstallationSaveData installationSaveState = new InstallationSaveData();
        installationSaveState.lastSaveTime = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();

        installationSaveState.hyparCubes = new InstallationSaveData.HyparCube[cubes.Length];
        for (int i = 0; i < installationSaveState.hyparCubes.Length; i++)
        {
            installationSaveState.hyparCubes[i] = new InstallationSaveData.HyparCube(
                cubes[i].GetIndex(),
                cubes[i].gameObject.transform.position,
                cubes[i].gameObject.transform.rotation,
                cubes[i].gameObject.transform.localScale
                );
        }

        string jsonData = JsonUtility.ToJson(installationSaveState, true);
        File.WriteAllText(INSTALLATION_SAVE_FOLDER + fileName + FILE_EXTENSION, jsonData);

        PlayerPrefs.SetString("LastInstallationFile", INSTALLATION_SAVE_FOLDER + fileName + FILE_EXTENSION);

        Debug.Log("Saved " + fileName + " to " + INSTALLATION_SAVE_FOLDER + fileName + FILE_EXTENSION);

    }

    public static void SaveAnimation(string fileName, string description, float speed, TrackSlot[] tracks)
    {

        CheckSaveFileDirectorySetup();


        AnimationSaveData animationSaveState = new AnimationSaveData();
        animationSaveState.lastSaveTime = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
        animationSaveState.description = description;
        animationSaveState.speed = speed;

        // Make appropriate amount of tracks in save file
        animationSaveState.tracks = new AnimationSaveData.Track[tracks.Length];

        // Loop through all tracks in save file
        for (int i = 0; i < animationSaveState.tracks.Length; i++)
        {
            // Create new trackitems in save file, with appropriate amount of keyframes
            animationSaveState.tracks[i] = new AnimationSaveData.Track(
                i,
                tracks[i].keyFrames.Count
                );

            // Loop through all keyframes in track, make them with properties
            for (int j = 0; j < tracks[i].keyFrames.Count; j++)
            {
                animationSaveState.tracks[i].keyFrames[j] = new AnimationSaveData.KeyFrame(
                    tracks[i].keyFrames[j].time,
                    tracks[i].keyFrames[j].position,
                    tracks[i].keyFrames[j].rotation,
                    tracks[i].keyFrames[j].radius,
                    tracks[i].keyFrames[j].colour
                    );

            }
        }

        // Write to file as JSON
        string jsonData = JsonUtility.ToJson(animationSaveState, true);
        File.WriteAllText(ANIMATION_SAVE_FOLDER + fileName + FILE_EXTENSION, jsonData);
        Debug.Log("Saved " + fileName + " to " + ANIMATION_SAVE_FOLDER + fileName + FILE_EXTENSION);
    }

    public static AnimationSaveData GetAnimationSaveData(string filepath)
    {
        try
        {
            return JsonUtility.FromJson<AnimationSaveData>(File.ReadAllText(filepath));
        }
        catch (Exception e)
        {
            Debug.LogError("Got an exception getting the save data of a file.");
            throw;
        }
    }

}

public class InstallationSaveData
{

    public string lastSaveTime;
    public HyparCube[] hyparCubes;

    [Serializable]
    public class HyparCube
    {
        public int id;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public HyparCube(int id, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.id = id;
            this.position = position;
            this.rotation = rotation.eulerAngles;
            this.scale = scale;
        }
    }

}

public class AnimationSaveData
{

    public string lastSaveTime;
    public string description;
    public float speed;
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

        public KeyFrame GetKeyFrameAt(float time)
        {
            for (int i = 0; i < keyFrames.Length; i++)
            {
                if (keyFrames[i].time > time)
                {
                    return keyFrames[i];
                }
            }

            return null;
        }

        public void PrintKeyFrames()
        {
            for (int i = 0; i < keyFrames.Length; i++)
            {
                Debug.Log(keyFrames[i].time);
            }
        }
    }

    [Serializable]
    public class KeyFrame
    {
        public Vector3 position;
        public Vector3 rotation;
        public float radius;
        public float time;
        public Color colour;

        public KeyFrame(float time, Vector3 position, Quaternion rotation, float radius, Color color)
        {
            this.position = position;
            this.rotation = rotation.eulerAngles;
            this.radius = radius;
            this.time = time;
            this.colour = color;
        }
    }
}