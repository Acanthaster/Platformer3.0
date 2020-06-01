using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_FireRain : MonoBehaviour
{
    public List<Sprite> sprites;
    public AXD_PlayerStatus pStatus;
    private BoxCollider2D collider;
    SpriteRenderer display;
    public bool damaging;
    bool world;

    private Animator anim;
    private static readonly string ANIMATION_FIRE = "FireRain";
    private static readonly string ANIMATION_WATER = "WaterRain";
    // Start is called before the first frame update
    void Start()
    {
        world = true;
        damaging = true;
        display = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pStatus.LivingWorld != world)
        {
            world = pStatus.LivingWorld;
            if (world)
            {
                if (!collider.enabled)
                {
                    collider.enabled = true;
                }
                damaging = true;
                display.sprite = sprites[0];

                anim.SetTrigger(ANIMATION_FIRE);
            }
            else
            {
                if (collider.enabled)
                {
                    collider.enabled = false;
                }
                damaging = false;
                display.sprite = sprites[1];

                anim.SetTrigger(ANIMATION_WATER);
            }
        }
    }
}
