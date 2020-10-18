using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDW_BPM_Jump : MonoBehaviour
{
    Vector3[] locations;
    float totalSpheres;
    public List<GameObject> spheres = new List<GameObject>();
    int currentLocation = 0;
    int numberOfCubes = 14;
    double distanceBetweenCubes;
    private float[] spectrum;
    private int currentBeat = 0;
    // Start is called before the first frame update
    void Start()
    {
        totalSpheres = GetComponent<AudioVisualizer>().sphereAmount;
        locations = GetComponent<AudioVisualizer>().spiralLocations;
        distanceBetweenCubes = System.Math.Floor(numberOfCubes / totalSpheres);

        for (int i = 0; i < totalSpheres; i++)
        {
            spheres.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere));
            spheres[i].GetComponent<LightObject>().SetRadius(0.5f);

        }
        updateSpheres();
    }

    // Update is called once per frame
    void Update()
    {
        spectrum = new float[AudioVisualizer.numberOfSamples / 4];
        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, GetComponent<AudioVisualizer>().fftWindow);
        audioAnimation();
    }

    private void audioAnimation()
    {
        for (int x = (int)System.Math.Floor(GetComponent<AudioVisualizer>().minHertz / 93.72f); x <= System.Math.Ceiling(GetComponent<AudioVisualizer>().maxHertz / 93.72f); x++)
        {
            if (spectrum[x] > ((2000 / 30000f) - (AudioVisualizer.heightMultiplier / 30000)))
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
