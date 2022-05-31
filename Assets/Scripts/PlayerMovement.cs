using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1.0f;

    [SerializeField] private float dashDistance = 1.0f;
    [SerializeField] private float dashSpeed = 5.0f;

    private float currentDashDistance = 0.0f;
    private Vector2 movementDirection = new Vector2(0, 0);
    private bool isDashing = false;
    private Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Start()
    {
      rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
      if(!isDashing)
      {
        movementDirection.x = Input.GetAxis("Horizontal") * movementSpeed;
        movementDirection.y = Input.GetAxis("Vertical") * movementSpeed;
        movementDirection = Vector2.ClampMagnitude(movementDirection, 1);

        if(Input.GetButton("Dash") && movementDirection != Vector2.zero)
        {
          // if dash button was pressed while movement keys were pressed
          isDashing = true;
          // rigidBody.MovePosition(rigidBody.position + movementDirection * dashSpeed * Time.deltaTime);
          rigidBody.velocity = movementDirection * dashSpeed;
          currentDashDistance += dashSpeed * Time.deltaTime;
        }
        else
        {
          // normal movement without dash
          //rigidBody.MovePosition(rigidBody.position + movementDirection * movementSpeed * Time.deltaTime);
          rigidBody.velocity = movementDirection * movementSpeed;
        }
      }
      else
      {
        if(currentDashDistance < dashDistance)
        {
          // continue dashing
          //rigidBody.MovePosition(rigidBody.position + movementDirection * dashSpeed * Time.deltaTime);
          rigidBody.velocity = movementDirection * dashSpeed;
          currentDashDistance += dashSpeed * Time.deltaTime;
        }
        else
        {
          // finish dashing
          isDashing = false;
          movementDirection = Vector2.zero;
          currentDashDistance = 0.0f;
        }
      }
    }
}
