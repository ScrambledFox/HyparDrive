using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HauteTechnique.Dmx;
using System;



public class ArtNetController : MonoBehaviour
{
    public static ArtNetController INSTANCE = null;
    private ArtNetDmxNode installationNode1, installationNode2, interactionNode;
    private List<DmxUniverse> installation1Universes = new List<DmxUniverse>();
    private List<DmxUniverse> installation2Universes = new List<DmxUniverse>();
    private List<DmxUniverse> interactionUniverses = new List<DmxUniverse>();
    const int numInstallation1Universes = 45;
    const int numInstallation2Universes = 68;
    const int numInteractionUniverses = 2;
    public int testLedIndex;

    public void Awake() {
        INSTANCE = this;
        installationNode1 = new ArtNetDmxNode("Broadcast Node", "192.168.0.11");     // ADVA INSTALLATIE 1
        installationNode2 = new ArtNetDmxNode("Broadcast Node", "192.168.0.12");     // ADVA INSTALLATIE 2
        interactionNode = new ArtNetDmxNode("Broadcast Node", "192.168.0.31");     // ADVA INTERACTIE

        for (int i = 0; i < numInstallation1Universes; i++) {
            installation1Universes.Add(new DmxUniverse(i));
            installationNode1.AddUniverse(installation1Universes[i]);
        }
        for (int i = 0; i < numInstallation2Universes; i++) {
            installation2Universes.Add(new DmxUniverse(i + numInstallation1Universes));
            installationNode2.AddUniverse(installation2Universes[i]);
        }
        for (int i = 0; i < numInteractionUniverses; i++) {
            interactionUniverses.Add(new DmxUniverse(i + numInstallation1Universes + numInstallation2Universes + 1));
            interactionNode.AddUniverse(interactionUniverses[i]);
        }
    }



    public void SendArtNet(int ledIndex, byte r, byte g, byte b) {
        int channelIndex = ledIndex * 3;
        int universeNumber = channelIndex / 512;
        int channelNumber = channelIndex % 512;

        ADVATEK_BOARD board;

        board = universeNumber > numInstallation1Universes - 1 ? (universeNumber > numInstallation1Universes + numInstallation2Universes + 1 ? ADVATEK_BOARD.INTERACTION : ADVATEK_BOARD.INSTALLATION2) : ADVATEK_BOARD.INSTALLATION1;

        switch (board) {
            case ADVATEK_BOARD.INSTALLATION1:
                // nothing
                break;
            case ADVATEK_BOARD.INSTALLATION2:
                channelIndex -= 512 * numInstallation1Universes;
                universeNumber = channelIndex / 512;
                channelNumber = channelNumber % 512;
                break;
            case ADVATEK_BOARD.INTERACTION:
                channelIndex -= 512 * (numInstallation1Universes + numInstallation2Universes + 1);
                universeNumber = channelIndex / 512;
                channelNumber = channelNumber % 512;
                break;
            default:
                break;
        }

        if ((channelIndex%512) <= 509)
        {
            switch (board) {
                case ADVATEK_BOARD.INSTALLATION1:
                    installation1Universes[universeNumber].SetValue(channelNumber, r);
                    installation1Universes[universeNumber].SetValue(channelNumber + 1, g);
                    installation1Universes[universeNumber].SetValue(channelNumber + 2, b);
                    break;
                case ADVATEK_BOARD.INSTALLATION2:
                    installation2Universes[universeNumber].SetValue(channelNumber, r);
                    installation2Universes[universeNumber].SetValue(channelNumber + 1, g);
                    installation2Universes[universeNumber].SetValue(channelNumber + 2, b);
                    break;
                case ADVATEK_BOARD.INTERACTION:
                    interactionUniverses[universeNumber].SetValue(channelNumber, r);
                    interactionUniverses[universeNumber].SetValue(channelNumber + 1, g);
                    interactionUniverses[universeNumber].SetValue(channelNumber + 2, b);
                    break;
                default:
                    Debug.LogError("Advatek Board has not been set.");
                    break;
            }
            //Debug.Log("normal");
        } else {
            //Debug.Log("difficult");
            for (int i = channelIndex; i < (channelIndex + 3); i++) {
                universeNumber = i / 512;
                channelNumber = i % 512;
                switch (i - channelIndex)
                {
                    case 0:
                        switch (board) {
                            case ADVATEK_BOARD.INSTALLATION1:
                                installation1Universes[universeNumber].SetValue(channelNumber, r);
                                break;
                            case ADVATEK_BOARD.INSTALLATION2:
                                installation2Universes[universeNumber].SetValue(channelNumber, r);
                                break;
                            case ADVATEK_BOARD.INTERACTION:
                                interactionUniverses[universeNumber].SetValue(channelNumber, r);
                                break;
                            default:
                                break;
                        }
                        //Debug.Log("Universe: " + universeNumber + " Channel: " + channelNumber + " R:" + r);
                        break;
                    case 1:
                        switch (board) {
                            case ADVATEK_BOARD.INSTALLATION1:
                                installation1Universes[universeNumber].SetValue(channelNumber, g);
                                break;
                            case ADVATEK_BOARD.INSTALLATION2:
                                installation2Universes[universeNumber].SetValue(channelNumber, g);
                                break;
                            case ADVATEK_BOARD.INTERACTION:
                                interactionUniverses[universeNumber].SetValue(channelNumber, g);
                                break;
                            default:
                                break;
                        }
                        //Debug.Log("Universe: " + universeNumber + " Channel: " + channelNumber + " G:" + g);
                        break;
                    case 2:
                        switch (board) {
                            case ADVATEK_BOARD.INSTALLATION1:
                                installation1Universes[universeNumber].SetValue(channelNumber, b);
                                break;
                            case ADVATEK_BOARD.INSTALLATION2:
                                installation2Universes[universeNumber].SetValue(channelNumber, b);
                                break;
                            case ADVATEK_BOARD.INTERACTION:
                                interactionUniverses[universeNumber].SetValue(channelNumber, b);
                                break;
                            default:
                                break;
                        }
                        //Debug.Log("Universe: " + universeNumber + " Channel: " + channelNumber + " B:" + b);
                        break;
                }
            }
        }


              
    }

    public void NodeUpdateTick() {
        installationNode1.Send();
        installationNode2.Send();
        interactionNode.Send();
    }

    public enum ADVATEK_BOARD {
        INSTALLATION1, INSTALLATION2, INTERACTION
    }
}