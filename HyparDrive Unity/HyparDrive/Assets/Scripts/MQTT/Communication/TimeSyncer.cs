using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class TimeSyncer
{

    public static bool happyFam = false;
    public static bool[] unitStatus = new bool[4];

    public static void syncTime(TimeSyncData timeSyncData)
    {
        unitStatus[timeSyncData.unit - 1] = timeSyncData.status;
        happyFam = unitStatus.All(n => n == true);

        Debug.Log(unitStatus[0] + ", " + unitStatus[1] + ", " + unitStatus[2] + ", " + unitStatus[3]);
        Debug.Log(happyFam);
        /*
         * if(happyFam == true)
         * {
         * mqtt.Publish("/interaction/timeSync", "Let's go!");
         * }
        */

    }
}
