using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{

    public bool isActive = false;
    private bool isUp = false;
    public float speed = 2f;
    public float moveDistance;
    private float currentMoveDistance = 0f;

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            if(currentMoveDistance < moveDistance)
            {
                if(isUp)
                {
                    transform.Translate(new Vector3(0,-speed * Time.deltaTime,0));
                    currentMoveDistance += speed * Time.deltaTime;
                }
                else
                {
                    transform.Translate(new Vector3(0,speed * Time.deltaTime,0));
                    currentMoveDistance += speed * Time.deltaTime;
                }
            }
            else
            {
                isActive = false;
                currentMoveDistance = 0f;
                if(isUp)
                {
                    isUp = false;
                }
                else
                {
                    isUp = true;
                }
            }

        }

    }
}
