using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALR_MenuInputHandler : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private bool isPaused;
    private bool isPausedMenu;
    [SerializeField] ALR_ButtonAction targetButton;

    private void Update()
    {
        if(isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        /*if(FindObjectOfType<ALR_PlayerInputHandler>().isAlreadyTalking == true)
        {
            isPaused = true;
        }
        else
        {
            isPaused = false;
        }*/



       if (Input.GetButtonDown("Pause Menu"))
        {
            isPausedMenu = !isPausedMenu;
            //isPaused = !isPaused;
        }

       if (isPausedMenu)
        {
            isPaused = true;
            ActivatePause();
        }
       else
        {
            DeactivatePause();
        }

    }

    void ActivatePause()
    {
        //Time.timeScale = 0;
        //AudioListener.pause = true;           //pour le son plus tard
        targetButton.isFromGame();
        pauseMenuUI.SetActive(true);
        
    }

    public void DeactivatePause()
    {
        //Time.timeScale = 1;
        //AudioListener.pause = false;          //pour le son plus tard
        pauseMenuUI.SetActive(false);
        isPausedMenu = false;
        isPaused = false;
    }

}
