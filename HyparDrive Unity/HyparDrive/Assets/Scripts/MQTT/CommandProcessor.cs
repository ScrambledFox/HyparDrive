using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Dynamic;

public class CommandProcessor : MonoBehaviour
{
    public static InteractionController interactionController;
    void Start()
    {
        interactionController = GameObject.Find("InteractionController").GetComponent<InteractionController>();
    }

    public static T ToDataType<T>(string JSON)
    {
        return JsonUtility.FromJson<T>(JSON);
    }

    public static void ProcessInteractionCommand(string JSON)
    {
        InteractionData interactionData = ToDataType<InteractionData>(JSON);

        interactionData.time = DateTime.Now.Ticks;

        if(interactionData.type == "send")
        {
            interactionController.registerNewInteraction(interactionData);
        }        
    }
    public static void ProcessTimeCommand(string JSON)
    {
        TimeSyncData timeData = ToDataType<TimeSyncData>(JSON);
        TimeSyncer.syncTime(timeData);
    }
}