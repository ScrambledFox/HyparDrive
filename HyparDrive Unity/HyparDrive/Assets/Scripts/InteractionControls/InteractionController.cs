﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class InteractionController : MonoBehaviour
{

    public static InteractionController INSTANCE = null;
    public static InteractionData[] lastInteractions = new InteractionData[4];
    public static float margin = 1f;
    public static List<int> collaborativeInteractions = new List<int>();

    public void Awake() {
        INSTANCE = this;
    }

    public void registerNewInteraction(InteractionData interactionData)
    {
        // Put timesent into array with all timesents at unit-1
        lastInteractions[interactionData.unit - 1] = interactionData;
        collaborativeInteractions.Clear();

        //Check of deze interactie bijna tegelijk was met een ander
        HandleInteraction(interactionData);
        //}
    }


    float getLastInteractionTime(int unit)
    {
        return lastInteractions[unit - 1].time;
    }


    public static void HandleInteraction(InteractionData thisInteraction)
    {
        // All units of interactions within margin are put into a list

        foreach (InteractionData value in lastInteractions)
        {
            if (value != null)
            {
                if ((value.time >= thisInteraction.time - margin && value.time <= thisInteraction.time + margin))
                {
                    collaborativeInteractions.Add(Array.IndexOf(lastInteractions, value));
                }
            }
        }
        Debug.Log("Interaction between " + thisInteraction.unit + " and " + (collaborativeInteractions.Count - 1) + " others.");
        if (collaborativeInteractions.Count > 1)
        {
            Interaction interaction = new Interaction();
            interaction.type = Interaction.InteractionType.COLLABORATION;
            interaction.interactionModules = new InteractionLightManager.InteractionTowerPhysicalLocations[collaborativeInteractions.Count];

            for (int i = 0; i < collaborativeInteractions.Count; i++) {
                switch (collaborativeInteractions[i]) {
                    case 0:
                        interaction.interactionModules[i] = InteractionLightManager.InteractionTowerPhysicalLocations.SW;
                        break;
                    case 1:
                        interaction.interactionModules[i] = InteractionLightManager.InteractionTowerPhysicalLocations.SE;
                        break;
                    case 2:
                        interaction.interactionModules[i] = InteractionLightManager.InteractionTowerPhysicalLocations.NW;
                        break;
                    case 3:
                        interaction.interactionModules[i] = InteractionLightManager.InteractionTowerPhysicalLocations.NE;
                        break;
                    default:
                        Debug.LogError("WOAT");
                        break;
                }
            }

            DoInteraction(interaction);
        }
        else
        {
            Interaction interaction = new Interaction();
            interaction.type = Interaction.InteractionType.SINGLE;
            if (thisInteraction.duration <= 1000) {
                interaction.speed = Interaction.InteractionSpeed.SMALL;
            }
            else
            {
                interaction.speed = Interaction.InteractionSpeed.BIG;
            }

            switch (thisInteraction.unit) {
                case 1:
                    interaction.interactionModules[0] = InteractionLightManager.InteractionTowerPhysicalLocations.SW;
                    break;
                case 2:
                    interaction.interactionModules[0] = InteractionLightManager.InteractionTowerPhysicalLocations.SE;
                    break;
                case 3:
                    interaction.interactionModules[0] = InteractionLightManager.InteractionTowerPhysicalLocations.NW;
                    break;
                case 4:
                    interaction.interactionModules[0] = InteractionLightManager.InteractionTowerPhysicalLocations.NE;
                    break;
                default:
                    Debug.LogError("WOAT");
                    break;
            }

            DoInteraction(interaction);
        }
    }

    public static void DoInteraction (Interaction interaction) {

        switch (interaction.type) {
            case Interaction.InteractionType.SINGLE:

                switch (interaction.interactionModules[0]) {
                    case InteractionLightManager.InteractionTowerPhysicalLocations.NE:
                        if(interaction.speed == Interaction.InteractionSpeed.BIG)
                        {
                            AnimationPlayer.INSTANCE.PlayAnimation("tech2big");
                        } else
                        {
                            AnimationPlayer.INSTANCE.PlayAnimation("tech2small");
                        }
                        break;
                    case InteractionLightManager.InteractionTowerPhysicalLocations.NW:
                        if (interaction.speed == Interaction.InteractionSpeed.BIG)
                        {
                            AnimationPlayer.INSTANCE.PlayAnimation("tech1big");
                        }
                        else
                        {
                            AnimationPlayer.INSTANCE.PlayAnimation("tech1small");
                        }
                        break;
                    case InteractionLightManager.InteractionTowerPhysicalLocations.SE:
                        if (interaction.speed == Interaction.InteractionSpeed.BIG)
                        {
                            AnimationPlayer.INSTANCE.PlayAnimation("nature2big");
                        }
                        else
                        {
                            AnimationPlayer.INSTANCE.PlayAnimation("nature2small");
                        }
                        break;
                    case InteractionLightManager.InteractionTowerPhysicalLocations.SW:
                        if (interaction.speed == Interaction.InteractionSpeed.BIG)
                        {
                            AnimationPlayer.INSTANCE.PlayAnimation("nature1big");
                        }
                        else
                        {
                            AnimationPlayer.INSTANCE.PlayAnimation("nature1small");
                        }
                        break;
                    default:
                        break;
                }

                break;
            case Interaction.InteractionType.COLLABORATION:

                bool[] interactions = new bool[4];
                if (collaborativeInteractions.Count == 4)
                {
                    Debug.Log("BigCollab");
                    AnimationPlayer.INSTANCE.PlayAnimation("bigCollab");
                } else if(collaborativeInteractions.Contains(0) && collaborativeInteractions.Contains(1))
                {
                    Debug.Log("NatureCollab");
                    AnimationPlayer.INSTANCE.PlayAnimation("natureCollab");
                } else if (collaborativeInteractions.Contains(2) && collaborativeInteractions.Contains(3))
                {
                    Debug.Log("TechCollab");
                    AnimationPlayer.INSTANCE.PlayAnimation("techCollab");
                }

                break;
            default:
                break;
        }

    }

    public void PlayAnimation()
    {

    }

    public struct Interaction {

        public enum InteractionType {
            SINGLE, COLLABORATION
        }
        public enum InteractionSpeed
        {
            BIG, SMALL
        }

        public InteractionType type;
        public InteractionSpeed speed;
        public InteractionLightManager.InteractionTowerPhysicalLocations[] interactionModules;
    }
}