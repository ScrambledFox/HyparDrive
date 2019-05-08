using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HauteTechnique.Dmx;

public class Example : MonoBehaviour 
{
	private ArtNetDmxNode node;
	private List<DmxUniverse> universes = new List<DmxUniverse>();
    private DmxUniverse universe6;
    private int numUniverses = 5; 

	void Awake()
	{
		node = new ArtNetDmxNode("Broadcast Node", "10.0.0.3");
        for (int i = 0; i < numUniverses; i++)
        {
            universes.Add(new DmxUniverse(i));
            node.AddUniverse(universes[i]);
        }
		
	}

    void Update()
    {
        for (int i = 0; i < numUniverses; i++)
        {
            for (int j = 0; j < 360; j+=3)
            {
                universes[i].SetValue(j, 255);      // Universe, Channel, Value
                universes[i].SetValue(j+1, 100);      // Universe, Channel, Value
            }
        }
    }

    void SendArtNet(int ledIndex, byte r, byte g, byte b)
	{
        int universeNumber = 0;
        if (ledIndex <= 512)
        {
            universes[universeNumber].SetValue(ledIndex, r);
            if (ledIndex + 1 <= 512)
            {
                universes[universeNumber].SetValue(ledIndex + 1, g);
                if (ledIndex + 2 <= 512)
                {
                    universes[universeNumber].SetValue(ledIndex + 1, b);
                }
                else
                {
                    universes[universeNumber + 1].SetValue(1, b);
                }
            }
            else
            {
                universes[universeNumber+1].SetValue(1, g);
                universes[universeNumber + 1].SetValue(2, b);
            }
        } else
        {
            Debug.Log("Channel number too high");
        }
        //LateUpdate();
    }

	void LateUpdate()
	{
		node.Send();
	}
}