using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKiller : MonoBehaviour
{
    [SerializeField] private AudioSource killSoundEffect;
    [SerializeField] private LayerMask enemyLayerMask;

    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rigidbodyPlayer;
    private PlayerLife playerLife;

    //Player Boosters
    private const float MOVEMENTBOOST = 1.5f;
    private const float MOVEMENTLITTLEBOOST = 1.04f;

    //Enemy Boosters
    private const float ENEMYFORCEBOOST = 100f;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        playerLife = GetComponent<PlayerLife>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!playerLife.getDeadState())
            PlayerRayCast();
    }

    private void PlayerRayCast()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
        if (hit.collider != null && hit.distance < 1.2f && hit.collider.CompareTag("Enemy"))
        {
            Rigidbody2D rigidbodyEnemy = hit.collider.GetComponent<Rigidbody2D>();

            //Play the kill sound effect
            killSoundEffect.Play();

            //Physical aftereffects of the player
            if (rigidbodyPlayer.velocity.y < 0f)
            {
                rigidbodyPlayer.velocity = new Vector2(rigidbodyPlayer.velocity.x * MOVEMENTBOOST, Mathf.Abs(rigidbodyPlayer.velocity.y) * MOVEMENTBOOST);
            }                
            else
            {
                rigidbodyPlayer.velocity = new Vector2(rigidbodyPlayer.velocity.x * MOVEMENTLITTLEBOOST, rigidbodyPlayer.velocity.y * MOVEMENTLITTLEBOOST);
            }

            //Physical aftereffects of the enemy
            rigidbodyEnemy.AddForce(rigidbodyEnemy.velocity * ENEMYFORCEBOOST);
            rigidbodyEnemy.gravityScale = 8;
            rigidbodyEnemy.freezeRotation = false;

            //Disable scripts of the enemy
            hit.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            hit.collider.gameObject.GetComponent<EnemyMovement>().enabled = false;

            //Play the death animation
            hit.collider.gameObject.GetComponent<Animator>().SetTrigger("death");

            //Add Killing Score
            ScoreSystem.AddEnemyScore();
        }
    }
}
