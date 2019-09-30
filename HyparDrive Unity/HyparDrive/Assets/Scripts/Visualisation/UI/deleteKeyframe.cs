using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class deleteKeyframe : MonoBehaviour
{
    /*public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Input.mousePosition;

            if (RectTransformUtility.RectangleContainsScreenPoint(this.GetComponent<RectTransform>(), mousePos))
            {
                onDeleteClick();
            }
        }
    }*/

    public void onDeleteClick()
    {
        if (EditorUtility.DisplayDialog("Delete?!", "Are you sure you want to delete this item?", "Yes", "Hell no"))
        {
            //TODO:  AnimationCreatorManager.INSTANCE.removeThisKeyFrame(this.gameObject);
            Destroy(gameObject);
            Debug.Log(gameObject);
            
        }
    }
}
