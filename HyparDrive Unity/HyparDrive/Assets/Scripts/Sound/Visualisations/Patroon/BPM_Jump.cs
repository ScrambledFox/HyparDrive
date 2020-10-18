using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BPM_Jump : MonoBehaviour
{
    Vector3[] locations;
    float totalSpheres;
    public List<GameObject> spheres = new List<GameObject>();
    int currentLocation = 0;
    int numberOfCubes = 31;
    double distanceBetweenCubes;
    private float[] spectrum;
    private int currentBeat = 0;


    // Start is called before the first frame update
    void Start()
    {
        totalSpheres = GetComponent<AudioVisualizer>().sphereAmount;
        locations = GetComponent<AudioVisualizer>().spiralLocations;
        distanceBetweenCubes = System.Math.Floor(numberOfCubes / totalSpheres);

        for(int i = 0; i < totalSpheres; i++)
        {
            spheres.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere));
            spheres[i].GetComponent<LightObject>().SetRadius(1);

        }
        updateSpheres();
    }
    
    // Update is called once per frame
    void Update()
    {
        spectrum = new float[AudioVisualizer.numberOfSamples / 4];
        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, GetComponent<AudioVisualizer>().fftWindow);
        for (int x = (int)System.Math.Floor(GetComponent<AudioVisualizer>().minHertz / 93.72f); x <= System.Math.Ceiling(GetComponent<AudioVisualizer>().maxHertz / 93.72f); x++)
        {
            if (spectrum[x] > AudioVisualizer.threshold)
            {
                currentBeat++;
                if (currentBeat >= GetComponent<AudioVisualizer>().beatAmount)
                {
                    currentBeat = 0;
                    currentLocation++;
                    if (currentLocation >= numberOfCubes)
                    {
                        currentLocation = 0;

                    }
                    updateSpheres();
                }
                break;
            }
        }

    }

    void updateSpheres()
    {
        for (int i = 0; i < totalSpheres; i++)
        {
            int newPos = currentLocation + i * (int)distanceBetweenCubes;
            if (newPos >= numberOfCubes)
            {
                newPos = newPos - numberOfCubes;
            }
            spheres[i].GetComponent<Transform>().position = locations[newPos];
        }
    }
    void OnDestroy()
    {
        for (int i = 0; i < spheres.Count; i++)
        {
            Destroy(spheres[i]);
        }
    }
}
