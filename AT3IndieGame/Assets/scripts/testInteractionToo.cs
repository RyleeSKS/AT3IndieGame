using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testInteractionToo : MonoBehaviour, IInteractable
{
    public testInteration interaction;

    private void Start()
    {
        interaction.interactionDeleagte += TestMethodThree;
    }

    public void Activate()
    {
        Debug.Log("the interaction is currently turned on:" + interaction.ExampleBool);
        interaction.Activate();
    }

    private void TestMethodThree()
    {
        Debug.Log("test method 3 has been activated");
    }
    
}
