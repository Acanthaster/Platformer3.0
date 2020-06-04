using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALR_SoundAltar : MonoBehaviour
{

    [SerializeField] AudioClip[] altarClips;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void BlowSound()
    {
        audioSource.PlayOneShot(altarClips[0]);

    }

    private void ShimmerOneSound()
    {
        audioSource.PlayOneShot(altarClips[1]);

    }
    private void ShimmerTwoSound()
    {
        audioSource.PlayOneShot(altarClips[2]);

    }

    private void RumbleSound()
    {
        audioSource.PlayOneShot(altarClips[3]);

    }

    private void AltarSound()
    {
        audioSource.PlayOneShot(altarClips[4]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
