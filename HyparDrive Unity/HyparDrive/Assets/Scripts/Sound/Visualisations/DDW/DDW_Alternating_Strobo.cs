using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DDW_Alternating_Strobo : MonoBehaviour
{
    private GameObject stroboscoopLeft;
    private GameObject stroboscoopRight;
    private Boolean left = true;

    private float[] spectrum;
    private int currentBeat = 0;


    // Start is called before the first frame update
    void Start()
    {
        stroboscoopLeft = Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(-3, 2, 0), Quaternion.identity);
        //stroboscoop.GetComponent<Transform>().localScale = new Vector3(15, 15, 15);
        stroboscoopLeft.GetComponent<LightObject>().SetRadius(3);

        stroboscoopRight = Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(3, 2, 0), Quaternion.identity);
        //stroboscoop.GetComponent<Transform>().localScale = new Vector3(15, 15, 15);
        stroboscoopRight.GetComponent<LightObject>().SetRadius(3);
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
        for (int x = (int)System.Math.Floor(GetComponent<AudioVisualizer>().minHertz / 93.72f); x <= System.Math.Ceiling(GetComponent<AudioVisualizer>().maxHertz / 93.72f); x++)
        {
            if (spectrum[x] > ((2000f / 30000f) - (AudioVisualizer.heightMultiplier / 30000)))
            {
                currentBeat++;
                if (currentBeat >= GetComponent<AudioVisualizer>().beatAmount)
                {
                    currentBeat = 0;
                    if (left)
                    {
                        left = !left;
                        stroboscoopLeft.GetComponent<LightObject>().SetColor(Color.red);
                        StartCoroutine(TurnOffStroboscoopLeft(0.14f));
                    }
                    else
                    {
                        left = !left;
                        stroboscoopRight.GetComponent<LightObject>().SetColor(Color.red);
                        StartCoroutine(TurnOffStroboscoopRight(0.14f));
                    }

                }

                break;
            }
        }
    }

    IEnumerator TurnOffStroboscoopLeft(float time)
    {
        yield return new WaitForSeconds(time);
        stroboscoopLeft.GetComponent<LightObject>().SetColor(Color.black);
        // Code to execute after the delay
    }
    IEnumerator TurnOffStroboscoopRight(float time)
    {
        yield return new WaitForSeconds(time);
        stroboscoopRight.GetComponent<LightObject>().SetColor(Color.black);
        // Code to execute after the delay
    }
    void OnDestroy()
    {
        Destroy(stroboscoopLeft);
        Destroy(stroboscoopRight);
    }
}
