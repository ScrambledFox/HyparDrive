using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreadHelperRuntime : MonoBehaviour {

    List<System.Action> actions = new List<System.Action>();

    private void Update () {
        if (ThreadHelper.actionsInQueue) {
            actions = ThreadHelper.GetActionsInQueue();

            for (int i = 0; i < actions.Count; i++) {
                actions[i].Invoke();
            }
        }
    }

}