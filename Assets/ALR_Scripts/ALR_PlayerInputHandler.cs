using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(ALR_CustomCharacterController))]

public class ALR_PlayerInputHandler : MonoBehaviour
{

    private ALR_CustomCharacterController charac;
    private ALR_CharacterData cData;
    private AXD_PlayerStatus pStatus;
    public ALR_DialogueTrigger dTrigger;
    public ALR_DialogueManager dManager;
    public AXD_ScoreManager sManager;
    private ALR_MenuInputHandler mInput;
    private ALR_DebugCheckPoints dCheckPoints;
    private ALR_SoundManager soundManager;

    [SerializeField] ALR_FountainSound[] arrFoutainSound;

    public float translation;
    bool checkingOnAir = false;
    bool isbufferedJumping = false;
    bool isGhostJumping = false;
    public bool talkingToNPC = false;
    public bool isAlreadyTalking = false;
    public bool endingDialogue = false;
    private bool dialJustEnded = false;
    public bool makeOffering = false;
    private bool isPauseMenu = false;
    public bool quitPauseMenu = false;
    private bool menuJustQuit = false;
    public bool switchDisable = true;
    public bool lockInput = false;
    float timeSinceJumpInput;
    float timeCheckGhostJump;

    void Start()
    {
        pStatus = GetComponent<AXD_PlayerStatus>();
        charac = GetComponent<ALR_CustomCharacterController>();
        cData = GetComponent<ALR_CharacterData>();
        sManager = FindObjectOfType<AXD_ScoreManager>();
        mInput = FindObjectOfType<ALR_MenuInputHandler>();
        dCheckPoints = FindObjectOfType<ALR_DebugCheckPoints>();
        soundManager = GetComponent<ALR_SoundManager>();

    }


    void Update()
    {
        
        translation = Input.GetAxis("Horizontal");
        if (!lockInput)
        {
            charac.Walk(translation);
        }

        if (checkingOnAir)
        {
            if (!charac.collisions.onGround)
            {
                checkingOnAir = false;
                isbufferedJumping = true;
            }


        }

        if (isbufferedJumping)
        {
            timeSinceJumpInput += Time.fixedDeltaTime;

            if (charac.collisions.onGround == true && timeSinceJumpInput <= cData.maxBufferedJump)
            {
                if (charac.jumped == false)
                {
                    soundManager.JumpSound();
                }
                charac.Jump();
                charac.jumped = true;
                timeSinceJumpInput = 0f;
                isbufferedJumping = false;

            }
            else if (timeSinceJumpInput > cData.maxBufferedJump)
            {
                timeSinceJumpInput = 0f;
                isbufferedJumping = false;
            }
        }

        if (charac.collisions.below == false)
        {

            timeCheckGhostJump += Time.fixedDeltaTime;

            if (timeCheckGhostJump <= cData.maxGhostJump && !charac.jumped)
            {
                charac.isGhostJumping = true;
            }
            else
            {
                charac.isGhostJumping = false;
            }

        }
        else
        {
            timeCheckGhostJump = 0;
        }


        if (endingDialogue)
        {
            isAlreadyTalking = false;
            talkingToNPC = false;
            endingDialogue = false;
            dialJustEnded = true;
        }
        
        if(quitPauseMenu == true)
        {
            Debug.Log("ICIIII !");
            quitPauseMenu = false;
            menuJustQuit = true;
            isPauseMenu = false;

        }

        if ((Input.GetKeyDown("joystick button 0") || Input.GetKeyDown("space")) && talkingToNPC == false && makeOffering == false)
        {
            if (dialJustEnded == true)
            {
                dialJustEnded = false;
                mInput.TimeStop(false);
            } else if (menuJustQuit == true)
            {
                menuJustQuit = false;
                mInput.TimeStop(false);
 
            }
            else
            {
                if (!lockInput)
                {
                    if(charac.jumped == false)
                    {
                        soundManager.JumpSound();
                    }

                    charac.Jump();
                    charac.jumped = true;

  
                    if (checkingOnAir == false && charac.collisions.onGround)
                    {
                        checkingOnAir = true;
                    }

                    if (!charac.collisions.onGround && isbufferedJumping == false)
                    {
                        checkingOnAir = false;
                        isbufferedJumping = true;
                    }

                    if (isbufferedJumping && !charac.collisions.onGround)
                    {
                        timeSinceJumpInput = 0f;

                    }
                }



            }


        }


        if ((Input.GetKeyDown("joystick button 0") || Input.GetKeyDown("space")) && talkingToNPC == true && isAlreadyTalking == false)
        {
            isAlreadyTalking = true;
            charac.interactionNPC.SetActive(false);
            charac.interactionNPC1.SetActive(false);
            mInput.TimeStop(true);
            dTrigger.TriggerDialogue();
            if (dTrigger.nbNPC == 2)
            {
                switchDisable = false;
            }
        }

        if ((Input.GetKeyDown("joystick button 0") || Input.GetKeyDown("space")) && makeOffering == true)
        {
            PlayerPrefs.SetInt("nbCorn", pStatus.Corn);
            PlayerPrefs.SetInt("valueCorn", sManager.cornValue);

            PlayerPrefs.SetInt("nbCacao", pStatus.Cacao);
            PlayerPrefs.SetInt("valueCacao", sManager.cacaoValue);

            PlayerPrefs.SetInt("nbDeath", pStatus.deaths);
            PlayerPrefs.SetInt("valueDeath", sManager.deathValue);

            var sec = AXD_TimeManager.GetSeconds();
            PlayerPrefs.SetInt("seconds", sec);
            sManager.CalculateScore();
            var score = AXD_ScoreManager.GetScore();
            PlayerPrefs.SetInt("score", score);
            sManager.UpdateHighScore();
            SceneManager.LoadScene("Menu_Score");
        }


        if ((Input.GetKeyUp("joystick button 0") || Input.GetKeyUp("space")))
        {
            //Debug.Log("STOP JUMP ! ");
            charac.EndJump();
        }

        // POUR LE SWITCH DE MONDE

        //World Switch = Right Bumper or Left Bumper
        if ((Input.GetButtonDown("World Switch") || (Input.GetKeyDown("left shift"))) && switchDisable == false)
        {
            pStatus.ChangeWorld();
            soundManager.SwitchingWorld();

            for (int i = 0; i < arrFoutainSound.Length; i++)
            {
                
                arrFoutainSound[i].playFountainSound(pStatus.LivingWorld);
            }
        }

        if (Input.GetButtonDown("Pause Menu") && isAlreadyTalking == false)
        {
            if (isPauseMenu == false)
            {
                isPauseMenu = true;
                mInput.ActivatePause();
            }
            else
            {
                
                isPauseMenu = false;
                mInput.DeactivatePause();
               
            }
            //isPaused = !isPaused;
        }


        if (Input.GetKeyDown("i"))
        {
            dCheckPoints.NextCheckPoint();
        }

        if (Input.GetKeyDown("u"))
        {
            dCheckPoints.PreviousCheckPoint();
        }

        // Charger l'écran des scores ou high score ==> DEBUG

        if (Input.GetKeyDown("h"))
        {
            sManager.UpdateHighScore();
            SceneManager.LoadScene("Menu_HighScore");
        }

        if (Input.GetKeyDown("j"))
        {
            PlayerPrefs.DeleteKey("HS_One");
            PlayerPrefs.DeleteKey("HS_Two");
            PlayerPrefs.DeleteKey("HS_Three");
            PlayerPrefs.DeleteKey("HS_Four");
            PlayerPrefs.DeleteKey("HS_Five");
        }

        if (Input.GetKey("o"))
        {

            PlayerPrefs.SetInt("nbCorn", pStatus.Corn);
            PlayerPrefs.SetInt("valueCorn", sManager.cornValue);

            PlayerPrefs.SetInt("nbCacao", pStatus.Cacao);
            PlayerPrefs.SetInt("valueCacao", sManager.cacaoValue);

            PlayerPrefs.SetInt("nbDeath", pStatus.deaths);
            PlayerPrefs.SetInt("valueDeath", sManager.deathValue);

            var sec = AXD_TimeManager.GetSeconds();
            PlayerPrefs.SetInt("seconds", sec);
            sManager.CalculateScore();

            var score = AXD_ScoreManager.GetScore();
            PlayerPrefs.SetInt("score", score);
            SceneManager.LoadScene("Menu_Score");
        }
    }
}
