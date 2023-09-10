using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntController : MonoBehaviour
{
    [SerializeField] private List<AnimationClip> stuntClips;
    [SerializeField] GameObject animationObject;
    public AnimationClip stuntClip;

    public static StuntController Instance;
    public bool canStunt = false;


    private void Awake()
    {
        Instance = this;
    }

    public void OnStunt()
    {
        if (canStunt)
        {
            if (stuntClip != null)
            {
                animationObject.GetComponent<Animation>().Play(stuntClip.name);
            }
            else
            {
                animationObject.GetComponent<Animation>().Play(stuntClips[Random.Range(0, stuntClips.Count)].name);
            }
        }
    }


}
