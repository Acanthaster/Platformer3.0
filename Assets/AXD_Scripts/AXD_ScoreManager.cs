using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_ScoreManager : MonoBehaviour
{
    private static AXD_ScoreManager instance;
    public AXD_PlayerStatus pStatus;
    private static int score;
    private static int highScore;
    public int cornValue;
    public int cacaoValue;
    public int deathValue;
    public List<int> levels;
    public List<int> levelsValue;

    private void Awake()
    {
        score = 0;
        instance = this;
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CalculateScore();
    }

    public void CalculateScore()
    {
        score = GetCornScore();
        score += GetCacaoScore();
        score += GetTimeBonusScore();
        score += GetDeathMalus();
    }

    public int GetCornScore()
    {
        return pStatus.Corn * cornValue;
    }

    public int GetCacaoScore()
    {
        return pStatus.Cacao * cacaoValue;
    }

    public int GetTimeBonusScore()
    {
        int bonus = 0;
        int i = 0;
        while (i < levels.Count && i != -1)
        {
            if (AXD_TimeManager.GetSeconds() < levels[i])
            {
                bonus = levelsValue[i];
                i = -2;
            }
            i++;
        }

        return bonus;
    }

    public int GetDeathMalus()
    {
        return -deathValue * pStatus.deaths;
    }


    public static int GetScore()
    {
        return score;
    }

    public static int GetHighScore()
    {
        return highScore;
    }

    public static void UpdateHighScore()
    {
        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    public static void ClearHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
    }
}
