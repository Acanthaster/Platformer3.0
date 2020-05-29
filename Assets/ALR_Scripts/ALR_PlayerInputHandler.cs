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

    bool checkingOnAir = false;
    bool isbufferedJumping = false;
    bool isGhostJumping = false;
    public bool talkingToNPC = false;
    public bool isAlreadyTalking = false;
    public bool endingDialogue = false;
    private bool dialJustEnded = false;
    public bool makeOffering = false;

    float timeSinceJumpInput;
    float timeCheckGhostJump;

    void Start()
    {
        pStatus = GetComponent<AXD_PlayerStatus>();
        charac = GetComponent<ALR_CustomCharacterController>();
        cData = GetComponent<ALR_CharacterData>();
        sManager = FindObjectOfType<AXD_ScoreManager>();

    }

  
    void Update()
    {
        float translation = Input.GetAxis("Horizontal");
        charac.Walk(translation);

        if(checkingOnAir)
        {
      

            if(charac.collisions.onGround)
            {
                //Debug.Log("Pas dans les airs !");

            } 
            
            else if (!charac.collisions.onGround )
            {
                //Debug.Log("Buffering !");
                checkingOnAir = false;
                isbufferedJumping = true;
            }

         
        }

        if(isbufferedJumping)
        {
            timeSinceJumpInput += Time.fixedDeltaTime;
            //Debug.Log("Time Since Input :" + Time.fixedDeltaTime);

            if(charac.collisions.onGround == true && timeSinceJumpInput <= cData.maxBufferedJump)
            {
                charac.Jump();
                charac.jumped = true;
                timeSinceJumpInput = 0f;
                isbufferedJumping = false;
                //Debug.Log("Buffered Jump !");

            } 
            
            else if (timeSinceJumpInput > cData.maxBufferedJump)
            {
                timeSinceJumpInput = 0f;
               isbufferedJumping = false;
                //Debug.Log("NO Buffered Jump !");
            }
        }

        if(charac.collisions.below == false)
        {
            //Debug.Log("DANS LES AIRS !");

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

        if(Input.GetKeyDown("joystick button 0") && talkingToNPC == false && makeOffering == false) 
        {
            if(dialJustEnded == true)
            {
                dialJustEnded = false;
            } 
            else 
            {

                charac.Jump();
                charac.jumped = true;


                //Debug.Log("INPUT JUMP !");
                charac.Jump();

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
        

        if (Input.GetKeyDown("joystick button 0") && talkingToNPC == true && isAlreadyTalking == false)
        {
            isAlreadyTalking = true;
            dTrigger.TriggerDialogue();
            Debug.Log("Wesh");
        }

        if (Input.GetKeyDown("joystick button 0") && makeOffering == true)
        {
            PlayerPrefs.SetInt("nbCorn", pStatus.Corn);
            PlayerPrefs.SetInt("valueCorn", sManager.cornValue);

            PlayerPrefs.SetInt("nbCacao", pStatus.Cacao);
            PlayerPrefs.SetInt("valueCacao", sManager.cacaoValue);

            PlayerPrefs.SetInt("nbDeath", pStatus.deaths);
            PlayerPrefs.SetInt("valueDeath", sManager.deathValue);

            var sec = AXD_TimeManager.GetSeconds();
            PlayerPrefs.SetInt("seconds", sec);

            var score = AXD_ScoreManager.GetScore();
            PlayerPrefs.SetInt("score", score);

            SceneManager.LoadScene("Menu_Score");
        }


            if (Input.GetKeyUp("joystick button 0")) 
        {
            //Debug.Log("STOP JUMP ! ");
            charac.EndJump();
         }

        // POUR LE SWITCH DE MONDE

        //World Switch = Right Bumper
        if (Input.GetButtonDown("World Switch"))
        {
            pStatus.ChangeWorld();
        }

        // Charge l'écran du score après la fin du niveau. MODE DEBUG pour l'input !

        /*if (Input.GetKey("o"))
        {

            PlayerPrefs.SetInt("nbCorn", pStatus.Corn);
            PlayerPrefs.SetInt("valueCorn", sManager.cornValue);

            PlayerPrefs.SetInt("nbCacao", pStatus.Cacao);
            PlayerPrefs.SetInt("valueCacao", sManager.cacaoValue);

            PlayerPrefs.SetInt("nbDeath", pStatus.deaths);
            PlayerPrefs.SetInt("valueDeath", sManager.deathValue);

            var sec = AXD_TimeManager.GetSeconds();
            PlayerPrefs.SetInt("seconds", sec);

            var score = AXD_ScoreManager.GetScore();
            PlayerPrefs.SetInt("score", score);


            SceneManager.LoadScene("Menu_Score");
        }*/
    }
}
