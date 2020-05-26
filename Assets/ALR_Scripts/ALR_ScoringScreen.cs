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

    void Start()
    {
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
