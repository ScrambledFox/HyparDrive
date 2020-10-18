using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Rotating_Stroboscoop : MonoBehaviour
{
    public List<GameObject> stroboscoops = new List<GameObject>();
    float totalSpheres;
    private int currentBeat = 0;



    private float[] spectrum;
    // Start is called before the first frame update
    void Start()
    {
        totalSpheres = GetComponent<AudioVisualizer>().sphereAmount;
        for(int i = 0; i < totalSpheres; i++)
        {
            stroboscoops.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere));
            stroboscoops[i].GetComponent<LightObject>().SetRadius(1);
            stroboscoops[i].AddComponent<Animator>();
            stroboscoops[i].GetComponent<Animator>().runtimeAnimatorController = GetComponent<AudioVisualizer>().spiralAnimationController;
            stroboscoops[i].GetComponent<Animator>().Play("SpiralLoop", 0, 1.0f/totalSpheres*i);
        }



    }

    // Update is called once per frame
    void Update()
    {
        // initialize our float array
        spectrum = new float[AudioVisualizer.numberOfSamples / 4];

        // populate array with fequency spectrum data
        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, GetComponent<AudioVisualizer>().fftWindow);
        for (int x = (int)System.Math.Floor(GetComponent<AudioVisualizer>().minHertz / 93.72f); x <= System.Math.Ceiling(GetComponent<AudioVisualizer>().maxHertz / 93.72f); x++)
        {
            if (spectrum[x] > AudioVisualizer.threshold)
            {
                currentBeat++;
                if(currentBeat >= GetComponent<AudioVisualizer>().beatAmount)
                {
                    currentBeat = 0;
                    for (int i = 0; i < totalSpheres; i++)
                    {
                        stroboscoops[i].GetComponent<LightObject>().SetColor(Color.red);
                    }
                    StartCoroutine(TurnOffStroboscoop(0.14f));
                }

                break;
            }
        }
    }

    IEnumerator TurnOffStroboscoop(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < totalSpheres; i++)
        {
            stroboscoops[i].GetComponent<LightObject>().SetColor(Color.black);
        }
        // Code to execute after the delay
    }
    void OnDestroy()
    {
        for (int i = 0; i < stroboscoops.Count; i++)
        {
            Destroy(stroboscoops[i]);
        }
    }
}
