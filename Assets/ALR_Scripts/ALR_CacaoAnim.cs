using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALR_CacaoAnim : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator animator;
    private static readonly string ANIMATION_COLLECT = "collect";
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Collect()
    {
        animator.SetTrigger(ANIMATION_COLLECT);
    }
    void DestroyMe ()
    {
        Destroy(this.gameObject);
    }
}
