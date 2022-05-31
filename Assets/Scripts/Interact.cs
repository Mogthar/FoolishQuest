using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;

public class Interact : MonoBehaviour
{
    public List<Command> commandList = new List<Command>();

    public void AddCommand(Command command)
    {
        commandList.Add(command);
    }

    public void Interaction()
    {
        for(int i = 0; i < commandList.Count; i++)
        {
            commandList[i].Execute();
        }
    }
}
