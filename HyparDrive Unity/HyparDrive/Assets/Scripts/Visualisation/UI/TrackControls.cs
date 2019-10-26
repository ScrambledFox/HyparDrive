using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class TrackControls : MonoBehaviour
{
    public int trackIndex;
    public void onDeleteClick()
    {
#if UNITY_EDITOR
        if (EditorUtility.DisplayDialog("Delete?!","Are you sure you want to delete this item?", "Yes", "Hell no"))
        {
            // TODO: Change to trackIndex rather than gameObject

            AnimationCreatorManager.INSTANCE.RemoveThisTrack(this.gameObject);
            //Debug.Log(this.gameObject.GetComponent<KeyFrame>().trackIndex);
            AnimationCreatorManager.INSTANCE.lightObjects[trackIndex].gameObject.GetComponent<MovableObject>().Deselect();
            Destroy(AnimationCreatorManager.INSTANCE.lightObjects[trackIndex].gameObject);
            Destroy(transform.gameObject);
        }
#else
            AnimationCreatorManager.INSTANCE.RemoveThisTrack(this.gameObject);
            //Debug.Log(this.gameObject.GetComponent<KeyFrame>().trackIndex);
            Destroy(AnimationCreatorManager.INSTANCE.lightObjects[trackIndex].gameObject);
            Destroy(transform.gameObject);
#endif
        {

        }
    }
}
