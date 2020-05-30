using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AXD_LifeUI : MonoBehaviour
{

    public Image icon;
    public AXD_LifeSprites sp;
    public AXD_PlayerStatus pStatus;
    GameObject lifePoints;
    private bool resetingUI;
    int lastHP;
    void Start()
    {
        resetingUI = false;
        lifePoints = GameObject.Find("LifePoints");
        lastHP = pStatus.MaxHealthPoint;
    }
    private void FixedUpdate()
    {
        UI_Update();
        if(pStatus.resetUI && !resetingUI)
        {
            resetingUI = true;
            ResetUI();
        }
    }
    public void UI_Update()
    {
        //Si les PV réels sont inférieurs aux PV affichés
        if (pStatus.HealthPoint <= lastHP)
        {
            //Pour chaque PV qui sont mal affichés (affichés rempli alors qu'ils doivent être vide)
            for (int i = pStatus.HealthPoint; i < lastHP; i++)
            {
                GameObject obj = lifePoints.transform.GetChild(i).gameObject;
                if (obj != null)
                {
                    obj.GetComponent<AXD_LifeSprites>().ChangeSprite(false);
                }
                lastHP = i;

            }

        }//Si les PV réels sont suppérieurs aux PV indiqués
        else if (pStatus.HealthPoint > lastHP)
        {
            for (int i = lastHP; i < pStatus.HealthPoint; i++)
            {
                GameObject obj = lifePoints.transform.GetChild(i).gameObject;
                if (obj != null)
                {
                    obj.GetComponent<AXD_LifeSprites>().ChangeSprite(true);

                }
                lastHP = i;

            }
        }
    }

    public void ResetUI()
    {
        for (int i = 0; i < pStatus.MaxHealthPoint; i++)
        {
            GameObject obj = lifePoints.transform.GetChild(i).gameObject;
            if (obj != null)
            {
                obj.GetComponent<AXD_LifeSprites>().ChangeSprite(true);
            }
        }
        lastHP = pStatus.MaxHealthPoint;
        pStatus.resetUI = false;
        resetingUI = false;
    }

}
