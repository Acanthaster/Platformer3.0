using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALR_FireHead : MonoBehaviour
{
    public List<Sprite> sprites;
    public AXD_PlayerStatus pStatus;
    SpriteRenderer display;

    bool world;

    void Start()
    {
        world = true;
        display = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pStatus.LivingWorld != world)
        {
            world = pStatus.LivingWorld;
            if (world)
            {
                display.sprite = sprites[0];
            }
            else
            {
                display.sprite = sprites[1];
            }
        }
    }
}