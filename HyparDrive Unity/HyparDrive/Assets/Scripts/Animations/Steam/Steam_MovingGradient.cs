using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steam_MovingGradient : MonoBehaviour
{
    public List<GameObject> towerLights = new List<GameObject>();
    private List<LightObject> towerLightObjects = new List<LightObject>();
    private Color[] colours;

    public int amount = 4;
    public float spacing = 3.0f;

    public bool direction = false;

    private bool state = false;

    // Start is called before the first frame update
    void Start () {
        for (int i = 0; i < amount; i++) {
            towerLights.Add(Instantiate(GetComponent<CustomAnimationController>().lightSpherePrefab, new Vector3(spacing * i - (amount * spacing / 2), 1f, 0), Quaternion.identity));
        }

        for (int i = 0; i < towerLights.Count; i++) {
            towerLightObjects.Add(towerLights[i].GetComponent<LightObject>());
        }

        for (int i = 0; i < towerLightObjects.Count; i++) {
            towerLightObjects[i].SetRadius(3.25f);
            if (i % 2 == 0) {
                towerLightObjects[i].SetColor(INSTALLATION_CONFIG.PRIMARY_COLOUR);
            } else {
                towerLightObjects[i].SetColor(INSTALLATION_CONFIG.SECONDARY_COLOUR);
            }
            
        }
    }

    // Update is called once per frame
    void Update () {
        animate();
    }

    private void animate () {
        float speed = (direction ? CustomAnimationController.speedMultiplier : -CustomAnimationController.speedMultiplier) * Time.deltaTime;

        for (int i = 0; i < towerLights.Count; i++) {
            if (i % 2 == 0) {
                towerLightObjects[i].SetColor(state ? INSTALLATION_CONFIG.PRIMARY_COLOUR : INSTALLATION_CONFIG.SECONDARY_COLOUR);
            } else {
                towerLightObjects[i].SetColor(state ? INSTALLATION_CONFIG.SECONDARY_COLOUR : INSTALLATION_CONFIG.PRIMARY_COLOUR);
            }

            towerLights[i].transform.Translate(speed, 0, 0);

            if (towerLights[i].transform.position.x > (amount * spacing / 2)) {
                towerLights[i].transform.position = new Vector3(-(amount * spacing / 2), towerLights[i].transform.position.y, towerLights[i].transform.position.z);
            } else if (towerLights[i].transform.position.x < -(amount * spacing / 2)) {
                towerLights[i].transform.position = new Vector3((amount * spacing / 2), towerLights[i].transform.position.y, towerLights[i].transform.position.z);
            }
        }
    }

    void OnDestroy () {
        for (int i = 0; i < towerLights.Count; i++) {
            Destroy(towerLights[i]);
        }
    }
}
