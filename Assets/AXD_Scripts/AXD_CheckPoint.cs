using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_CheckPoint : MonoBehaviour
{
    private Animator anim;
    public float YGround;
    public bool activated;
    // Start is called before the first frame update
    void Start()
    {
        activated = false;
        anim = GetComponent<Animator>();
        RaycastHit2D hit = Physics2D.Raycast(transform.position,Vector2.down,Mathf.Infinity,LayerMask.GetMask("TempleGround"));
        //Debug.Log("Collider : " + hit.collider+"\nDistance : " + hit.distance);
        YGround = hit.distance;
    }
    
    public float GetYAboveGround(BoxCollider2D collider)
    {
        Debug.Log("YGround : " + YGround+"\nTaille Sprite : "+ (collider.size.y / 2));
        return transform.position.y - YGround + collider.size.y/2;
    }

    public void Activate()
    {
        if (!activated)
        {
            anim.Play("Anim_Checkpoint");
        }
    }

}
