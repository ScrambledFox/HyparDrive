using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class InteractionLightManager : MonoBehaviour {

    public enum InteractionTowerState {
        READY, CHARGING, DISABLED, SPECIAL
    }

    public enum InteractionTowerType {
        NATURE, TECHNOLOGY
    }

    public enum InteractionTowerPhysicalLocations {
        NE, NW, SE, SW
    }

    Thread interactionLightThread;

    public bool sendArtNetData = true;

    private InteractionTowerData[] towers = new InteractionTowerData[4];

    private void Awake () {
        towers[0] = new InteractionTowerData(InteractionTowerState.CHARGING, InteractionTowerType.TECHNOLOGY, InteractionTowerPhysicalLocations.SW);
        towers[1] = new InteractionTowerData(InteractionTowerState.CHARGING, InteractionTowerType.TECHNOLOGY, InteractionTowerPhysicalLocations.SE);
        towers[2] = new InteractionTowerData(InteractionTowerState.CHARGING, InteractionTowerType.NATURE, InteractionTowerPhysicalLocations.NW);
        towers[3] = new InteractionTowerData(InteractionTowerState.CHARGING, InteractionTowerType.NATURE, InteractionTowerPhysicalLocations.NE);

        interactionLightThread = new Thread(new ThreadStart(InteractionLightThread));
        interactionLightThread.Start();
    }

    DateTime currentTime;
    long lastUpdateTicks = 0;
    public void InteractionLightThread () {
        while (true) {
            currentTime = System.DateTime.Now;

            if (currentTime.Ticks > lastUpdateTicks + 2000000) {

                InteractionTowerUpdate();

                lastUpdateTicks = currentTime.Ticks;
            }

        }
    }

    private void InteractionTowerUpdate () {
        


    }

    public void SetInteractionTowerState (int index, InteractionTowerState state) {
        towers[index].state = state;
    }

    public void SetInteractionTowerState ( InteractionTowerPhysicalLocations location, InteractionTowerState state ) {
        SetInteractionTowerState(GetInteractionTowerIndexAtLocation(location), state);
    }

    private int GetInteractionTowerIndexAtLocation (InteractionTowerPhysicalLocations location) {
        for (int i = 0; i < towers.Length; i++) {
            if (towers[i].location == location) {
                return i;
            }
        }

        return -1;
    }

    struct InteractionTowerData {

        public InteractionTowerState state;
        public InteractionTowerType type;
        public InteractionTowerPhysicalLocations location;

        public InteractionTowerData (InteractionTowerState state, InteractionTowerType type, InteractionTowerPhysicalLocations location) {
            this.state = state;
            this.type = type;
            this.location = location;
        }
    }

}