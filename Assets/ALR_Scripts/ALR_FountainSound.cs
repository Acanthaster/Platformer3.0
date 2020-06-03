using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALR_FountainSound : MonoBehaviour
{
     AudioSource audioSource;
    [SerializeField] AudioClip[] fountainClips;


    private 
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        //Debug.Log(audioSource.clip);
    }

    public void playFountainSound(bool world)
    {
        if (world == true)
        {
            //Debug.Log("PLAY LAVA SOUND !");
            audioSource.Stop();
            audioSource.clip = fountainClips[0];
            audioSource.Play();
        } else if (world == false)
        {
            //Debug.Log("PLAY WATER SOUND !");
            audioSource.Stop();
            audioSource.clip = fountainClips[1];
            audioSource.Play();
        }
    }

   

   
}
