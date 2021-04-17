using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading_Animation : MonoBehaviour
{
    private AudioVisualizer audioVis;

    private List<GameObject> spheres = new List<GameObject>();
    private List<LightObject> sphereLOs = new List<LightObject>();
    // Start is called before the first frame update
    void Start() {
        audioVis = this.GetComponent<AudioVisualizer>();

        spheres.Add(Instantiate(audioVis.lightSphere, new Vector3(-4f, -4.5f, 0), Quaternion.identity));
        spheres.Add(Instantiate(audioVis.lightSphere, new Vector3(4f, -4.5f, 0), Quaternion.identity));
        for (int i = 0; i < spheres.Count; i++) {
            sphereLOs.Add(spheres[i].GetComponent<LightObject>());
            sphereLOs[i].SetRadius(4);
            sphereLOs[i].SetColor(INSTALLATION_CONFIG.PRIMARY_COLOUR);
        }
        
    }

    // Update is called once per frame
    void Update() {
        foreach (LightObject sphere in sphereLOs) {
            sphere.SetColor(INSTALLATION_CONFIG.PRIMARY_COLOUR);
        }

        Animation();
    }

    private void Animation() {
        float y = spheres[0].transform.position.y;
        float newY = y;

        if (newY > 1f) {
            newY = 1f;
        } else {
            newY = y + Time.deltaTime * audioVis.sensitivitySlider.value * 0.001f;
        }

        foreach (GameObject sphere in spheres) {
            sphere.transform.position = new Vector3(sphere.transform.position.x, newY, sphere.transform.position.z);
        }
    }

    void OnDestroy() {
        foreach (GameObject sphere in spheres) {
            Destroy(sphere);
        }
    }
}
