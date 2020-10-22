using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading_Animation : MonoBehaviour
{
    private GameObject sphere;
    // Start is called before the first frame update
    void Start()
    {
        sphere = Instantiate(GetComponent<AudioVisualizer>().lightSphere, new Vector3(0, -4.5f, 0), Quaternion.identity);
        sphere.GetComponent<LightObject>().SetRadius(4);
    }

    // Update is called once per frame
    void Update()
    {
        animation();
    }

    private void animation()
    {
        float lerpY = Mathf.Lerp(sphere.transform.position.y, sphere.transform.position.y + (AudioVisualizer.heightMultiplier/40000f), GetComponent<AudioVisualizer>().lerpTime);
        if(lerpY > -0.5f)
        {
            lerpY = -4.5f;
        }
        Vector3 newPos = new Vector3(sphere.transform.position.x, lerpY, sphere.transform.position.z);
        sphere.transform.position = newPos;
    }
    void OnDestroy()
    {
        Destroy(sphere);

    }
}
