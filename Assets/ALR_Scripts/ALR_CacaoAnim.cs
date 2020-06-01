using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALR_CacaoAnim : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator animator;
    private CircleCollider2D cacaoCollider;
    private static readonly string ANIMATION_COLLECT = "collect";
    void Start()
    {
        cacaoCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
    }

    public void Collect()
    {
        cacaoCollider.enabled = false;
        animator.SetTrigger(ANIMATION_COLLECT);
    }
    void DestroyMe ()
    {
        Destroy(this.gameObject);
    }
}
