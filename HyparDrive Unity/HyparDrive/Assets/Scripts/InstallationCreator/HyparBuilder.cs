using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class HyparBuilder : MonoBehaviour {

    Cube[] arc = new Cube[16] {
        new Cube(0, new Vector3(0, 3.0f, -0.2f), Quaternion.Euler(90, 90, 0), Vector3.one),
        new Cube(1, new Vector3(0, 2.9f, -0.7f), Quaternion.Euler(90, 90, 0), Vector3.one),
        new Cube(2, new Vector3(0, 2.5f, -1.2f), Quaternion.Euler(90, 90, 0), Vector3.one),
        new Cube(3, new Vector3(0, 2f, -1.6f), Quaternion.Euler(90, 90, 0), Vector3.one),
        new Cube(4, new Vector3(0, 1.5f, -1.8f), Quaternion.Euler(90, 90, 0), Vector3.one),
        new Cube(5, new Vector3(0, 1f, -2.0f), Quaternion.Euler(90, 90, 0), Vector3.one),
        new Cube(6, new Vector3(0, 0.5f, -2.299999952316284f), Quaternion.Euler(90, 90, 0), Vector3.one),
        new Cube(7, new Vector3(0, 0, -2.5f), Quaternion.Euler(90, 90, 0), Vector3.one),
        new Cube(8, new Vector3(0, 3.0f, 0.3f), Quaternion.Euler(90, 90, 0), Vector3.one),
        new Cube(9, new Vector3(0, 2.9f, 0.8f), Quaternion.Euler(90, 90, 0), Vector3.one),
        new Cube(10, new Vector3(0, 2.5f, 1.3f), Quaternion.Euler(90, 90, 0), Vector3.one),
        new Cube(11, new Vector3(0, 2.0f, 1.7f), Quaternion.Euler(90, 90, 0), Vector3.one),
        new Cube(12, new Vector3(0, 1.5f, 1.9f), Quaternion.Euler(90, 90, 0), Vector3.one),
        new Cube(13, new Vector3(0, 1.0f, 2.1f), Quaternion.Euler(90, 90, 0), Vector3.one),
        new Cube(14, new Vector3(0, 0.5f, 2.4f), Quaternion.Euler(90, 90, 0), Vector3.one),
        new Cube(15, new Vector3(0, 0f, 2.6f), Quaternion.Euler(90, 90, 0), Vector3.one)
    };

    List<Cube> cubes = new List<Cube>();

    private void Awake () {

        string INSTALLATION_SAVE_FOLDER = Application.persistentDataPath + "/installations/";
        string ANIMATION_SAVE_FOLDER = Application.persistentDataPath + "/animations/";
        string FILE_EXTENSION = ".hype";

        for (int j = 0; j < 10; j++) {
            for (int i = 0; i < 16; i++) {
                Cube newCube = arc[i];
                newCube.id += j * 16;
                newCube.position += new Vector3((j - 5f) * 0.5f, 0, 0);

                if (j == 0 || j == 9) {
                    newCube.position += new Vector3(0f, 0.9f, 0);
                }
                if (j == 1 || j == 8) {
                    newCube.position += new Vector3(0f, 0.6f, 0);
                }
                if (j == 2 || j == 7) {
                    newCube.position += new Vector3(0f, 0.3f, 0);
                }
                if (j == 3 || j == 6) {
                    newCube.position += new Vector3(0f, 0.1f, 0);
                }
                if (j == 4 || j == 5) {
                    newCube.position += new Vector3(0f, 0f, 0);
                }

                if (newCube.id % 16 < 8) {
                    newCube.rotation = Quaternion.Euler(90f, 0, 180f);
                } else {
                    newCube.rotation = Quaternion.Euler(90f, 0, 0f);                
                }


                newCube.scale = Vector3.one * 0.25f;
                cubes.Add(newCube);
            }


            InstallationSaveData installationSaveState = new InstallationSaveData();
            installationSaveState.lastSaveTime = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();

            installationSaveState.hyparCubes = new InstallationSaveData.HyparCube[cubes.Count];
            for (int i = 0; i < installationSaveState.hyparCubes.Length; i++) {
                installationSaveState.hyparCubes[i] = new InstallationSaveData.HyparCube(
                    cubes[i].id,
                    cubes[i].position,
                    cubes[i].rotation,
                    cubes[i].scale
                    );
            }

            string jsonData = JsonUtility.ToJson(installationSaveState, true);
            File.WriteAllText(INSTALLATION_SAVE_FOLDER + "Hypar160" + FILE_EXTENSION, jsonData);

            PlayerPrefs.SetString("LastInstallationFile", INSTALLATION_SAVE_FOLDER + "Hypar160" + FILE_EXTENSION);

            Debug.Log("Saved " + "Hypar160" + " to " + INSTALLATION_SAVE_FOLDER + "Hypar160" + FILE_EXTENSION);
        }
    }

    struct Cube {
        public int id;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public Cube ( int id, Vector3 position, Quaternion rotation, Vector3 scale ) {
            this.id = id;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
    }

}