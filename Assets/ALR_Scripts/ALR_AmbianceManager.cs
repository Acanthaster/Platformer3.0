using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALR_AmbianceManager : MonoBehaviour
{

    [SerializeField] AudioClip[] ambianceClips;
    [SerializeField] float minTimer;
    [SerializeField] float maxTimer;
    [SerializeField] float minSoundFx;
    [SerializeField] float maxSoundFx;
    [SerializeField] float minVolume;
    [SerializeField] float maxVolume;


    AudioClip[] playableClips;
    AudioSource audioSource;

    private float randAmountSeconds;
    private float randTimer;
    private float randNbEffect;
    private float randVolume;

    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        randAmountSeconds = UnityEngine.Random.Range(minTimer, maxTimer);
        randTimer = randAmountSeconds;
        randNbEffect = UnityEngine.Random.Range(minSoundFx, maxSoundFx);
    }

    void FixedUpdate()
    {

       // Debug.Log("Random Timer : " + randTimer);
        if(randTimer > 0)
        {
            randTimer -= 1 * Time.deltaTime;

        } else if (randTimer <= 0)
        {
            PlayAmbiance();
            randAmountSeconds = UnityEngine.Random.Range(minTimer, maxTimer);
            randTimer = randAmountSeconds;
        }
        
    }


    private void PlayAmbiance ()
    {
       
        for (int i = 0; i < randNbEffect; i++)
        {
            AudioClip clip = GetRandomClip(ambianceClips);
            randVolume = UnityEngine.Random.Range(minVolume, maxVolume);
            //Debug.Log("Random Volume : " + randVolume);
            audioSource.volume = randVolume;
            audioSource.PlayOneShot(clip);
        }

    }


    private AudioClip GetRandomClip(AudioClip[] arr)
    {
        return arr[UnityEngine.Random.Range(0, arr.Length)];

    }

    // Update is called once per frame

}
