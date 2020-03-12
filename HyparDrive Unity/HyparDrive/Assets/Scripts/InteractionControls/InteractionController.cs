using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class InteractionController : MonoBehaviour
{

    public static InteractionController INSTANCE = null;
    public static InteractionData[] lastInteractions = new InteractionData[4];
    public static float margin = 0.5f;
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


        if (InteractionLightManager.INSTANCE.GetTowerState(4- interactionData.unit) == InteractionLightManager.InteractionTowerState.READY)
        {
            HandleInteraction(interactionData);
            //InteractionLightManager.INSTANCE.SetTowerState(0, InteractionLightManager.InteractionTowerState.CHARGING);
            //InteractionLightManager.INSTANCE.SetTowerState(1, InteractionLightManager.InteractionTowerState.CHARGING);
            //InteractionLightManager.INSTANCE.SetTowerState(2, InteractionLightManager.InteractionTowerState.CHARGING);
            //InteractionLightManager.INSTANCE.SetTowerState(3, InteractionLightManager.InteractionTowerState.CHARGING);
            InteractionLightManager.INSTANCE.SetTowerState(4 - interactionData.unit, InteractionLightManager.InteractionTowerState.CHARGING);
        }


        //HandleInteraction(interactionData); 
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


        if (collaborativeInteractions.Count > 1)
        {
            Interaction interaction = new Interaction();
            interaction.type = Interaction.InteractionType.COLLABORATION;
            interaction.interactionModules = new InteractionLightManager.InteractionTowerPhysicalLocations[collaborativeInteractions.Count];

            for (int i = 0; i < collaborativeInteractions.Count; i++)
            {
                switch (collaborativeInteractions[i])
                {
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
            if (thisInteraction.duration <= 3000) {
                interaction.speed = Interaction.InteractionSpeed.SMALL;
            }
            else
            {
                interaction.speed = Interaction.InteractionSpeed.BIG;
            }


            interaction.interactionModules = new InteractionLightManager.InteractionTowerPhysicalLocations[4];
            switch (thisInteraction.unit)
            {
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

                int random = UnityEngine.Random.Range(0, 4);

                switch (interaction.speed)
                {
                    case Interaction.InteractionSpeed.BIG:

                        
                        switch (interaction.interactionModules[0])
                        {
                            case InteractionLightManager.InteractionTowerPhysicalLocations.SE:
                                AnimationPlayer.INSTANCE.PlayAnimation("nature1big");
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "NatureCollab");
                                break;
                            case InteractionLightManager.InteractionTowerPhysicalLocations.SW:
                                AnimationPlayer.INSTANCE.PlayAnimation("nature2big");
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "NatureCollab"); 
                                break;
                            case InteractionLightManager.InteractionTowerPhysicalLocations.NE:
                                AnimationPlayer.INSTANCE.PlayAnimation("tech1big");
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "TechCollab");
                                break;
                            case InteractionLightManager.InteractionTowerPhysicalLocations.NW:
                                AnimationPlayer.INSTANCE.PlayAnimation("tech2big");
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "TechCollab");
                                break;
                            default:
                                break;
                        }

                        break;
                    case Interaction.InteractionSpeed.SMALL:

                        switch (interaction.interactionModules[0])
                        {
                            case InteractionLightManager.InteractionTowerPhysicalLocations.SE:
                                switch (random)
                                {
                                    case 0:
                                        AnimationPlayer.INSTANCE.PlayAnimation("nature1small");
                                        MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Nature1Small1");
                                        break;
                                    case 1:
                                        AnimationPlayer.INSTANCE.PlayAnimation("nature1small");
                                        MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Nature1Small2");
                                        break;
                                    case 2:
                                        AnimationPlayer.INSTANCE.PlayAnimation("nature1small");
                                        MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Nature1Big1");
                                        break;
                                    case 3:
                                        AnimationPlayer.INSTANCE.PlayAnimation("nature1small");
                                        MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Nature1Big2");
                                        break;
                                    default:
                                        break;
                                }

                                break;
                            case InteractionLightManager.InteractionTowerPhysicalLocations.SW:
                                switch (random)
                                {
                                    case 0:
                                        AnimationPlayer.INSTANCE.PlayAnimation("nature2small");
                                        MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Nature1Small1");
                                        break;
                                    case 1:
                                        AnimationPlayer.INSTANCE.PlayAnimation("nature2small");
                                        MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Nature1Small2");
                                        break;
                                    case 2:
                                        AnimationPlayer.INSTANCE.PlayAnimation("nature2small");
                                        MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Nature1Big1");
                                        break;
                                    case 3:
                                        AnimationPlayer.INSTANCE.PlayAnimation("nature2small");
                                        MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Nature1Big2");
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case InteractionLightManager.InteractionTowerPhysicalLocations.NE: 
                                switch (random)
                                {
                                    case 0:
                                        AnimationPlayer.INSTANCE.PlayAnimation("tech1small");
                                        MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Tech1Small1");
                                        break;
                                    case 1:
                                        AnimationPlayer.INSTANCE.PlayAnimation("tech1small");
                                        MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Tech1Small2");
                                        break;
                                    case 2:
                                        AnimationPlayer.INSTANCE.PlayAnimation("tech1small");
                                        MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Tech1Big1");
                                        break;
                                    case 3:
                                        AnimationPlayer.INSTANCE.PlayAnimation("tech1small");
                                        MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Tech1Big2");
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case InteractionLightManager.InteractionTowerPhysicalLocations.NW:
                                switch (random)
                                {
                                    case 0:
                                        AnimationPlayer.INSTANCE.PlayAnimation("tech2small");
                                        MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Tech1Small1");
                                        break;
                                    case 1:
                                        AnimationPlayer.INSTANCE.PlayAnimation("tech2small");
                                        MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Tech1Small2");
                                        break;
                                    case 2:
                                        AnimationPlayer.INSTANCE.PlayAnimation("tech2small");
                                        MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Tech1Big1");
                                        break;
                                    case 3:
                                        AnimationPlayer.INSTANCE.PlayAnimation("tech2small");
                                        MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Tech1Big2");
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }

                        break;
                    default:
                        break;
                }

                /*
                switch (interaction.interactionModules[0]) {
                    case InteractionLightManager.InteractionTowerPhysicalLocations.NE:
                        if(interaction.speed == Interaction.InteractionSpeed.BIG)
                        {
                            AnimationPlayer.INSTANCE.PlayAnimation("tech2big");
                            
                            if (random)
                            {
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Tech2Big1");
                            }
                            else
                            {
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Tech2Big2");
                            }

                        } else
                        {
                            AnimationPlayer.INSTANCE.PlayAnimation("tech2small");

                            if (random)
                            {
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Tech2Small1");
                            }
                            else
                            {
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Tech2Small2");
                            }
                        }
                        break;
                    case InteractionLightManager.InteractionTowerPhysicalLocations.NW:
                        if (interaction.speed == Interaction.InteractionSpeed.BIG)
                        {
                            AnimationPlayer.INSTANCE.PlayAnimation("tech1big");

                            if (random)
                            {
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Tech1Big1");
                            }
                            else
                            {
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Tech1Big2");
                            }
                        }
                        else
                        {
                            AnimationPlayer.INSTANCE.PlayAnimation("tech1small");

                            if (random)
                            {
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Tech1Small1");
                            }
                            else
                            {
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Tech1Small2");
                            }
                        }
                        break;
                    case InteractionLightManager.InteractionTowerPhysicalLocations.SE:
                        if (interaction.speed == Interaction.InteractionSpeed.BIG)
                        {
                            AnimationPlayer.INSTANCE.PlayAnimation("nature2big");

                            if (random)
                            {
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Nature2Big1");
                            }
                            else
                            {
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Nature2Big1");
                            }
                        }
                        else
                        {
                            AnimationPlayer.INSTANCE.PlayAnimation("nature2small");

                            if (random)
                            {
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Nature2Small1");
                            }
                            else
                            {
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Nature2Small2");
                            }
                        }
                        break;
                    case InteractionLightManager.InteractionTowerPhysicalLocations.SW:
                        if (interaction.speed == Interaction.InteractionSpeed.BIG)
                        {
                            AnimationPlayer.INSTANCE.PlayAnimation("nature1big");

                            if (random)
                            {
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Nature1Big1");
                            }
                            else
                            {
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Nature1Big2");
                            }
                        }
                        else
                        {
                            AnimationPlayer.INSTANCE.PlayAnimation("nature1small");

                            if (random)
                            {
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Nature1Small1");
                            }
                            else
                            {
                                MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "Nature1Small2");
                            }
                        }
                        break;
                    default:
                        break;
                }

    */

                break;
            case Interaction.InteractionType.COLLABORATION:

                if (collaborativeInteractions.Count == 2)
                {
                    Debug.Log("BigCollab");
                    AnimationPlayer.INSTANCE.PlayAnimation("bigCollab");
                    MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "BigCollab");
                } 
                //else if(collaborativeInteractions.Contains(0) && collaborativeInteractions.Contains(1))
                //{
                //    Debug.Log("NatureCollab");
                //    AnimationPlayer.INSTANCE.PlayAnimation("natureCollab");
                //    MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "NatureCollab");
                //} else if (collaborativeInteractions.Contains(2) && collaborativeInteractions.Contains(3))
                //{
                //    Debug.Log("TechCollab");
                //    AnimationPlayer.INSTANCE.PlayAnimation("techCollab");
                //    MqttGame.INSTANCE.netHandler.Publish("/interaction/feed/sound", "TechCollab");
                //}

                break;
            default:
                break;
        }

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