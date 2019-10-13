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
        if (EditorUtility.DisplayDialog("Delete?!","Are you sure you want to delete this item?", "Yes", "Hell no"))
        {
            AnimationCreatorManager.INSTANCE.removeThisTrack(this.gameObject);
            Destroy(transform.gameObject);
        }
    }
}
