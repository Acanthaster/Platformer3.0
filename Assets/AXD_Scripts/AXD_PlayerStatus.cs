using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_PlayerStatus : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D myCollider;
    public Camera mainCamera;
    float invincible;
    float invincibilityCoolDown;
    public bool dead;
    public int deaths;
    public bool resetUI;
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
        anim = GetComponent<Animator>();
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

    public void TakeDamage()
    {
        if (Time.time > invincible)
        {
            HealthPoint--;
            invincible = Time.time + invincibilityCoolDown;
        }
        if (HealthPoint <= 0)
        {
            Die();
        }
        else
        {
            anim.SetTrigger("damage");
        }
    }

    public void ChangeWorld()
    {
        LivingWorld = !LivingWorld;
    }
    
    public void Die()
    {
        Debug.Log("Die");
        if (!dead)
        {
            StartCoroutine("Dying");
        }
        dead = true;
    }

    IEnumerator Dying()
    {
        anim.SetTrigger("death");
        //anim.Play("Anim_Death");
        yield return new WaitForSeconds(1);
        StartCoroutine("Respawning");
    }

    IEnumerator Respawning()
    {
        anim.SetTrigger("respawn");
        mainCamera.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        this.transform.position = LastCheckpoint;
        mainCamera.transform.position = LastCheckpoint;
        mainCamera.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        HealthPoint = MaxHealthPoint;
        deaths++;
        //anim.Play("Anim_Respawn");
        yield return new WaitForSeconds(1);
        dead = false;
        resetUI = true;
    }
}
