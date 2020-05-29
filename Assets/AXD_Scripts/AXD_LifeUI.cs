﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AXD_LifeUI : MonoBehaviour
{

    public Image icon;
    public AXD_LifeSprites sp;
    public AXD_PlayerStatus status;
    GameObject lifePoints;
    int lastHP;
    void Start()
    {
        lifePoints = GameObject.Find("LifePoints");
        lastHP = status.MaxHealthPoint;
    }
    private void FixedUpdate()
    {
        UI_Update();
    }
    public void UI_Update()
    {
        //Si les PV réels sont inférieurs aux PV affichés
        if (status.HealthPoint <= lastHP)
        {
            //Pour chaque PV qui sont mal affichés (affichés rempli alors qu'ils doivent être vide)
            for (int i = status.HealthPoint; i < lastHP; i++)
            {
                GameObject obj = lifePoints.transform.GetChild(i).gameObject;
                if (obj != null)
                {
                    obj.GetComponent<AXD_LifeSprites>().ChangeSprite(false);
                }
                lastHP = i;

            }

        }//Si les PV réels sont suppérieurs aux PV indiqués
        else if (status.HealthPoint > lastHP)
        {
            for (int i = lastHP; i < status.HealthPoint; i++)
            {
                GameObject obj = lifePoints.transform.GetChild(i).gameObject;
                if (obj != null)
                {
                    obj.GetComponent<AXD_LifeSprites>().ChangeSprite(true);

                }
                lastHP = i;

            }
        } else if (status.dead)
        {
            for (int i = 0; i < status.MaxHealthPoint; i++)
            {
                GameObject obj = lifePoints.transform.GetChild(i).gameObject;
                if (obj != null)
                {
                    obj.GetComponent<AXD_LifeSprites>().ChangeSprite(true);
                }
            }

        }
    }
}
