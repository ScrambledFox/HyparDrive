using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HauteTechnique.Dmx;
using System;

public class ArtNetController : MonoBehaviour
{
    private ArtNetDmxNode node;
    private List<DmxUniverse> universes = new List<DmxUniverse>();
    private int numUniverses = 5;
    public int testLedIndex;

    public void Awake()
    {
        node = new ArtNetDmxNode("Broadcast Node", "127.0.0.2");   // Advatek IP: 10.0.0.3
        for (int i = 0; i < numUniverses; i++)
        {
            universes.Add(new DmxUniverse(i));
            node.AddUniverse(universes[i]);
        }        
    }


    public void SendArtNet(int ledIndex, byte r, byte g, byte b)
    {
        int channelIndex = ledIndex * 3;
        int universeNumber = channelIndex / 512;
        int channelNumber = channelIndex % 512;
        if ((channelIndex%512) <= 509)
        {
            universes[universeNumber].SetValue(channelNumber, r);
            universes[universeNumber].SetValue(channelNumber+1, g);
            universes[universeNumber].SetValue(channelNumber+2, b);
            //Debug.Log("normal");
        } else
        {
            //Debug.Log("difficult");
            for (int i = channelIndex; i < (channelIndex + 3); i++)
            {
                universeNumber = i / 512;
                channelNumber = i % 512;
                switch (i - channelIndex)
                {
                    case 0:
                        universes[universeNumber].SetValue(channelNumber, r);
                        //Debug.Log("Universe: " + universeNumber + " Channel: " + channelNumber + " R:" + r);
                        break;
                    case 1:
                        universes[universeNumber].SetValue(channelNumber, g);
                        //Debug.Log("Universe: " + universeNumber + " Channel: " + channelNumber + " G:" + g);
                        break;
                    case 2:
                        universes[universeNumber].SetValue(channelNumber, b);
                        //Debug.Log("Universe: " + universeNumber + " Channel: " + channelNumber + " B:" + b);
                        break;
                }
            }
        }


              
    }

    public void LateUpdate()
    {
        node.Send();
    }
}