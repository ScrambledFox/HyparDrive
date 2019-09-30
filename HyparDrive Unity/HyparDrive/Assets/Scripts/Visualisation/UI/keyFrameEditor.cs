using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class keyFrameEditor : MonoBehaviour
{
    public GameObject keyFramePrefab;
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Input.mousePosition;

            if (RectTransformUtility.RectangleContainsScreenPoint(this.GetComponent<RectTransform>(), mousePos)){
                int index = AnimationCreatorManager.INSTANCE.indexOfTrack(transform.parent.gameObject);
                Debug.Log("Click inside track: " + index);
                GameObject newKeyFrame = Instantiate(keyFramePrefab,transform.Find("keyFrames"));
                

                Vector2 localpoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponent<RectTransform>(), Input.mousePosition, GetComponentInParent<Canvas>().worldCamera, out localpoint);

                Vector2 normalizedPoint = Rect.PointToNormalized(this.GetComponent<RectTransform>().rect, localpoint);

                newKeyFrame.GetComponent<RectTransform>().anchoredPosition = new Vector2(localpoint.x,0);

            }
        }
    }
}
