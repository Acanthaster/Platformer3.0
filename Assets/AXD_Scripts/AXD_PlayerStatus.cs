using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_PlayerStatus : MonoBehaviour
{
    float invincible;
    float invincibilityCoolDown;
    public bool dead;
    public int deaths;
    [Header("World")]
    public bool LivingWorld;
    public Vector2 LastCheckpoint;

    [Header("Health")]
    public int MaxHealthPoint;
    public int HealthPoint;

    [Header("Collectibles")]
    public int Corn;
    public int Cacao;

    private void Awake()
    {
        deaths = 0;
        dead = false;
        LastCheckpoint = this.transform.position;
        invincibilityCoolDown = 1f;
        invincible = Time.deltaTime;
        LivingWorld = true;
        HealthPoint = MaxHealthPoint = 5;
        Corn = 0;
        Cacao = 0;
    }

    private void Update()
    {
        if (HealthPoint <= 0)
        {
            this.transform.position = LastCheckpoint;
            HealthPoint = MaxHealthPoint;
            deaths++;
            dead = true;
        }
        Debug.Log("Seconds : " + (AXD_TimeManager.GetSeconds()));
    }

    public void TakeDamage()
    {
        if (Time.time > invincible)
        {
            HealthPoint--;
            invincible = Time.time + invincibilityCoolDown;
        }
    }

    public void ChangeWorld()
    {
        LivingWorld = !LivingWorld;
    }
    
}
