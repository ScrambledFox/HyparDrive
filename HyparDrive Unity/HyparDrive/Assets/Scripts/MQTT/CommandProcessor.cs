using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CommandProcessor : MonoBehaviour
{
    public static T ToDataType<T>(string JSON)
    {
        return JsonUtility.FromJson<T>(JSON);
    }

    public static void ProcessInteractionCommand(string JSON)
    {
        InteractionData interactionData = ToDataType<InteractionData>(JSON);
        InteractionController.registerNewInteraction(interactionData);
    }
    public static void ProcessTimeCommand(string JSON)
    {
        try
        {
            TimeSyncData timeData = ToDataType<TimeSyncData>(JSON);
            TimeSyncer.syncTime(timeData);
        }
        catch (Exception e)
        {
            Debug.Log("No JSON string received "+ e);
        }
    }
}