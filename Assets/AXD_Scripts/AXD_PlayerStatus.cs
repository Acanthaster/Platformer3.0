using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_PlayerStatus : MonoBehaviour
{
    public Camera mainCamera;
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
        mainCamera.transform.position = transform.position;
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
            mainCamera.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.position = LastCheckpoint;
            mainCamera.transform.position = LastCheckpoint;
            mainCamera.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            HealthPoint = MaxHealthPoint;
            deaths++;
            dead = true;
        }
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
    
    public void Die()
    {
        HealthPoint = 0;
    }
}
