using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] private PauseMenu pauseMenu;

    private AudioSource finishSoundEffect;

    private bool isCompleted = false;

    // Start is called before the first frame update
    private void Start()
    {
        pauseMenu = GameObject.Find("LevelCanvas").GetComponent<PauseMenu>();
        finishSoundEffect = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !isCompleted)
        {            
            finishSoundEffect.Play();
            isCompleted = true;
            ScoreSystem.AddFinishScore();
            Invoke("FinishLevel", 2f); //delays 2 seconds before executing the target function
        }
    }

    private void FinishLevel()
    {
        //SceneManager.LoadScene(getNextActiveScene());        
        pauseMenu.setWinState(true);        
    }

    private int getNextActiveScene()
    {
        ScoreSystem.ResetScore();
        return ((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }
}
