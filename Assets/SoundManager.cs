using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //Instance 
    public static SoundManager Instance;

    
    private void Awake()
    {
        Instance = this;        
    }
    

    //AudioSource
    public AudioSource AcellerateSource;
[SerializeField] bool PlayMusic = true; 
    //AudioSource
    public AudioSource MusicSource;

    public AudioClip MusciIntroClip;
    public AudioClip MusicLoopClip;
    public AudioClip MusicIntroFinishClip;


    public void PlayAcellerateSound()
    {
        AcellerateSource.Play();
    }

    
    private void Start()
    {
        if (PlayMusic)
         StartCoroutine(MainTrack());
    }

    IEnumerator MainTrack()
    {

        MusicSource.clip = MusciIntroClip;
        MusicSource.Play();
        MusicSource.loop = false;
        yield return new WaitForSeconds(MusciIntroClip.length);
        MusicSource.clip = MusicLoopClip;
        MusicSource.Play();
        MusicSource.loop = true;
    }

    public float PlayIntroFinishSound()
    {
        MusicSource.clip = MusicIntroFinishClip;
        MusicSource.Play();
        MusicSource.loop = false;
        return MusicIntroFinishClip.length;
    }

    

}
