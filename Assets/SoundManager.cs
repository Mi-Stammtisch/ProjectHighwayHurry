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
    [SerializeField] GameObject SoundSourece;
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

    public void PlaySound(AudioClip clip, float volume = 1)
    {
        GameObject SoundSoureces = Instantiate(SoundSourece, transform.position, Quaternion.identity);
        SoundSoureces.GetComponent<AudioSource>().clip = clip;
        SoundSoureces.GetComponent<AudioSource>().volume = volume;
        SoundSoureces.GetComponent<AudioSource>().Play();
        Destroy(SoundSoureces, clip.length + 1);
    }

    

}
