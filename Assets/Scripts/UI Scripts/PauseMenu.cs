using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    public static bool isGameOver = false;
    public static bool isGameWin = false;

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject gameOverMenuUI;
    [SerializeField] private GameObject winMenuUI;
    [SerializeField] private GameObject GameHUD;

    [SerializeField] private Text gameOverScoreText;
    [SerializeField] private Text gameOverBestScoreText;
    [SerializeField] private Text winScoreText;
    [SerializeField] private Text winBestScoreText;

    // Update is called once per frame
    public void Update()
    {
        if (!isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
            {
                if (isGamePaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }        

        gameOverScoreText.text = "Score: " + ScoreSystem.getScore().ToString("F2");
        gameOverBestScoreText.text = "Best Score: " + ScoreSystem.getBestScore().ToString("F2");
        winScoreText.text = "Score: " + ScoreSystem.getScore().ToString("F2");
        winBestScoreText.text = "Best Score: " + ScoreSystem.getBestScore().ToString("F2");

        if (isGameWin)
        {
            winMenuUI.SetActive(true);
            GameHUD.SetActive(false);
            Time.timeScale = 0f;
            isGamePaused = true;
        }
    }

    public void setGameOVerState(bool state)
    {
        isGameOver = state;
    }

    public void setPauseState(bool state)
    {
        isGamePaused = state;
    }

    public void setWinState(bool state)
    {
        isGameWin = state;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameHUD.SetActive(true);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameHUD.SetActive(false);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void GameOver()
    {
        gameOverMenuUI.SetActive(true);
        GameHUD.SetActive(false);
        Time.timeScale = 0f;
        isGameOver = true;
        isGamePaused = true;
    }

    public void RestartLevel()
    {        
        Time.timeScale = 1f;
        isGameOver = false;
        isGamePaused = false;
        isGameWin = false;
        ScoreSystem.ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameOverMenuUI.SetActive(false);
        winMenuUI.SetActive(false);
        GameHUD.SetActive(true);
    }

    public void LevelExit()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        isGamePaused = false;    
        isGameWin = false;
        SceneManager.LoadScene(0);
        ScoreSystem.ResetScore();
    }
}
