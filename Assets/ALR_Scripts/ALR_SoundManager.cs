using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALR_SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip[] steps;
    [SerializeField] AudioClip[] jumpClips;
    [SerializeField] AudioClip[] landingClips;
    [SerializeField] AudioClip[] slidingClips;
    [SerializeField] AudioClip[] collectClips;
    [SerializeField] AudioClip[] switchClips;
    [SerializeField] AudioClip[] whisperClips;
    AudioSource audioSource;

    private ALR_CustomCharacterController cCharacController;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cCharacController = GetComponent<ALR_CustomCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Step()
    {
        AudioClip clip = GetRandomClip(steps);
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(clip);

    }

    private void JumpSound()
    {
        if(!audioSource.isPlaying)
        {
            for (int i = 0; i < jumpClips.Length; i++)
            {
                audioSource.PlayOneShot(jumpClips[i]);
            }
           
        }
      
    }

    private void LandingSound()
    {
        audioSource.PlayOneShot(landingClips[0]);
      
    }

    private void SlidingWall()
    {
        for (int i = 0; i < slidingClips.Length; i++)
        {
            audioSource.PlayOneShot(slidingClips[i]);
        }

    }

    public void SwitchingWorld()
    {
        for (int i = 0; i < switchClips.Length; i++)
        {
            audioSource.PlayOneShot(switchClips[i]);
        }

    }

    public void Whispering()
    {

        AudioClip clip = GetRandomClip(whisperClips);
        audioSource.PlayOneShot(clip);
    }

    public void CollectSound(string tag)
    {
       
        if(tag == "Cacao")
        {
            audioSource.PlayOneShot(collectClips[1]);
            audioSource.PlayOneShot(collectClips[2]);
        } else if (tag == "Corn")
        {
            audioSource.PlayOneShot(collectClips[0]);
        }

    }

    private AudioClip GetRandomClip(AudioClip[] arr)
    {
        return arr[UnityEngine.Random.Range(0, arr.Length)];
         
    }
}
