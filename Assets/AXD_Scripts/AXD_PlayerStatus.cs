using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_PlayerStatus : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D myCollider;
    public Camera mainCamera;
    public ALR_CharacterData cData;
    public ALR_PlayerInputHandler pInput;
    public ALR_CustomCharacterController pController;
    float invincible;
    public float invincibilityCoolDown;
    public bool dead;
    public int deaths;
    public bool resetUI;
    public bool lockedInput;

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
        cData = GetComponent<ALR_CharacterData>();
        anim = GetComponent<Animator>();
        pInput = GetComponent<ALR_PlayerInputHandler>();
        pController = GetComponent<ALR_CustomCharacterController>();
        deaths = 0;
        dead = false;
        LastCheckpoint = this.transform.position;
        mainCamera.transform.position = transform.position;
        invincibilityCoolDown = 1f;
        invincible = Time.deltaTime;
        LivingWorld = true;
        HealthPoint = MaxHealthPoint;
        Corn = 0;
        Cacao = 0;
    }

    public void TakeDamage()
    {
        if (Time.time > invincible)
        {
            HealthPoint--;
            invincible = Time.time + invincibilityCoolDown;
            pController.takingDamage = true;
        }
        if (HealthPoint <= 0)
        {
            Die();
        }
        else
        {
            pInput.lockInput = true;
            anim.SetTrigger("damage");
        }
    }

    public void ChangeWorld()
    {
        LivingWorld = !LivingWorld;
    }
    
    public void Die()
    {
        lockedInput = true;
        if (!dead)
        {
            StartCoroutine("Dying");
        }
        dead = true;
    }

    IEnumerator Dying()
    {
        anim.SetTrigger("death");
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
        yield return new WaitForSeconds(2);
        dead = false;
        lockedInput = false;
        resetUI = true;
    }
}
