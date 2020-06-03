using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ALR_ScoringScreen : MonoBehaviour
{

    // CORN VARIABLES
    private GameObject cCorn;
    private Text counterCorn;
    private GameObject vCorn;
    private Text valueCorn;
    private GameObject sCorn;
    private Text scoreCorn;
    private int cornScore;

    //CACAO VARIABLES
    private GameObject cCacao;
    private Text counterCacao;
    private GameObject vCacao;
    private Text valueCacao;
    private GameObject sCacao;
    private Text scoreCacao;
    private int cacaoScore;

    //DEATH VARIABLES
    private GameObject cDeath;
    private Text counterDeath;
    private GameObject vDeath;
    private Text valueDeath;
    private GameObject sDeath;
    private Text scoreDeath;
    private int deathScore;

    //TIME VARIABLES
    private GameObject cTime;
    private Text counterTime;

    //SCORE VARIABLES
    private GameObject scoreTotal;
    private Text scoreTxt;

    void Start()
    {
        //CORN COUNT
        cCorn = GameObject.Find("CounterCorn");
        counterCorn = cCorn.GetComponent<Text>();
        counterCorn.text = PlayerPrefs.GetInt("nbCorn").ToString();

        vCorn = GameObject.Find("ValueCorn");
        valueCorn = vCorn.GetComponent<Text>();
        valueCorn.text = "X " + PlayerPrefs.GetInt("valueCorn").ToString();

        cornScore = PlayerPrefs.GetInt("nbCorn") * PlayerPrefs.GetInt("valueCorn");
        sCorn = GameObject.Find("ScoreCorn");
        scoreCorn = sCorn.GetComponent<Text>();
        scoreCorn.text = cornScore.ToString();

        //CACAO COUNT
        cCacao = GameObject.Find("CounterCacao");
        counterCacao = cCacao.GetComponent<Text>();
        counterCacao.text = PlayerPrefs.GetInt("nbCacao").ToString();

        vCacao = GameObject.Find("ValueCacao");
        valueCacao = vCacao.GetComponent<Text>();
        valueCacao.text = "X" + PlayerPrefs.GetInt("valueCacao").ToString();

        cacaoScore = PlayerPrefs.GetInt("nbCacao") * PlayerPrefs.GetInt("valueCacao");
        sCacao = GameObject.Find("ScoreCacao");
        scoreCacao = sCacao.GetComponent<Text>();
        scoreCacao.text = cacaoScore.ToString();

        //DEATH COUNT
        cDeath = GameObject.Find("CounterDeath");
        counterDeath = cDeath.GetComponent<Text>();
        if(PlayerPrefs.GetInt("nbDeath")==0)
        {
            counterDeath.text = PlayerPrefs.GetInt("nbDeath").ToString();
        } else
        {
            counterDeath.text = "- " + PlayerPrefs.GetInt("nbDeath").ToString();
        }
       

        vDeath = GameObject.Find("ValueDeath");
        valueDeath = vDeath.GetComponent<Text>();
        valueDeath.text = "X " + PlayerPrefs.GetInt("valueDeath").ToString();

        deathScore = PlayerPrefs.GetInt("nbDeath") * -(PlayerPrefs.GetInt("valueDeath"));
        sDeath = GameObject.Find("ScoreDeath");
        scoreDeath = sDeath.GetComponent<Text>();
        scoreDeath.text = deathScore.ToString();

        cTime = GameObject.Find("CounterTime");
        counterTime = cTime.GetComponent<Text>();

        int totalSeconds = PlayerPrefs.GetInt("seconds");
        Debug.Log("TEMPS EN SECONDES : " + totalSeconds);
        int min;
        int sec;

        if (totalSeconds >= 60)
        {
            min = totalSeconds / 60;
            sec = totalSeconds % 60;
            counterTime.text = min.ToString() + " min " + sec.ToString() + " s";

        } else
        {
            counterTime.text = totalSeconds.ToString() + " s";
        }


        int points = cornScore + cacaoScore + deathScore;
        scoreTotal = GameObject.Find("ScoreTotal");
        scoreTxt = scoreTotal.GetComponent<Text>();
        scoreTxt.text = points.ToString();

    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("Number of Corns : " + PlayerPrefs.GetInt("nbCorn") + " of the individual value of " + PlayerPrefs.GetInt("cornValue"));

        if (Input.GetKey("o"))
        {
            PlayerPrefs.SetInt("nbCorn", 0);
            SceneManager.LoadScene("Level_1");
        }
    }
}
