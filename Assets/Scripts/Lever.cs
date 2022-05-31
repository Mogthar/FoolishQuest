using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;

public class LeverToggleCommand : Command
{
    private Lever _lever;
    public LeverToggleCommand(Lever lever)
    {
        _lever = lever;
    }

    public override void Execute()
    {
        if(_lever.controllableObject.activeSelf)
        {
            _lever.controllableObject.SetActive(false);
            _lever.transform.localScale = new Vector3(-_lever.transform.localScale.x, 1,1);
        }
        else
        {
            _lever.controllableObject.SetActive(true);
            _lever.transform.localScale = new Vector3(-_lever.transform.localScale.x, 1,1);
        }
    }
}

public class Lever : MonoBehaviour
{

    public Interact interactiveObject;
    public GameObject controllableObject;
    private LeverToggleCommand command;
    // public Vector2 moveVector;
    // Start is called before the first frame update
    void Start()
    {
        command = new LeverToggleCommand(this);
        if(interactiveObject != null)
        {
            interactiveObject.AddCommand(command);
        }
    }

}
