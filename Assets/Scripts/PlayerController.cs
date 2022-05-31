using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Patterns;

public class Alive : State
{
    private PlayerController _controller;

    public Alive(PlayerController controller)
    {
        _controller = controller;
    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {
        if(_controller.movementEnabled)
        {
            Move();
            Climb();
            Jump();
        }
    }

    void Move()
    {
        _controller.rigidBody.velocity = new Vector2(_controller.movementDirection.x * _controller.movementSpeed, _controller.rigidBody.velocity.y);
    }

    void Jump()
    {
        // need to have another check there for is grounded
        if(_controller.isJumping)
        {
            _controller.rigidBody.AddForce(Vector2.up * _controller.jumpForce, ForceMode2D.Impulse);
        }
        _controller.isJumping = false;
    }

    void Climb()
    {
        if(_controller.boxCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            _controller.rigidBody.velocity = new Vector2(_controller.rigidBody.velocity.x, _controller.movementDirection.y * _controller.climbSpeed);
            _controller.rigidBody.gravityScale = 0f;
        }
        else
        {
            _controller.rigidBody.gravityScale = _controller.initialGravity;
        }
    }
}

public class Dead : State
{
    private PlayerController _controller;

    public Dead(PlayerController controller)
    {
        _controller = controller;
    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {
        if(_controller.movementEnabled)
        {
            Move();
        }
    }

    public override void Enter()
    {
        _controller.spriteRenderer.sprite = _controller.ghostSprite;
        _controller.rigidBody.mass = 0f;
        _controller.rigidBody.gravityScale = 0f;
        _controller.capsuleCollider.size = new Vector2(0.74f, 0.87f);
        _controller.capsuleCollider.offset = new Vector2(0.0f, 0.1f);
        _controller.boxCollider.size = new Vector2(0.1f, 0.1f);
    }

    void Move()
    {
        _controller.rigidBody.velocity = new Vector2(_controller.movementDirection.x * _controller.movementSpeed, _controller.movementDirection.y * _controller.movementSpeed);
    }

    // need  on enter method to disable gravity

}

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // movement properties
    // in the future find a way of making these private
    // probably define getters
    public float movementSpeed = 1.0f;
    public float climbSpeed = 0.6f;
    public float jumpForce = 5.0f;
    public Vector2 movementDirection = new Vector2(0,0);
    public float levitationGravity = 9.81f;

    public bool isJumping = false;
    public bool canJump = false;
    public bool movementEnabled = true;
    private bool isAlive = true;

    // components of the player object
    public Rigidbody2D rigidBody;
    public CapsuleCollider2D capsuleCollider;
    public BoxCollider2D boxCollider;
    public SpriteRenderer spriteRenderer;

    public Sprite ghostSprite;

    public float initialGravity;

    public GameController gameController;

    // state of the player i.e. alive, ghost
    public enum playerState
    {
        ALIVE = 0,
        DEAD = 1,
    }

    public enum jumpMode
    {
        GROUND_ONLY = 0,
        GROUND_AND_ROPE = 1,
    }

    public jumpMode playerJumpMode = jumpMode.GROUND_ONLY;
    public FSM playerFSM = new FSM();

    void Start()
    {
      rigidBody = GetComponent<Rigidbody2D>();
      capsuleCollider = GetComponent<CapsuleCollider2D>();
      boxCollider = GetComponent<BoxCollider2D>();
      spriteRenderer = GetComponent<SpriteRenderer>();
      playerFSM.AddState((int)playerState.ALIVE, new Alive(this));
      playerFSM.AddState((int)playerState.DEAD, new Dead(this));
      playerFSM.SetCurrentState(playerFSM.GetState((int)playerState.ALIVE));

      initialGravity = rigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerFSM != null)
        {
            playerFSM.Update();
        }
    }

    void FixedUpdate()
    {
        if(playerFSM != null)
        {
            playerFSM.FixedUpdate();
        }
        FlipSprite();
    }

    void OnMove(InputValue value)
    {
        movementDirection = value.Get<Vector2>();
      // movementDirection = Vector2.ClampMagnitude(movementDirection, 1);
    }

    // implemented a choice of jumping mode via enums
    void OnJump(InputValue value)
    {
        switch(playerJumpMode)
        {
            case jumpMode.GROUND_ONLY:
                if(boxCollider.IsTouchingLayers(LayerMask.GetMask("Platforms")) || boxCollider.IsTouchingLayers(LayerMask.GetMask("Interactables")))
                {
                    isJumping = true;
                }
                break;
            case jumpMode.GROUND_AND_ROPE:
                if(boxCollider.IsTouchingLayers(LayerMask.GetMask("Platforms")) || boxCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")) || boxCollider.IsTouchingLayers(LayerMask.GetMask("Interactables")))
                {
                    isJumping = true;
                }
                break;
        }
    }

    void FlipSprite()
    {
        bool isMovingHorizontally = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
        if(isMovingHorizontally)
        {
            transform.localScale = new  Vector2 (Mathf.Sign(rigidBody.velocity.x), 1f);
        }
    }

    public void KillPlayer()
    {
        playerFSM.SetCurrentState(playerFSM.GetState((int)playerState.DEAD));
        isAlive = false;
    }

    void OnMenu(InputValue value)
    {
        if(!gameController.isMenuActive)
        {
            gameController.ActivateMenu();
            movementEnabled = false;
        }
        else
        {
            gameController.DisableMenu();
            movementEnabled = true;
            isJumping = false;
        }

    }

    void OnRestartLevel(InputValue value)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnInteract(InputValue value)
    {
        if(isAlive)
        {
            List<Collider2D> overlaps = new List<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(LayerMask.GetMask("Interactables"));
            filter.useTriggers = true;
            capsuleCollider.OverlapCollider(filter, overlaps);
            for(int i = 0; i < overlaps.Count; i++)
            {
                Interact interactiveObject = overlaps[i].GetComponent<Interact>();
                if(interactiveObject != null)
                {
                    interactiveObject.Interaction();
                }
            }
        }
    }

    void OnLevitate(InputValue value)
    {
        if(!isAlive)
        {
            float modifier = value.Get<float>();
            Physics2D.gravity = new Vector2(0, -9.81f + modifier * (9.81f + levitationGravity));
        }
    }

}
