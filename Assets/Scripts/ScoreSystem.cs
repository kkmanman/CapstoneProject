using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private static float collectibleScore = 25f;
    [SerializeField] private static float enemyScore = 50f;
    [SerializeField] private static float finishScore = 200f;
    [SerializeField] private static float timeBonusScore = 1000000f;

    private static float score = 0;
    private static float theBestScore = 0;
    private static float timeSeconds = 0;

    private Text scoreText;
    [SerializeField] private Text timeText;

    public static float getBestScore()
    {
        if (PlayerPrefs.HasKey("bestScore"))
        {
            if (PlayerPrefs.GetFloat("bestScore") < score)
            {
                PlayerPrefs.SetFloat("bestScore", score);
                theBestScore = score;
            }
            else
            {
                theBestScore = PlayerPrefs.GetFloat("bestScore");
            }
        }
        else
        {
            PlayerPrefs.SetFloat("bestScore", 0f);
            theBestScore = 0f;
        }

        return theBestScore;
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        getBestScore();
    }

    // Update is called once per frame
    void Update()
    {        
        if (!PauseMenu.isGamePaused)
        {
            timeSeconds += Time.deltaTime;
        }        
        timeText.text = "Time: " + (Mathf.Round(timeSeconds * 100f) / 100f).ToString("F2") + "s";
        scoreText.text = "Score: " + score.ToString("F0");
    }

    private static void CalculateTimeBonusScore(float completionTime)
    {
        score += timeBonusScore * Mathf.Exp(-1.00001f * completionTime);       
    }

    public static float getScore()
    {
        return score;
    }

    public static void AddFinishScore()
    {
        score += finishScore;
        CalculateTimeBonusScore(timeSeconds);
    }

    public static void AddCollectableScore()
    {
        score += collectibleScore;
    }

    public static void AddEnemyScore()
    {
        score += enemyScore;
    }

    public static void ResetScore()
    {
        score = 0;
        timeSeconds = 0;
    }
}
