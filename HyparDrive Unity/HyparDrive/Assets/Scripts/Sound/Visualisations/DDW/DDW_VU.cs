using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDW_VU : MonoBehaviour
{
    public List<GameObject> towerLights = new List<GameObject>();
    private List<LightObject> towerLightObjects = new List<LightObject>();
    float offsetDown = -2.5f;
    private float[] spectrum;
    // Start is called before the first frame update
    void Start()
    {      
        towerLights.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(-4f, offsetDown, 0), Quaternion.identity));
        towerLights.Add(Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(4f, offsetDown, 0), Quaternion.identity));
        towerLightObjects.Add(towerLights[0].GetComponent<LightObject>());
        towerLightObjects.Add(towerLights[1].GetComponent<LightObject>());
        towerLightObjects[0].SetRadius(2.0f);
        towerLightObjects[0].SetColor(INSTALLATION_CONFIG.DDW_ANIMATION_COLOR);
        towerLightObjects[1].SetRadius(2.0f);
        towerLightObjects[1].SetColor(INSTALLATION_CONFIG.DDW_ANIMATION_COLOR);
    }

    // Update is called once per frame
    void Update()
    {
        spectrum = new float[AudioVisualizer.numberOfSamples];

        // populate array with fequency spectrum data
        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, GetComponent<AudioVisualizer>().fftWindow);

        towerLightObjects[0].SetColor(INSTALLATION_CONFIG.DDW_ANIMATION_COLOR);
        towerLightObjects[1].SetColor(INSTALLATION_CONFIG.DDW_ANIMATION_COLOR);

        audioAnimation();
    }

    private void audioAnimation()
    {
        for (int i = 0; i < towerLights.Count; i++)
        {

            // apply height multiplier to intensity
            float intensity = spectrum[i + 10] * AudioVisualizer.heightMultiplier;

            // calculate object's scale
            float lerpY = Mathf.Lerp(towerLights[i].transform.position.y, intensity + offsetDown, GetComponent<AudioVisualizer>().lerpTime);
            Vector3 newPos = new Vector3(towerLights[i].transform.position.x, lerpY, towerLights[i].transform.position.z);

            // appply new position to object
            towerLights[i].transform.position = newPos;

        }
    }
    void OnDestroy()
    {
        Destroy(towerLights[0]);
        Destroy(towerLights[1]);
    }
}
