using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Rigidbody2D rigidbodyPlatform;
    private Animator animator;

    private float destroyDelay = 5f;

    private void Start()
    {
        rigidbodyPlatform = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("fall");
            rigidbodyPlatform.bodyType = RigidbodyType2D.Dynamic;
            Destroy(gameObject, destroyDelay);
        }
    }
}
