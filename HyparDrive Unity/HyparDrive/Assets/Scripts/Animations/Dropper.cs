using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour {

    System.Random rnd = new System.Random();

    public GameObject objectToDrop;

    private void Update () {
        if (rnd.Next(100) == 0) {
            Instantiate(objectToDrop, transform.position + new Vector3(rnd.Next(4) - 2, 0, rnd.Next(4) - 2), Quaternion.identity);
        }
    }

}
