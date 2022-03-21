using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testInteractionToo : MonoBehaviour, IInteractable
{
    public testInteration interaction;

    public void Activate()
    {
        Debug.Log("the interaction is currently turned on:" + interaction.ExampleBool);
        interaction.Activate();
    }

    
}
