using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions { up, right, down, left };
public class AXD_Launcher : MonoBehaviour
{
    private Animator animator;
    public Transform toLaunch;
    public float launchingRythm;
    private float lastLaunch;
    public Directions direction;

    private static readonly string ANIMATION_SHOOT = "shoot";

    // Start is called before the first frame update
    void Start()
    {
        lastLaunch = 0;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > lastLaunch + launchingRythm)
        {
            Launch();
            lastLaunch = Time.time;
        }
    }

    void Launch()
    {
        animator.SetTrigger(ANIMATION_SHOOT);
        Transform arrow = Instantiate(toLaunch, transform);
        arrow.GetComponent<AXD_Arrow>().SetDirection(direction);
    }
}
