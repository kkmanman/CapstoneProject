using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHead : MonoBehaviour
{
    //Serializable variables
    [SerializeField] private AudioSource hitSoundEffect;
    [SerializeField] private float moveSpeed = 0.3f;
    [SerializeField] private float moveAcceleration = 0.1f;
    [SerializeField] private float maxSpeed = 1.0f;
    [SerializeField] private float attackRange = 16f;
    [SerializeField] private float checkDelay = 1f;

    //Component variables
    private Vector2 destination;
    private Vector2 originalPosition;
    private Animator animator;

    //Data variables
    private float originalMoveSpeed;
    private float checkTimer;    
    private bool isAttacking;
    //private enum animationState { idle, hit }

    //List of 4 directions
    private Vector2[] directions = new Vector2[4];

    private void OnEnable()
    {
        Stop();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        originalPosition = transform.position;
        originalMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        if (isAttacking)
        {
            transform.Translate(destination * Time.deltaTime * moveSpeed);
            if (moveSpeed < maxSpeed)
            {
                moveSpeed += moveAcceleration * Time.deltaTime;
            }            
        }
        else
        {
            if (Vector2.Distance(originalPosition, transform.position) > 0.01f)
            {
                Restart();
            }
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
            {
                CheckForPlayer();
            }
        }
        
    }

    private void CheckForPlayer()
    {
        CalculateDirection();

        //Check if spikehead sees the player
        Debug.DrawRay(transform.position, directions[3], Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[3], attackRange);

        if (hit.collider != null && !isAttacking && hit.collider.gameObject.name == "Player" && Vector2.Distance(originalPosition, transform.position) <= 0.01f)
        {
            isAttacking = true;
            destination = directions[3];
            checkTimer = 0;
        }
    }

    private void CalculateDirection()
    {
        //directions[0] = transform.right * attackRange; //Right direction
        //directions[1] = -transform.right * attackRange; //Left direction
        //directions[2] = transform.up * attackRange; //Up direction
        directions[3] = -transform.up * attackRange; //Down direction
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("TimedTrap"))
        {
            if (Vector2.Distance(originalPosition, transform.position) > 0.1f)
            {
                animator.SetBool("hit", true);
                hitSoundEffect.Play();
            }                
            Stop();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        animator.SetBool("hit", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("TimedTrap"))
        {
            animator.SetBool("hit", true);
            hitSoundEffect.Play();
            Stop();
        }        
    }

    private void Stop()
    {        
        destination = transform.position;
        isAttacking = false;
        moveSpeed = originalMoveSpeed;
        //Invoke("Restart", 2);
    }

    private void Restart() => transform.position = Vector2.MoveTowards(transform.position, originalPosition, Time.deltaTime * maxSpeed);
}
