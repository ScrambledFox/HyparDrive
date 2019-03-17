using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ThreadHelper {

    private static List<System.Action> actions = new List<System.Action>();

    public volatile static bool actionsInQueue = false;

    public static void Execute (System.Action action) {
        if (action == null) {
            throw new System.ArgumentNullException("action");
        }

        lock (actions) {
            actions.Add(action);
            actionsInQueue = true;
        }
    }

    public static List<System.Action> GetActionsInQueue () {
        List<System.Action> returnList = new List<System.Action>(actions);
        actions.Clear();
        actionsInQueue = false;
        return returnList;
    }
}