using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_CheckPoint : MonoBehaviour
{
    private Animator anim;
    public float YGround;
    public bool activated;
    private ALR_SoundManager soundManager;
    // Start is called before the first frame update

    private void Awake()
    {
        soundManager = FindObjectOfType<ALR_SoundManager>();
        activated = false;
        anim = GetComponent<Animator>();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("TempleGround"));
        //Debug.Log("This : " + this.name + " Collider : " + hit.collider + "\nDistance : " + hit.distance);
        YGround = hit.distance;
    }
    
    public float GetYAboveGround(BoxCollider2D collider)
    {
        return transform.position.y - YGround + collider.size.y/2;
    }

    public void Activate()
    {     
            soundManager.CheckingSound();
            anim.Play("Anim_CheckpointOn");
            activated = true;
    }

    public void playCheckSound()
    {
        soundManager.CheckingSound();
    }

    public void Desactivated()
    {
        //Debug.Log("Desactivated");
        activated = false;
    }

}
