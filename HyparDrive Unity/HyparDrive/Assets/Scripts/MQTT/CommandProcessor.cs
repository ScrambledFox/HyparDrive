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
        try
        {
            return JsonUtility.FromJson<T>(JSON);
        }
        catch 
        {
            return default(T);
        }
         
    }

    public static void ProcessInteractionCommand(string JSON)
    {
        InteractionData interactionData = ToDataType<InteractionData>(JSON);        
        if (interactionData != null)
        {
            ThreadHelper.ExecuteInUpdate(() =>
            {
                interactionData.time = Time.time;

                if (interactionData.type == "send")
                {
                    InteractionController.INSTANCE.registerNewInteraction(interactionData);
                }
            });
        }
    }
    public static void ProcessTimeCommand(string JSON)
    {
        TimeSyncData timeData = ToDataType<TimeSyncData>(JSON);
        if (timeData != null)
        {
            TimeSyncer.syncTime(timeData);
        }
    }
}