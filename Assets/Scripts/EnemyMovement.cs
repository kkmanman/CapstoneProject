using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rigidbodyEnemy;
    private Animator animator;

    [SerializeField] private float moveSpeed = 2f;
    private float moveDirection = 1f;

    private enum MovementState { idle, walk }

    private void Start()
    {
        rigidbodyEnemy = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimationUpdate();

        rigidbodyEnemy.velocity = new Vector2(moveSpeed * moveDirection, rigidbodyEnemy.velocity.y);
        if (IsFacingRight())
        {
            moveDirection = 1f;
        }
        else
        {
            moveDirection = -1f;
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon; //Epsilon = 0.0000...1f
    }

    //When the trigger exits the terrain, this horizontally inverts the scale on the x-axis
    private void OnTriggerExit2D(Collider2D collision)
    {    
        if (collision.CompareTag("Ground"))
        {
            transform.localScale = new Vector2(-Mathf.Sign(rigidbodyEnemy.velocity.x), transform.localScale.y);
        }        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            transform.localScale = new Vector2(-Mathf.Sign(rigidbodyEnemy.velocity.x), transform.localScale.y);
        }
    }

    private void AnimationUpdate()
    {
        MovementState movementState;

        if (moveDirection != 0)
        {
            movementState = MovementState.walk;
        }
        else
        {
            movementState = MovementState.idle;
        }

        animator.SetInteger("movementState", (int)movementState);
    }
}
