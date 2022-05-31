using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;

public class LeverMoveCommand : Command
{
    private LeverMove _lever;
    public LeverMoveCommand(LeverMove lever)
    {
        _lever = lever;
    }

    public void Toggle()
    {
        PlatformMove platform = _lever.controllableObject.GetComponent<PlatformMove>();
        if(platform != null)
        {
            platform.isActive = true;
        }
    }

    public override void Execute()
    {
        Toggle();
    }
}

public class LeverMove : MonoBehaviour
{

    public Interact interactiveObject;
    public GameObject controllableObject;
    private LeverMoveCommand command;
    public float delay = 3f;
    // public Vector2 moveVector;
    // Start is called before the first frame update
    void Start()
    {
        command = new LeverMoveCommand(this);
        if(interactiveObject != null)
        {
            interactiveObject.AddCommand(command);
        }
    }

}
