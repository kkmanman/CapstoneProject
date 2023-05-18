using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Component variables
    private Rigidbody2D rigidbodyPlayer;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private Animator animator;

    //Physics variables
    private float directionX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private LayerMask jumpableGround;

    //Animation variables
    private enum MovementState { idle, walk, jump, fall }

    //Jump variables
    private float coyoteTime = 0.5f; //allows the player to jump after leaving the ground within a fixed time
    private float coyoteTimeCounter;
    private float jumpBufferTime = 1f; //allows the player to press jump even before the player lands on the ground
    private float jumpBufferCounter;

    //Sound variables
    [SerializeField] private AudioSource jumpSoundEffect;

    //Particle variable
    [SerializeField] private ParticleSystem dust;

    // Start is called before the first frame update
    private void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        MovementUpdate();
        AnimationUpdate();
    }

    //Movement of the player character and the corresponding inputs
    private void MovementUpdate()
    {
        //Move Horizontally        
        directionX = Input.GetAxisRaw("Horizontal");
        rigidbodyPlayer.velocity = new Vector2(directionX * moveSpeed, rigidbodyPlayer.velocity.y);

        //Jump Upwards
        if (IsGrounded())
        {            
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter = -Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter = -Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            jumpSoundEffect.Play();
            dust.Play();
            rigidbodyPlayer.velocity = new Vector2(rigidbodyPlayer.velocity.x, jumpForce);
                        
            jumpBufferCounter = 0f;
        }

        //allows the player to jump upwards higher by pressing the jump button longer        
        if (Input.GetButtonUp("Jump") && rigidbodyPlayer.velocity.y > 0f) 
        {
            rigidbodyPlayer.velocity = new Vector2(rigidbodyPlayer.velocity.x, rigidbodyPlayer.velocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }
    }

    //Animation of the player character and its trigger conditions
    private void AnimationUpdate()
    {
        MovementState movementState;

        //Check if the player is walking
        if (directionX > 0f)
        {
            movementState = MovementState.walk;
            spriteRenderer.flipX = false; //unflip the player sprite on the X-axis
            //dust.Play();
        }
        else if (directionX < 0f)
        {
            movementState = MovementState.walk;
            spriteRenderer.flipX = true; //flip the player sprite on the X-axis
            //dust.Play();
        }
        else
        {            
            movementState = MovementState.idle;
        }

        //Check if the player is jumping
        if (rigidbodyPlayer.velocity.y > 0.1f) //the velocity of the rigidbody is not 0 even if the player is idle
        {
            movementState = MovementState.jump;
        }
        else if (rigidbodyPlayer.velocity.y < -0.1f)
        {
            movementState = MovementState.fall;
        }

        //Transfer the animation state to the animator 
        animator.SetInteger("movementState", (int)movementState);
    }

    //The new boxcast is slightly below the collider. The boxcast will overlap with the ground
    //so that it can be known that the player object is standing on the ground
    private bool IsGrounded()
    {
        //To create a box around the player object that has the same shape as the box collider
        //This extra box collider can be used to check if something overlaps with it        
        return Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.2f, jumpableGround);        
    }
}
