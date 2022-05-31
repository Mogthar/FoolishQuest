using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DangerTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerController controller = collider.gameObject.GetComponent<PlayerController>();
        if(controller != null)
        {
            controller.KillPlayer();
        }
    }
}
