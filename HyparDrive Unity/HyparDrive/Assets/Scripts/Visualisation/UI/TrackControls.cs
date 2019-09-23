using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackControls : MonoBehaviour
{
    public int trackIndex;
    public void onDeleteClick()
    {
        AnimationCreatorManager.INSTANCE.removeThisTrack(this.gameObject);
        Destroy(transform.gameObject);
    }
}
