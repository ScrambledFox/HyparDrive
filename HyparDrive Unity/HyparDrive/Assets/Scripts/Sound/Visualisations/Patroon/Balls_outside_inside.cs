using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balls_outside_inside : MonoBehaviour
{
    public List<GameObject> spiralAudioObjects = new List<GameObject>();
    public List<GameObject> PTRNAudioObjects = new List<GameObject>();
    private float[] spectrum;



    // Start is called before the first frame update
    void Start()
    {
        spiralAudioObjects.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(-6.1f, 4.5f, 1.5f), Quaternion.identity));
        spiralAudioObjects.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(4.2f, 4.5f, 1.5f), Quaternion.identity));
        spiralAudioObjects[0].GetComponent<LightObject>().SetRadius(2.5f);
        spiralAudioObjects[1].GetComponent<LightObject>().SetRadius(2.5f);

        PTRNAudioObjects.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(-3.2f, -1, -3.5f), Quaternion.identity));
        PTRNAudioObjects.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(-1, -1, -3.5f), Quaternion.identity));
        PTRNAudioObjects.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(1.2f, -1, -3.5f), Quaternion.identity));
        PTRNAudioObjects.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(3.75f, -1, -3.5f), Quaternion.identity));
        PTRNAudioObjects[0].GetComponent<LightObject>().SetRadius(0.5f);
        PTRNAudioObjects[1].GetComponent<LightObject>().SetRadius(0.5f);
        PTRNAudioObjects[2].GetComponent<LightObject>().SetRadius(0.5f);
        PTRNAudioObjects[3].GetComponent<LightObject>().SetRadius(0.5f);



    }

    // Update is called once per frame
    void Update()
    {
        // initialize our float array
        spectrum = new float[AudioVisualizer.numberOfSamples];

        // populate array with fequency spectrum data
        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, GetComponent<AudioVisualizer>().fftWindow);
        PTRNAnimation();
        SpiralAnimation();



        //Loop over Spiral light objects

    }

    private void PTRNAnimation()
    {
        for (int i = 0; i < PTRNAudioObjects.Count; i++)
        {

            // apply height multiplier to intensity
            float intensity = spectrum[i + 10] * AudioVisualizer.heightMultiplier;

            // calculate object's scale
            float lerpY = Mathf.Lerp(PTRNAudioObjects[i].transform.position.y, intensity - 1, GetComponent<AudioVisualizer>().lerpTime);
            Vector3 newPos = new Vector3(PTRNAudioObjects[i].transform.position.x, lerpY, PTRNAudioObjects[i].transform.position.z);

            // appply new scale to object
            PTRNAudioObjects[i].transform.position = newPos;

        }
    }
    private void SpiralAnimation()
    {
        for (int i = 0; i < spiralAudioObjects.Count; i++)
        {

            // apply height multiplier to intensity
            float intensity = spectrum[i + 10] * AudioVisualizer.heightMultiplier;

            // calculate object's scale            
            float lerpX = Mathf.Lerp(spiralAudioObjects[i].transform.position.x, intensity - 6.5f, GetComponent<AudioVisualizer>().lerpTime);
            Vector3 newPos = new Vector3(lerpX, spiralAudioObjects[i].transform.position.y, spiralAudioObjects[i].transform.position.z);

            if (i == 1)
            {
                lerpX = Mathf.Lerp(spiralAudioObjects[i].transform.position.x, (intensity - 4.2f) * -1, GetComponent<AudioVisualizer>().lerpTime);
                newPos = new Vector3(lerpX, spiralAudioObjects[i].transform.position.y, spiralAudioObjects[i].transform.position.z);
            }
            // appply new scale to object
            spiralAudioObjects[i].transform.position = newPos;

        }
    }

    void OnDestroy()
    {
        for (int i = 0; i < spiralAudioObjects.Count; i++)
        {
            Destroy(spiralAudioObjects[i]);
        }
        for (int i = 0; i < PTRNAudioObjects.Count; i++)
        {
            Destroy(PTRNAudioObjects[i]);
        }
    }
}
