using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidbodyPlayer;

    public bool isDead = false;

    [SerializeField] private AudioSource dieSoundEffect;
    [SerializeField] private GameObject gameOverMenuUI;
    [SerializeField] private GameObject GameHUD;
    [SerializeField] private PauseMenu pauseMenu;

    // Start is called before the first frame update
    private void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    //Player will die from a list of objects in the following
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDead)
            //Player touches traps
            if (collision.gameObject.CompareTag("Trap") || collision.gameObject.CompareTag("Enemy"))
            {
                isDead = true;
                Dead();
            }
    }

    private void Update()
    {
        //Player falls out of bounds
        if (transform.position.y < -100f && !isDead)
        {
            isDead = true;
            Dead();
        }
    }

    public bool getDeadState()
    {
        return isDead;
    }

    //Player dies
    public void Dead()
    {
        dieSoundEffect.Play();
        //this can make the player object unable to move
        //and physics will not apply to the object anymore
        rigidbodyPlayer.bodyType = RigidbodyType2D.Static; 
        animator.SetTrigger("death");
    }

    private void GameOver()
    {
        gameOverMenuUI.SetActive(true);
        GameHUD.SetActive(false);
        Time.timeScale = 0f;
        pauseMenu.setPauseState(true);
        pauseMenu.setGameOVerState(true);
    }

    private void RestartLevel()
    {
        ScoreSystem.ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
