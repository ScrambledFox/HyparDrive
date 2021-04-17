using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steam_Gradient : MonoBehaviour
{
    public List<GameObject> towerLights = new List<GameObject>();
    private List<LightObject> towerLightObjects = new List<LightObject>();
    private Color[] colours;

    // Start is called before the first frame update
    void Start () {
        towerLights.Add(Instantiate(GetComponent<CustomAnimationController>().lightSpherePrefab, new Vector3(-1.5f, 1f, 0), Quaternion.identity));
        towerLights.Add(Instantiate(GetComponent<CustomAnimationController>().lightSpherePrefab, new Vector3(1.5f, 1f, 0), Quaternion.identity));
        towerLightObjects.Add(towerLights[0].GetComponent<LightObject>());
        towerLightObjects.Add(towerLights[1].GetComponent<LightObject>());
        towerLightObjects[0].SetRadius(3.25f);
        towerLightObjects[0].SetColor(INSTALLATION_CONFIG.PRIMARY_COLOUR);
        towerLightObjects[1].SetRadius(3.25f);
        towerLightObjects[1].SetColor(INSTALLATION_CONFIG.SECONDARY_COLOUR);
    }

    // Update is called once per frame
    void Update () {
        towerLightObjects[0].SetColor(INSTALLATION_CONFIG.PRIMARY_COLOUR);
        towerLightObjects[1].SetColor(INSTALLATION_CONFIG.SECONDARY_COLOUR);

        //animate();
    }

    private void animate () {
        for (int i = 0; i < towerLights.Count; i++) {


            // do stuff

        }
    }
    void OnDestroy () {
        Destroy(towerLights[0]);
        Destroy(towerLights[1]);
    }
}
