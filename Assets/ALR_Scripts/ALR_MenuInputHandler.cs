using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALR_MenuInputHandler : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] ALR_ButtonAction targetButton;
    public ALR_PlayerInputHandler pInputHandler;


    private void start()
    {
       
        Debug.Log(pInputHandler);
    }

    private void Update()
    {

    }

    public void TimeStop(bool timeState)
    {
        if(timeState)
        {
            Time.timeScale = 0;
        } else
        {
            Time.timeScale = 1;
        }

    }

    public void ActivatePause()
    {
        TimeStop(true);
        //AudioListener.pause = true;           //pour le son plus tard
        targetButton.isFromGame();
        pauseMenuUI.SetActive(true);
        
    }

    public void DeactivatePause()
    {

        //TimeStop(false);
        //AudioListener.pause = false;          //pour le son plus tard
       
        pInputHandler.quitPauseMenu = true;
        pauseMenuUI.SetActive(false);
    }

}
