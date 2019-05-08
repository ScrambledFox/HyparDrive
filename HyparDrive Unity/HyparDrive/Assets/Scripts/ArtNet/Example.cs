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
		node = new ArtNetDmxNode("Broadcast Node", "127.0.0.1");
        universe6 = new DmxUniverse(6);
        for (int i = 0; i < numUniverses; i++)
        {
            universes.Add(new DmxUniverse(i));
            node.AddUniverse(universes[i]);
        }
		
	}

	void Update()
	{
        universes[4].SetValue(0, 255);      // Universe, Channel, Value
    }

	void LateUpdate()
	{
		node.Send();
	}
}