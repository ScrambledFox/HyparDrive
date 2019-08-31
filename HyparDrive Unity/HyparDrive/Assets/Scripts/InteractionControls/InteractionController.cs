using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public static class InteractionController
{
    public static long[] lastInteractionTime;
    public static float margin = 0.5f;
    public static void registerNewInteraction(InteractionData interactionData)
    {
        lastInteractionTime[interactionData.unit-1] = interactionData.timeSent;

        //Check time dingetje
        // 
        //Ga animaties doen van die unit af 
        checkNewInteraction(interactionData.unit);
    }


    static long getLastInteraction(int unit)
    {
        return lastInteractionTime[unit - 1];
    }


    public static void checkNewInteraction(int unit)
    {
        long thisInteractionTime = getLastInteraction(unit);
        long[] close = Array.FindAll(lastInteractionTime, n => n > thisInteractionTime - margin && n < thisInteractionTime + margin);
        // Want exact hetzelfde kan 2x voorkomen maar dan met andere index
        for (int i = 0; i < close.Length; i++)
        {
            if (close[i] != unit)
            {
                Debug.Log("Collaboration "+ close[i]);
            }
        }           
    }
}