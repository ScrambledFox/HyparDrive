using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDW_VU : MonoBehaviour
{
    public List<GameObject> towerLightObjects = new List<GameObject>();
    float offsetDown = -2f;
    private float[] spectrum;
    // Start is called before the first frame update
    void Start()
    {      
        towerLightObjects.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(-2f, offsetDown, 0), Quaternion.identity));
        towerLightObjects.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(2f, offsetDown, 0), Quaternion.identity));
        towerLightObjects[0].GetComponent<LightObject>().SetRadius(2.0f);
        towerLightObjects[1].GetComponent<LightObject>().SetRadius(2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        spectrum = new float[AudioVisualizer.numberOfSamples];

        // populate array with fequency spectrum data
        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, GetComponent<AudioVisualizer>().fftWindow);
        audioAnimation();
    }

    private void audioAnimation()
    {
        for (int i = 0; i < towerLightObjects.Count; i++)
        {

            // apply height multiplier to intensity
            float intensity = spectrum[i + 10] * AudioVisualizer.heightMultiplier;

            // calculate object's scale
            float lerpY = Mathf.Lerp(towerLightObjects[i].transform.position.y, intensity + offsetDown, GetComponent<AudioVisualizer>().lerpTime);
            Vector3 newPos = new Vector3(towerLightObjects[i].transform.position.x, lerpY, towerLightObjects[i].transform.position.z);

            // appply new position to object
            towerLightObjects[i].transform.position = newPos;

        }
    }
    void OnDestroy()
    {
        Destroy(towerLightObjects[0]);
        Destroy(towerLightObjects[1]);
    }
}
