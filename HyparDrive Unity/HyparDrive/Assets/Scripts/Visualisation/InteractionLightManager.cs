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

    System.Random rnd = new System.Random();
    Thread interactionLightThread;

    private Color techGradientColour1 = new Color(4 / 255f, 255 / 255f, 255 / 255f);
    private Color techGradientColour2 = new Color(255 / 255f, 44 / 255f, 226 / 255f);

    private Color natureGradientColour1 = new Color(0, 1.0f, 0);
    private Color natureGradientColour2 = new Color(1.0f, 165 / 255f, 0);

    private float interactionUpdateRate = 1 / 60f;

    public bool sendArtNetData = true;

    private InteractionTowerData[] towers = new InteractionTowerData[4];

    private void Awake () {
        towers[0] = new InteractionTowerData(InteractionTowerState.CHARGING, InteractionTowerType.TECHNOLOGY, InteractionTowerPhysicalLocations.SW, new ChargeStatus());
        towers[1] = new InteractionTowerData(InteractionTowerState.CHARGING, InteractionTowerType.TECHNOLOGY, InteractionTowerPhysicalLocations.SE, new ChargeStatus());
        towers[2] = new InteractionTowerData(InteractionTowerState.CHARGING, InteractionTowerType.NATURE, InteractionTowerPhysicalLocations.NW, new ChargeStatus());
        towers[3] = new InteractionTowerData(InteractionTowerState.CHARGING, InteractionTowerType.NATURE, InteractionTowerPhysicalLocations.NE, new ChargeStatus());

        interactionLightThread = new Thread(new ThreadStart(InteractionLightThread));
        interactionLightThread.Start();
    }

    DateTime currentTime;
    long lastUpdateTicks = 0;
    public void InteractionLightThread () {
        while (true) {
            currentTime = System.DateTime.Now;

            if (currentTime.Ticks > lastUpdateTicks + 500000) {

                if (sendArtNetData) {
                    towers[0].state = InteractionTowerState.READY;
                    towers[1].state = InteractionTowerState.READY;
                    towers[2].state = InteractionTowerState.READY;
                    //towers[3].state = InteractionTowerState.READY;
                    InteractionTowerUpdate();
                }

                lastUpdateTicks = currentTime.Ticks;
            }

        }
    }

    private void InteractionTowerUpdate () {

        for (int i = 0; i < towers.Length; i++) {
            switch (towers[i].type) {
                case InteractionTowerType.NATURE:
                    switch (towers[i].state) {
                        case InteractionTowerState.READY:
                            for (int j = 0; j < INSTALLATION_CONFIG.LEDS_PER_TOWER; j++) {
                                ArtNetController.INSTANCE.SendArtNet(towers[i].LED_START + j, 0, 255, 0);
                            }
                            break;
                        case InteractionTowerState.CHARGING:
                            /* RANDOM LEDS
                            if (towers[i].chargeStatus.ledStates == null) {
                                towers[i].chargeStatus.ledStates = new bool[INSTALLATION_CONFIG.LEDS_PER_TOWER];
                            }

                            if (towers[i].chargeStatus.progress > towers[i].chargeStatus.ledsActive / (float)INSTALLATION_CONFIG.LEDS_PER_TOWER) {
                                bool foundIndex = false;
                                while (!foundIndex) {
                                    int ledIndex = rnd.Next(0, INSTALLATION_CONFIG.LEDS_PER_TOWER - 1);
                                    if (!towers[i].chargeStatus.ledStates[ledIndex]) {
                                        towers[i].chargeStatus.ledStates[ledIndex] = true;
                                        foundIndex = true;
                                    }
                                }
                            }

                            towers[i].chargeStatus.progress += interactionUpdateRate;

                            for (int k = 0; k < INSTALLATION_CONFIG.LEDS_PER_TOWER; k++) {
                                if (towers[i].chargeStatus.ledStates[k]) {
                                    ArtNetController.INSTANCE.SendArtNet(towers[i].LED_START + k, 0, 255, 0);
                                }
                            }*/

                            // GROWING
                            for (int u = 19; u > -1; u++) {
                                if (towers[i].chargeStatus.progress * 80f > (20 - u) * 4f) {
                                    Color gradientColour = Color.Lerp(natureGradientColour1, natureGradientColour2, (20 - u) / 20f);
                                    ArtNetController.INSTANCE.SendArtNet(towers[i].LED_START + u, (byte)(gradientColour.r * 255), (byte)(gradientColour.g * 255), (byte)(gradientColour.b * 255));
                                } else {
                                    ArtNetController.INSTANCE.SendArtNet(towers[i].LED_START + u, 0, 0, 0);
                                }
                            }
                            for (int o = 20; o < 40; o++) {
                                if (towers[i].chargeStatus.progress * 80f > (o - 20) * 4f) {
                                    Color gradientColour = Color.Lerp(natureGradientColour1, natureGradientColour2, (o - 20) / 20f);
                                    ArtNetController.INSTANCE.SendArtNet(towers[i].LED_START + o, (byte)(gradientColour.r * 255), (byte)(gradientColour.g * 255), (byte)(gradientColour.b * 255));
                                } else {
                                    ArtNetController.INSTANCE.SendArtNet(towers[i].LED_START + o, 0, 0, 0);
                                }
                            }
                            for (int p = 59; p > 39; p++) {
                                if (towers[i].chargeStatus.progress * 80f > (20 - (p - 40)) * 4f) {
                                    Color gradientColour = Color.Lerp(natureGradientColour1, natureGradientColour2, (20 - (p - 40)) / 20f);
                                    ArtNetController.INSTANCE.SendArtNet(towers[i].LED_START + p, (byte)(gradientColour.r * 255), (byte)(gradientColour.g * 255), (byte)(gradientColour.b * 255));
                                } else {
                                    ArtNetController.INSTANCE.SendArtNet(towers[i].LED_START + p, 0, 0, 0);
                                }
                            }
                            for (int q = 60; q < 80; q++) {
                                if (towers[i].chargeStatus.progress * 80f > (q - 60) * 4f) {
                                    Color gradientColour = Color.Lerp(natureGradientColour1, natureGradientColour2, (q - 60) / 20f);
                                    ArtNetController.INSTANCE.SendArtNet(towers[i].LED_START + q, (byte)(gradientColour.r * 255), (byte)(gradientColour.g * 255), (byte)(gradientColour.b * 255));
                                } else {
                                    ArtNetController.INSTANCE.SendArtNet(towers[i].LED_START + q, 0, 0, 0);
                                }
                            }

                            towers[i].chargeStatus.progress += interactionUpdateRate;

                            if (towers[i].chargeStatus.progress >= 1.0f) {
                                towers[i].state = InteractionTowerState.READY;
                                towers[i].chargeStatus = new ChargeStatus();
                            }
                            break;
                        case InteractionTowerState.DISABLED:
                            for (int j = 0; j < INSTALLATION_CONFIG.LEDS_PER_TOWER; j++) {
                                ArtNetController.INSTANCE.SendArtNet(towers[i].LED_START + j, 255, 0, 0);
                            }
                            break;
                        case InteractionTowerState.SPECIAL:
                            break;
                        default:
                            break;
                    }
                    break;
                case InteractionTowerType.TECHNOLOGY:
                    switch (towers[i].state) {
                        case InteractionTowerState.READY:
                            for (int j = 0; j < INSTALLATION_CONFIG.LEDS_PER_TOWER; j++) {
                                float percentage = (j / 80.0f);
                                Color colour = Color.Lerp(techGradientColour1, techGradientColour2, (percentage) );
                                ArtNetController.INSTANCE.SendArtNet(towers[i].LED_START + j, (byte)(colour.r * 255), (byte)(colour.g * 255), (byte)(colour.b * 255));
                            }
                            break;
                        case InteractionTowerState.CHARGING:
                            for (int j = 0; j < INSTALLATION_CONFIG.LEDS_PER_TOWER; j++) {
                                float percentage = (j / 80.0f);
                                Color colour = Color.Lerp(techGradientColour1, techGradientColour2, (percentage));

                                if (towers[i].chargeStatus.progress * 80 > j) {
                                    ArtNetController.INSTANCE.SendArtNet(towers[i].LED_START + j, (byte)(colour.r * 255), (byte)(colour.g * 255), (byte)(colour.b * 255));
                                } else {
                                    ArtNetController.INSTANCE.SendArtNet(towers[i].LED_START + j, 0, 0, 0);
                                }   
                            }

                            towers[i].chargeStatus.progress += interactionUpdateRate;

                            if (towers[i].chargeStatus.progress >= 1f) {
                                towers[i].state = InteractionTowerState.READY;
                                towers[i].chargeStatus = new ChargeStatus();
                            }
                            break;
                        case InteractionTowerState.DISABLED:
                            for (int j = 0; j < INSTALLATION_CONFIG.LEDS_PER_TOWER; j++) {
                                ArtNetController.INSTANCE.SendArtNet(towers[i].LED_START + j, 255, 0, 0);
                            }
                            break;
                        case InteractionTowerState.SPECIAL:
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

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
        public ChargeStatus chargeStatus;

        public int LED_START;
        public int LED_END;

        public InteractionTowerData (InteractionTowerState state, InteractionTowerType type, InteractionTowerPhysicalLocations location, ChargeStatus chargeStatus) {
            this.state = state;
            this.type = type;
            this.location = location;
            this.chargeStatus = chargeStatus;

            switch (location) {
                case InteractionTowerPhysicalLocations.NE:
                    LED_START = 19450;
                    LED_END = 19529;
                    break;
                case InteractionTowerPhysicalLocations.NW:
                    LED_START = 19370;
                    LED_END = 19449;
                    break;
                case InteractionTowerPhysicalLocations.SE:
                    LED_START = 19290;
                    LED_END = 19369;
                    break;
                case InteractionTowerPhysicalLocations.SW:
                    LED_START = 19210;
                    LED_END = 19289;
                    break;
                default:
                    LED_START = -1;
                    LED_END = -1;
                    break;
            }
        }
    }

    struct ChargeStatus {

        public float progress;

        // NATURE
        public bool[] ledStates;
        public int ledsActive;

    }

}
