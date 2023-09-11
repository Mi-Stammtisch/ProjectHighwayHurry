using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntController : MonoBehaviour
{
    [SerializeField] private List<AnimationClip> stuntClips;
    [SerializeField] GameObject animationObject;
    public AnimationClip stuntClip;
    [SerializeField] AudioClip loopingSound;

    public static StuntController Instance;
    public bool canStunt = false;
    public bool CanAllWaysStunt = true;


    private void Awake()
    {
        Instance = this;
    }

    public void OnStunt()
    {
        if (canStunt )
        {
            if (stuntClip != null)
            {
                animationObject.GetComponent<Animation>().Play(stuntClip.name);
            }
            else
            {

                int random = Random.Range(0, stuntClips.Count);
                animationObject.GetComponent<Animation>().Play(stuntClips[random].name);
                if(random == 2) {
                    SoundManager.Instance.PlaySound(loopingSound, 0.6f);
                }
                if(CanAllWaysStunt){
                    StartCoroutine(StuntTimer());
                }
                else{
                    StartCoroutine(StuntTimer2());
                }

            }
        }
    }

    IEnumerator StuntTimer()
    {
        canStunt = false;
        yield return new WaitForSeconds(0.5f);
        Scoreboard.Instance.stuntBonus(5);
        canStunt = true;        
    }
    IEnumerator StuntTimer2()
    {
        canStunt = false;
        yield return new WaitForSeconds(0.5f);
        Scoreboard.Instance.stuntBonus(5);
             
    }


}
