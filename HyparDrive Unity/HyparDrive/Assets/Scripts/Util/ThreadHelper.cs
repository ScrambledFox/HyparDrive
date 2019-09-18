using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreadHelper : MonoBehaviour
{
    public static ThreadHelper INSTANCE = null;

    private static List<System.Action> actionsToTake = new List<System.Action>();
    List<System.Action> actionsToTakeCopied = new List<System.Action>();

    private volatile static bool actionsInQueue = false;

    public static void ExecuteInUpdate(System.Action action)
    {
        if (action == null)
        {
            throw new System.ArgumentNullException("action");
        }

        lock (actionsToTake)
        {
            actionsToTake.Add(action);
            actionsInQueue = true;
        }
    }

    private void Update()
    {
        if (!actionsInQueue) return;

        actionsToTakeCopied.Clear();
        lock (actionsToTakeCopied)
        {
            actionsToTakeCopied.AddRange(actionsToTake);
            actionsToTake.Clear();
            actionsInQueue = false;
        }

        for (int i = 0; i < actionsToTakeCopied.Count; i++)
        {
            actionsToTakeCopied[i].Invoke();
        }
    }
}
