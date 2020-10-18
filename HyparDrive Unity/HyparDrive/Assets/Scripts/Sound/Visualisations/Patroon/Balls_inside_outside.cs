using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balls_inside_outside : MonoBehaviour
{
    public List<GameObject> spiralAudioObjects = new List<GameObject>();
    public List<GameObject> PTRNAudioObjects = new List<GameObject>();
    private float[] spectrum;


    // Start is called before the first frame update
    void Start()
    {
        spiralAudioObjects.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(-0.4f, 4.5f, 1.5f), Quaternion.identity));
        spiralAudioObjects.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(-0.4f, 4.5f, 1.5f), Quaternion.identity));
        spiralAudioObjects[0].GetComponent<LightObject>().SetRadius(2.5f);
        spiralAudioObjects[1].GetComponent<LightObject>().SetRadius(2.5f);


        PTRNAudioObjects.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(-3.2f, 1, -3.5f), Quaternion.identity));
        PTRNAudioObjects.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(-1, 1, -3.5f), Quaternion.identity));
        PTRNAudioObjects.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(1.2f, 1, -3.5f), Quaternion.identity));
        PTRNAudioObjects.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(3.75f, 1, -3.5f), Quaternion.identity));
        PTRNAudioObjects[0].GetComponent<LightObject>().SetRadius(0.5f);
        PTRNAudioObjects[1].GetComponent<LightObject>().SetRadius(0.5f);
        PTRNAudioObjects[2].GetComponent<LightObject>().SetRadius(0.5f);
        PTRNAudioObjects[3].GetComponent<LightObject>().SetRadius(0.5f);



        //The Mirrored spheres for PTRN
        PTRNAudioObjects.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(-3.2f, 1, -3.5f), Quaternion.identity));
        PTRNAudioObjects.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(-1, 1, -3.5f), Quaternion.identity));
        PTRNAudioObjects.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(1.2f, 1, -3.5f), Quaternion.identity));
        PTRNAudioObjects.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(3.75f, 1, -3.5f), Quaternion.identity));
        PTRNAudioObjects[4].GetComponent<LightObject>().SetRadius(0.5f);
        PTRNAudioObjects[5].GetComponent<LightObject>().SetRadius(0.5f);
        PTRNAudioObjects[6].GetComponent<LightObject>().SetRadius(0.5f);
        PTRNAudioObjects[7].GetComponent<LightObject>().SetRadius(0.5f);



    }

    // Update is called once per frame
    void Update()
    {
        spectrum = new float[AudioVisualizer.numberOfSamples];

        // populate array with fequency spectrum data
        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, GetComponent<AudioVisualizer>().fftWindow);
        SpiralAnimation();
        PTRNAnimation();
        
    }



    private void PTRNAnimation()
    {
        for (int i = 0; i < PTRNAudioObjects.Count/2; i++)
        {

            // apply height multiplier to intensity
            float intensity = spectrum[i + 10] * AudioVisualizer.heightMultiplier/2;

            // calculate object's scale
            float lerpY = Mathf.Lerp(PTRNAudioObjects[i].transform.position.y, intensity + 1, GetComponent<AudioVisualizer>().lerpTime);
            Vector3 newPos = new Vector3(PTRNAudioObjects[i].transform.position.x, lerpY, PTRNAudioObjects[i].transform.position.z);

            float lerpYMirrored = Mathf.Lerp(PTRNAudioObjects[i+4].transform.position.y, (intensity - 1) * -1, GetComponent<AudioVisualizer>().lerpTime);
            Vector3 newPosMirrored = new Vector3(PTRNAudioObjects[i+4].transform.position.x, lerpYMirrored, PTRNAudioObjects[i+4].transform.position.z);

            // appply new position to object
            PTRNAudioObjects[i].transform.position = newPos;
            PTRNAudioObjects[i + 4].transform.position = newPosMirrored;

        }
    }



    private void SpiralAnimation()
    { 

            // apply height multiplier to intensity
            float intensity = spectrum[10] * AudioVisualizer.heightMultiplier/2;

            // calculate object's scale
            float lerpX = Mathf.Lerp(spiralAudioObjects[0].transform.position.x, intensity - 0.4f, GetComponent<AudioVisualizer>().lerpTime);
            Vector3 newPos = new Vector3(lerpX, spiralAudioObjects[0].transform.position.y, spiralAudioObjects[0].transform.position.z);
        float lerpXMirrored = Mathf.Lerp(spiralAudioObjects[1].transform.position.x, (intensity + 0.4f) * -1, GetComponent<AudioVisualizer>().lerpTime);
        Vector3 newPosMirrored = new Vector3(lerpXMirrored, spiralAudioObjects[1].transform.position.y, spiralAudioObjects[1].transform.position.z);
        spiralAudioObjects[0].transform.position = newPos;
        spiralAudioObjects[1].transform.position = newPosMirrored;


       
    }
    void OnDestroy()
    {
        for(int i = 0; i < spiralAudioObjects.Count; i++)
        {
            Destroy(spiralAudioObjects[i]);
        }
        for (int i = 0; i < PTRNAudioObjects.Count; i++)
        {
            Destroy(PTRNAudioObjects[i]);
        }
    }
}
