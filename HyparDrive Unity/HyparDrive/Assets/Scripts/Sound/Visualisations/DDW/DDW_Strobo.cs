using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DDW_Strobo : MonoBehaviour
{
    private GameObject stroboscoop;
    private float[] spectrum;
    private int currentBeat = 0;


    // Start is called before the first frame update
    void Start()
    {
        stroboscoop = Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(0, 1, 0), Quaternion.identity);
        LightObject stroboscoopLO = stroboscoop.GetComponent<LightObject>();
        stroboscoopLO.SetRadius(20f);
        stroboscoopLO.SetColor(INSTALLATION_CONFIG.PRIMARY_COLOUR);
    }

    // Update is called once per frame
    void Update()
    {
        // initialize our float array
        spectrum = new float[AudioVisualizer.numberOfSamples / 4];

        // populate array with fequency spectrum data
        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, GetComponent<AudioVisualizer>().fftWindow);
        audioAnimation();
    }

    private void audioAnimation()
    {
        float threshold = (2000f / 30000f) - (AudioVisualizer.heightMultiplier / 30000);
        //Debug.Log(AudioVisualizer.heightMultiplier);
        //Debug.Log(threshold);
        for (int x = (int)System.Math.Floor(GetComponent<AudioVisualizer>().minHertz / 93.72f); x <= System.Math.Ceiling(GetComponent<AudioVisualizer>().maxHertz / 93.72f); x++)
        {
            if (spectrum[x] > threshold)
            {
                currentBeat++;
                if (currentBeat >= GetComponent<AudioVisualizer>().beatAmount)
                {
                    currentBeat = 0;
                    stroboscoop.GetComponent<LightObject>().SetColor(INSTALLATION_CONFIG.PRIMARY_COLOUR);
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
