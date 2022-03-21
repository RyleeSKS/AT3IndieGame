using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//RANDOM NOTE: in blackboard go to 'Unity tutorials' and 'programing in C# with Unity' find 'unityC#programming.pdf
public class testInteration : MonoBehaviour, IInteractable
{
    private bool exampleBool;

    public bool ExampleBool //{ get; private set; } (another option)
    {
        get { return exampleBool; } //how to make a privately set varriable to be publically read
        //set { exampleBool = value; } (how to make a setter
    }

    public void Activate()
    {
        //ExampleBool = true; (how to make a setter)
        exampleBool = !exampleBool;
        if(exampleBool == true)
        {
            Debug.Log("Case a.");
        }
        else
        {
            Debug.Log("Case z.");
        }
        
    }
}
