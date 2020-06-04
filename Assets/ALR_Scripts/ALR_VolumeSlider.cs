using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ALR_VolumeSlider : MonoBehaviour
{

    private Slider mySlider;

    private float saveValue;
    private AudioSource[] allAudioSources;

    private float[] allOriginalVolume;
    // Start is called before the first frame update
    void Awake()
    {
        mySlider = GetComponent<Slider>();
        allAudioSources = FindObjectsOfType<AudioSource>();
        allOriginalVolume = new float[allAudioSources.Length];
        //Debug.Log("Audio Sources" + allAudioSources.Length);
        //Debug.Log("Audio Sources " + allAudioSources[0] + " : Volume is : " + allAudioSources[0].volume);


        /*if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", mySlider.value);
        }*/
        //InitValue();
        //Debug.Log(" Volume " + mySlider.value);
 
        
    }

    void Start()
    {
        InitValue();
        saveValue = mySlider.value;
    }

        // Update is called once per frame
    void Update()
    {
        if (mySlider.value != saveValue)
        {
           
            for (int i=0; i<allAudioSources.Length; i++)
            {
                allAudioSources[i].volume = allOriginalVolume[i] * mySlider.value;
            }

            saveValue = mySlider.value;
        }
    }

    void InitValue()
    {
        for (int i = 0; i < allAudioSources.Length; i++)
        {
            allOriginalVolume[i] = allAudioSources[i].volume;
            //Debug.Log("Audio Sources " + allAudioSources[i] + " : Volume is : " + allAudioSources[i].volume);
            //Debug.Log(" Volume de " + allAudioSources[i] + " est de " + allAudioSources[i].volume + " >>> " + allOriginalVolume[i]);
        }
    }
}
