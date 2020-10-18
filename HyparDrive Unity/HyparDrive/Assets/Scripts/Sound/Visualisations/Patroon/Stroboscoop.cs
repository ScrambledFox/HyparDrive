using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stroboscoop : MonoBehaviour
{
    private GameObject stroboscoop;
    private float[] spectrum;
    private int currentBeat = 0;


    // Start is called before the first frame update
    void Start()
    {
        stroboscoop = Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(0, 4, 0), Quaternion.identity);
        //stroboscoop.GetComponent<Transform>().localScale = new Vector3(15, 15, 15);
        stroboscoop.GetComponent<LightObject>().SetRadius(8);
    }

    // Update is called once per frame
    void Update()
    {
        // initialize our float array
        spectrum = new float[AudioVisualizer.numberOfSamples/4];

        // populate array with fequency spectrum data
        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, GetComponent<AudioVisualizer>().fftWindow);
        for(int x = (int)System.Math.Floor(GetComponent<AudioVisualizer>().minHertz / 93.72f) ; x <= System.Math.Ceiling(GetComponent<AudioVisualizer>().maxHertz / 93.72f) ; x++)
        {
            if(spectrum[x] > AudioVisualizer.threshold)
            {
                currentBeat++;
                if(currentBeat >= GetComponent<AudioVisualizer>().beatAmount)
                {
                    currentBeat = 0;
                    stroboscoop.GetComponent<LightObject>().SetColor(Color.red);
                    StartCoroutine(TurnOffStroboscoop(0.14f));
                }

                break;
            }
        }
    }

    IEnumerator TurnOffStroboscoop(float time)
    {
        yield return new WaitForSeconds(time);
        stroboscoop.GetComponent<LightObject>().SetColor(Color.black);
        // Code to execute after the delay
    }
    void OnDestroy()
    {
        Destroy(stroboscoop);
    }
}
