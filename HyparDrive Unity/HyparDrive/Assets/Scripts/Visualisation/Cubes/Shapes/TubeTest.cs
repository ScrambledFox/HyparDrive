using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeTest : MonoBehaviour {

    private void Start () {
        GetComponent<MeshFilter>().mesh = PentagonTubeGeneration.GetTube(0);
    }

}
