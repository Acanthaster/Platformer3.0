using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_ScoreManager : MonoBehaviour
{
    private static AXD_ScoreManager instance;
    public AXD_PlayerStatus pStatus;
    private static int score;
    private static int highScore;

    private static int HS_One;
    private static int HS_Two;
    private static int HS_Three;
    private static int HS_Four;
    private static int HS_Five;

    private int[] arrHighScore = new int[5] ;

    public int cornValue;
    public int cacaoValue;
    public int deathValue;
    public List<int> levels;
    public List<int> levelsValue;

    private void Awake()
    {
        score = 0;
        instance = this;

            if (!PlayerPrefs.HasKey("HS_One"))
        {
            
            //Debug.Log("Yop !");
            PlayerPrefs.SetInt("HS_One", 0);
            PlayerPrefs.SetInt("HS_Two", 0);
            PlayerPrefs.SetInt("HS_Three", 0);
            PlayerPrefs.SetInt("HS_Four", 0);
            PlayerPrefs.SetInt("HS_Five", 0);

            arrHighScore[0] = PlayerPrefs.GetInt("HS_One");
            arrHighScore[1] = PlayerPrefs.GetInt("HS_Two");
            arrHighScore[2] = PlayerPrefs.GetInt("HS_Three");
            arrHighScore[3] = PlayerPrefs.GetInt("HS_Four");
            arrHighScore[4] = PlayerPrefs.GetInt("HS_Five");

        } else
        {
            arrHighScore[0] = PlayerPrefs.GetInt("HS_One");
            arrHighScore[1] = PlayerPrefs.GetInt("HS_Two");
            arrHighScore[2] = PlayerPrefs.GetInt("HS_Three");
            arrHighScore[3] = PlayerPrefs.GetInt("HS_Four");
            arrHighScore[4] = PlayerPrefs.GetInt("HS_Five");
        }


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
        //Debug.Log("Score Corn : " + score);
        score += GetCacaoScore();
        //Debug.Log("Score Cacao : " + score);
        /*score += GetTimeBonusScore();
        Debug.Log("Score Time : " + score);*/
        score += GetDeathMalus();
        //Debug.Log("Score Death : " + score);
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
        while (i <= levels.Count && i != -1)
        {
            if (AXD_TimeManager.GetSeconds() < levels[i])
            {
                bonus = levelsValue[i];
                i = -2;
            }
            i++;
        }
        //Debug.Log("Bonus : " + bonus);
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

    public void UpdateHighScore()
    {
        for (int i= 0; i < 5; i++)
        {
            //Debug.Log(" Before : " + arrHighScore[i]);

            if(score >= arrHighScore[i])
            {
                /*savedHS = arrHighScore[i];
                savedIndex = i;
                arrHighScore[i] = score;*/

                switch (i)
                {
                    case 0:

                        PlayerPrefs.SetInt("HS_One", score);
                        PlayerPrefs.SetInt("HS_Two", arrHighScore[0]);
                        PlayerPrefs.SetInt("HS_Three", arrHighScore[1]);
                        PlayerPrefs.SetInt("HS_Four", arrHighScore[2]);
                        PlayerPrefs.SetInt("HS_Five", arrHighScore[3]);

                        arrHighScore[0] = PlayerPrefs.GetInt("HS_One");
                        arrHighScore[1] = PlayerPrefs.GetInt("HS_Two");
                        arrHighScore[2] = PlayerPrefs.GetInt("HS_Three");
                        arrHighScore[3] = PlayerPrefs.GetInt("HS_Four");
                        arrHighScore[4] = PlayerPrefs.GetInt("HS_Five");

                        break;

                    case 1:

                        PlayerPrefs.SetInt("HS_Two", score);
                        PlayerPrefs.SetInt("HS_Three", arrHighScore[1]);
                        PlayerPrefs.SetInt("HS_Four", arrHighScore[2]);
                        PlayerPrefs.SetInt("HS_Five", arrHighScore[3]);

                        arrHighScore[0] = PlayerPrefs.GetInt("HS_One");
                        arrHighScore[1] = PlayerPrefs.GetInt("HS_Two");
                        arrHighScore[2] = PlayerPrefs.GetInt("HS_Three");
                        arrHighScore[3] = PlayerPrefs.GetInt("HS_Four");
                        arrHighScore[4] = PlayerPrefs.GetInt("HS_Five");
                        break;

                    case 2:

                        PlayerPrefs.SetInt("HS_Three", score);
                        PlayerPrefs.SetInt("HS_Four", arrHighScore[2]);
                        PlayerPrefs.SetInt("HS_Five", arrHighScore[3]);

                        arrHighScore[0] = PlayerPrefs.GetInt("HS_One");
                        arrHighScore[1] = PlayerPrefs.GetInt("HS_Two");
                        arrHighScore[2] = PlayerPrefs.GetInt("HS_Three");
                        arrHighScore[3] = PlayerPrefs.GetInt("HS_Four");
                        arrHighScore[4] = PlayerPrefs.GetInt("HS_Five");
                        break;

                    case 3:


                        PlayerPrefs.SetInt("HS_Four", score);
                        PlayerPrefs.SetInt("HS_Five", arrHighScore[3]);

                        arrHighScore[0] = PlayerPrefs.GetInt("HS_One");
                        arrHighScore[1] = PlayerPrefs.GetInt("HS_Two");
                        arrHighScore[2] = PlayerPrefs.GetInt("HS_Three");
                        arrHighScore[3] = PlayerPrefs.GetInt("HS_Four");
                        arrHighScore[4] = PlayerPrefs.GetInt("HS_Five");
                        break;

                    case 4:

                        PlayerPrefs.SetInt("HS_Five", score);

                        arrHighScore[0] = PlayerPrefs.GetInt("HS_One");
                        arrHighScore[1] = PlayerPrefs.GetInt("HS_Two");
                        arrHighScore[2] = PlayerPrefs.GetInt("HS_Three");
                        arrHighScore[3] = PlayerPrefs.GetInt("HS_Four");
                        arrHighScore[4] = PlayerPrefs.GetInt("HS_Five");
                        break;

                    default:
                    break;

                }

                break;
            }


        }

        
    }

    public static void ClearHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
    }
}
