using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ALR_HighScoringScreen : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject nbOne;
    private Text txtOne; 

    private GameObject nbTwo;
    private Text txtTwo;

    private GameObject nbThree;
    private Text txtThree;


    private GameObject nbFour;
    private Text txtFour;


    private GameObject nbFive;
    private Text txtFive;


    void Start()
    {

        nbOne = GameObject.Find("hsOne");
        txtOne = nbOne.GetComponent<Text>();
        txtOne.text = PlayerPrefs.GetInt("HS_One").ToString();

        nbTwo = GameObject.Find("hsTwo");
        txtTwo = nbTwo.GetComponent<Text>();
        txtTwo.text = PlayerPrefs.GetInt("HS_Two").ToString();

        nbThree = GameObject.Find("hsThree");
        txtThree = nbThree.GetComponent<Text>();
        txtThree.text = PlayerPrefs.GetInt("HS_Three").ToString();

        nbFour = GameObject.Find("hsFour");
        txtFour = nbFour.GetComponent<Text>();
        txtFour.text = PlayerPrefs.GetInt("HS_Four").ToString();

        nbFive = GameObject.Find("hsFive");
        txtFive = nbFive.GetComponent<Text>();
        txtFive.text = PlayerPrefs.GetInt("HS_Five").ToString();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey("h"))
        {
            SceneManager.LoadScene("Level_1");
        }

    }
}
