using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steam_StaticColour : MonoBehaviour
{
    public List<GameObject> towerLights = new List<GameObject>();
    private List<LightObject> towerLightObjects = new List<LightObject>();

    // Start is called before the first frame update
    void Start () {
        towerLights.Add(Instantiate(GetComponent<CustomAnimationController>().lightSpherePrefab, new Vector3(0f, 1f, 0), Quaternion.identity));

        for (int i = 0; i < towerLights.Count; i++) {
            towerLightObjects.Add(towerLights[i].GetComponent<LightObject>());
        }

        for (int i = 0; i < towerLightObjects.Count; i++) {
            towerLightObjects[i].SetRadius(10f);
            towerLightObjects[i].SetColor(INSTALLATION_CONFIG.PRIMARY_COLOUR);
        }
    }

    // Update is called once per frame
    void Update () {
        animate();
    }

    private void animate () {
        for (int i = 0; i < towerLights.Count; i++) {
            towerLightObjects[i].SetColor(INSTALLATION_CONFIG.PRIMARY_COLOUR);
        }
    }

    void OnDestroy () {
        for (int i = 0; i < towerLights.Count; i++) {
            Destroy(towerLights[i]);
        }
    }
}
