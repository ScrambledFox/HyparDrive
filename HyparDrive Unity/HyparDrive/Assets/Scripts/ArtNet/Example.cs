using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HauteTechnique.Dmx;
using System;



public class Example : MonoBehaviour
{
    private ArtNetDmxNode node;
    private List<DmxUniverse> universes = new List<DmxUniverse>();
    private int numUniverses = 5;
    public int testLedIndex;

    void Awake()
    {
        node = new ArtNetDmxNode("Broadcast Node", "127.0.0.2");   // 10.0.0.3
        for (int i = 0; i < numUniverses; i++)
        {
            universes.Add(new DmxUniverse(i));
            node.AddUniverse(universes[i]);
        }

    }

    void Update()
    {
        SendArtNet(testLedIndex, 255, 200, 50);
    }

    void SendArtNet(int ledIndex, byte r, byte g, byte b)
    {
        int channelIndex = ledIndex * 3;

        for (int i = channelIndex;i< (channelIndex +3);i++) {
            int universeNumber = i / 512;
            int channelNumber = i % 512;
            switch (i-channelIndex)
            {
                case 0:
                    universes[universeNumber].SetValue(channelNumber, r);
                    Debug.Log("Universe: " + universeNumber + " Channel: " + channelNumber + " R:"+ r);
                    break;
                case 1:
                    universes[universeNumber].SetValue(channelNumber, g);
                    Debug.Log("Universe: " + universeNumber + " Channel: " + channelNumber + " G:" + g);
                    break;
                case 2:
                    universes[universeNumber].SetValue(channelNumber, b);
                    Debug.Log("Universe: " + universeNumber + " Channel: " + channelNumber + " B:" + b);
                    break;
            }
        }        
    }

    void LateUpdate()
    {
        node.Send();
    }
}