using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class InteractionController : MonoBehaviour {
    public InteractionData[] lastInteractions = new InteractionData[4];
    public float margin = 1f;
    public List<int> collaborativeInteractions = new List<int>();
    private ArtNetController artNetController;

    private void Awake () {
        artNetController = GameObject.Find("ArtNetController").GetComponent<ArtNetController>();
    }

    public void registerNewInteraction ( InteractionData interactionData ) {
        //TOEVOEGEN, VOOR DEBUG ZO LATEN:
        //if (TimeSyncer.happyFam == true)
        //{
        // Put timesent into array with all timesents at unit-1
        lastInteractions[interactionData.unit - 1] = interactionData;
        collaborativeInteractions.Clear();

        //FOR TESTING
        for (int k = 0; k < 5 * 120; k++) {
            artNetController.SendArtNet(k, 0, 0, 0);
        }
        //


        //Check of deze interactie bijna tegelijk was met een ander
        checkCollab(interactionData);
        //}
    }


    float getLastInteractionTime ( int unit ) {
        return lastInteractions[unit - 1].time;
    }


    public void checkCollab ( InteractionData thisInteraction ) {
        // All units of interactions within margin are put into a list

        foreach (InteractionData value in lastInteractions) {
            if (value != null) {
                if ((value.time >= thisInteraction.time - margin && value.time <= thisInteraction.time + margin)) {
                    collaborativeInteractions.Add(Array.IndexOf(lastInteractions, value));
                }
            }
        }
        Debug.Log("Interaction between " + thisInteraction.unit + " and " + (collaborativeInteractions.Count - 1) + " others.");
        if (collaborativeInteractions.Count > 1) {
            collaboration(collaborativeInteractions);
        }
    }


    public void collaboration ( List<int> interactors ) {
        // For all collaborators turn on Cube + debug
        for (int i = 0; i < interactors.Count; i++) {
            Debug.Log("Collaborators are: " + (interactors[i] + 1));

            //FOR TESTING
            for (int j = interactors[i] * 120; j < interactors[i] * 120 + 120; j++) {
                artNetController.SendArtNet(j, 0, 255, 0);
            }
            //

        }
    }
}