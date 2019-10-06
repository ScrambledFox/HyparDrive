using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class keyFrameEditor : MonoBehaviour
{
    public GameObject keyFramePrefab;

    public static keyFrameEditor INSTANCE;
    public void Awake()
    {
        INSTANCE = this;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePos = Input.mousePosition;

            if (RectTransformUtility.RectangleContainsScreenPoint(this.GetComponent<RectTransform>(), mousePos)){
                KeyFrame keyFrame = AddNewKeyFrame(mousePos);

                AnimationCreatorManager.INSTANCE.addKeyframe(this.transform.parent.gameObject, keyFrame);           
            }
        }
    }
    
    // Create a new KeyFrame at the position of the mouse, inside the track. Give it a float component with its percentage of location
    private KeyFrame AddNewKeyFrame(Vector2 mousePos)
    {
        Vector2 localpoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponent<RectTransform>(), mousePos, GetComponentInParent<Canvas>().worldCamera, out localpoint);

        KeyFrame newKeyFrame = new KeyFrame(CreateKeyFrame(localpoint));

        float keyPosition = (localpoint.x / this.GetComponent<RectTransform>().rect.width + 0.5f) * 100f;
        newKeyFrame.keyFrameLocation = keyPosition;
        Debug.Log("percentage: " + newKeyFrame.keyFrameLocation + "%");

        return newKeyFrame;
    }

    // Instantiate the new keyframe
    private GameObject CreateKeyFrame(Vector2 localpoint)
    {
        GameObject newKeyFrame = Instantiate(keyFramePrefab, transform.Find("keyFrames"));
        newKeyFrame.GetComponent<RectTransform>().anchoredPosition = new Vector2(localpoint.x, 0);

        return newKeyFrame;
    }
}

public class KeyFrame
{

    public GameObject keyFrameObject;
    public float keyFrameLocation; // In % van 100%

    public KeyFrame(GameObject keyFrameObject)
    {
        this.keyFrameObject = keyFrameObject;
    }

    public GameObject GetGameObject()
    {
        return keyFrameObject;
    }

    public float GetLocation()
    {
        return keyFrameLocation;
    }
}
