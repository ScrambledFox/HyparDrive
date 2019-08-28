using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        TimeSyncData timeData = ToDataType<TimeSyncData>(JSON);
    }
}