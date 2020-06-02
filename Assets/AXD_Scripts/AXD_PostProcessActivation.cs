using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class AXD_PostProcessActivation : MonoBehaviour
{
    public AXD_PlayerStatus pStatus;
    private PostProcessVolume ppVolume;

    private void Awake()
    {
        ppVolume = GetComponent<PostProcessVolume>();
    }
    void Update()
    {
        if (this.CompareTag("PostProLife"))
        {
            if(pStatus.LivingWorld && !ppVolume.enabled)
            {
                ppVolume.enabled = true;
            }
            else if(!pStatus.LivingWorld && ppVolume.enabled)
            {
                ppVolume.enabled = false;
            }
        }else if (this.CompareTag("PostProDeath"))
        {
            if(!pStatus.LivingWorld && !ppVolume.enabled)
            {
                ppVolume.enabled = true;
            }else if(pStatus.LivingWorld && ppVolume.enabled)
            {
                ppVolume.enabled = false;
            }
        }
    }
}
