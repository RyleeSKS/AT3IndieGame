using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//RANDOM NOTE: in blackboard go to 'Unity tutorials' and 'programing in C# with Unity' find 'unityC#programming.pdf
public class testInteration : MonoBehaviour, IInteractable
{
    #region boolean definistion
    private bool exampleBool;

    public bool ExampleBool //{ get; private set; } (another option)
    {
        get { return exampleBool; } //how to make a privately set varriable to be publically read
        //set { exampleBool = value; } (how to make a setter
    }
    #endregion

    public delegate void InteractionDeleagte();
    // you can use events too (they work similarly to Delegate)
    public InteractionDeleagte interactionDeleagte;

    private void OnEnable()
    {
        interactionDeleagte = new InteractionDeleagte(TestMethod);
        interactionDeleagte += TestMethodtwo;
    }

    private void OnDisable()
    {
        interactionDeleagte -= TestMethod;
        interactionDeleagte -= TestMethodtwo;
    }


    /*
    private void Start()
    {
        interactionDeleagte = new InteractionDeleagte(TestMethod);
        interactionDeleagte += TestMethodtwo; //how to call two methods within the same delgate
    }
    */
    public void Activate()
    {
        #region examples
        /*
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
        */
        #endregion

        interactionDeleagte.Invoke();
    }

    private void TestMethod()
    {
        Debug.Log("first method has been exactued");
    }
    private void TestMethodtwo()
    {
        Debug.Log("second method has been exactued");
    }
}
